using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AIController : CharacterClass
{
    [Header("Vision Cone"), SerializeField]
    private float coneRadius = 5f;
    [Range(0, 1)]
    public float angleth = 5f;

    [Header("Area Check"), SerializeField]
    private float areaRadius = 3f;

    [Header("Agro Check"), SerializeField]
    private float aggroRadius = 7f;

    [SerializeField]
    protected float _attackrange = 2f;

    protected float _attackTimer = 50;

    protected Transform player;

    private SphereCollider playerCheckCollider;

    public AIBrain currentAction;

    [SerializeField]
    protected float rotationSpeed = 5f;

    protected NavMeshAgent agent;

    [SerializeField]
    private HealthCollectible healthDrop;

    [SerializeField]
    private SoulCollectible soulDrop;

    [SerializeField]
    protected float healthDropChance;

    protected List<AISpot> spotList;
    protected bool foundSpots = false;

    [SerializeField]
    private int maxAmountInGroup;

    [SerializeField]
    protected float minIdleTime;  // Duration for idle state

    [SerializeField]
    protected float minIdleRadius;  // Radius within which the AI can roam while idling

    [Header("Soul Drop")]
    [SerializeField]
    private int soulAmountMaxTier1;
    [SerializeField]
    private int soulAmountMinTier1;
    [SerializeField]
    private int soulAmountMaxTier2;
    [SerializeField]
    private int soulAmountMinTier2;
    [SerializeField]
    private int soulAmountMaxTier3;
    [SerializeField]
    private int soulAmountMinTier3;

    [Header("HealthTier")]
    [SerializeField]
    private float baseHealth;
    [SerializeField]
    private float healthMultTier2 = 2.5f;
    [SerializeField]
    private float healthMultTier3 = 5f;



    [Header("Boid Settings")]
    public float boidNeighborRadius = 5f;
    public float separationWeight = 1.5f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = _attackrange;
        initialSpeed = _speed;

        playerCheckCollider = GetComponent<SphereCollider>();
        playerCheckCollider.radius = aggroRadius;

        SetHealth();

        StartCoroutine(OnIdle());
    }

    protected virtual void Update()
    {
        _attackTimer += Time.deltaTime;

    }
    #region AI Brain

    public enum AIBrain
    {
        Idle,
        Patrol,
        Chase,
        Combat,
        Die
    }

    protected void SetBrain(AIBrain newState)
    {
        if (currentAction == newState) return;
        currentAction = newState;

        StopCoroutine(OnIdle());
        StopCoroutine(OnPatrol());
        StopCoroutine(OnCombat());
        StopCoroutine(OnChasing());

        switch (currentAction)
        {
            case AIBrain.Idle:
                StartCoroutine(OnIdle());
                break;
            case AIBrain.Die:
                OnDie();
                break;
            case AIBrain.Patrol:
                StartCoroutine(OnPatrol());
                break;
            case AIBrain.Chase:
                StartCoroutine(OnChasing());
                break;
            case AIBrain.Combat:
                StartCoroutine(OnCombat());
                break;
            default:
                break;
        }
    }
    protected virtual IEnumerator OnIdle()
    {
        float startTime = Time.time;

        float idleTime = UnityEngine.Random.Range(minIdleTime, minIdleTime * 2);
        float idleRadius = UnityEngine.Random.Range(minIdleRadius, minIdleRadius * 2);


        while (AIBrain.Idle == currentAction && Time.time - startTime < idleTime)
        {
            Vector3 finalPosition = this.transform.position + new Vector3(UnityEngine.Random.Range(0, idleRadius), 0f, UnityEngine.Random.Range(0, idleRadius));

            agent.SetDestination(finalPosition);

            while (Vector3.Distance(transform.position, finalPosition) > agent.stoppingDistance + 2f)
            {
                if (IsPlayerInView())
                {
                    agent.ResetPath();
                    SetBrain(AIBrain.Chase);
                    yield break;
                }

                yield return null;
            }

            // Random wait time
            float waitTime = UnityEngine.Random.Range(0.5f, 1.5f);


            yield return new WaitForSeconds(waitTime);


        }

        SetBrain(AIBrain.Patrol);
    }

    protected virtual IEnumerator OnCombat()
    {
        while (AIBrain.Combat == currentAction)
        {
            //anim.SetTrigger("Attack");

            Vector3 target = player.position - transform.position;
            target.y = 0;
            agent.speed = _speed;
            if (Vector3.Distance(this.transform.position, player.position) > _attackrange)
            {

                SetBrain(AIBrain.Chase);
                yield break;
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(target);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                if (IsLookingAtTarget(target.normalized))
                {
                    AttackPlayer();
                }
            }
            yield return null;
        }
        yield return null;
    }

    bool IsLookingAtTarget(Vector3 directionToTarget)
    {
        float dotProduct = Vector3.Dot(transform.forward, directionToTarget);
        return dotProduct >= 0.95f;
    }
    protected virtual IEnumerator OnPatrol()
    {
        if (!foundSpots)
            FindAISpots();


        while (AIBrain.Patrol == currentAction)
        {
            if (spotList.Count > 0)
            {
                agent.speed = _speed;

                AISpot targetSpot = spotList[UnityEngine.Random.Range(0, spotList.Count)];
                Quaternion targetRotation = Quaternion.LookRotation(targetSpot.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                agent.SetDestination(targetSpot.transform.position);

                while (Vector3.Distance(transform.position, targetSpot.transform.position) > agent.stoppingDistance)
                {
                    if (IsPlayerInView())
                    {
                        agent.ResetPath();
                        SetBrain(AIBrain.Chase);
                        yield break;
                    }

                    yield return null;
                }

                SetBrain(AIBrain.Idle);
                yield break;

            }
        }

        SetBrain(AIBrain.Idle);
        yield break;

    }
    protected virtual IEnumerator OnChasing()
    {

        while (AIBrain.Chase == currentAction)
        {
            while (Vector3.Distance(transform.position, player.position) >= _attackrange)
            {
                agent.speed = _speed * SpeedModifier;
                agent.SetDestination(player.position);


                Vector3 target = player.position - transform.position;
                target.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(target);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


                if (LoseAgro(player.position))
                {
                    agent.ResetPath();
                    SetBrain(AIBrain.Idle);
                    yield break;
                }

                yield return null;
            }

            SetBrain(AIBrain.Combat);
            yield break;

        }
        yield return null;
    }
    protected virtual IEnumerator OnGetHit()
    {
        yield return null;
    }
    protected virtual void OnDie()
    {
        StopAllCoroutines();
        agent.speed = 0f;
        agent.ResetPath();

        DropSouls();
        if (UnityEngine.Random.Range(0f, 1f) <= healthDropChance)
        {
            HealthCollectible health = Instantiate(healthDrop, transform.position + Vector3.forward, Quaternion.identity);
            health.tier = tier;
        }

        Destroy(this.gameObject, 1f);
    }

    protected virtual void DropSouls()
    {
        int nSoulDrops = 0;
        switch (tier)
        {
            case 1:
                nSoulDrops = UnityEngine.Random.Range(soulAmountMinTier1, soulAmountMaxTier1);
                break;
            case 2:
                nSoulDrops = UnityEngine.Random.Range(soulAmountMinTier2, soulAmountMaxTier2);
                break;
            case 3:
                nSoulDrops = UnityEngine.Random.Range(soulAmountMinTier3, soulAmountMaxTier3);
                break;
            default:
                break;
        }

        for (int i = 0; i < nSoulDrops; i++)
        {
            SoulCollectible soul = Instantiate(soulDrop, transform.position, Quaternion.identity);
            soul.tier = tier;
        }

    }

    protected virtual void SetHealth()
    {
        switch (tier)
        {
            case 1:
                health = baseHealth;
                break;
            case 2:
                health = baseHealth * healthMultTier2;
                break;
            case 3:
                health = baseHealth * healthMultTier3;
                break;
            default:
                break;
        }

        maxHealth = health;
    }

    public virtual void AttackPlayer() { }

    public override void GetHit(float damageAmount, GameObject attacker, SpellBook spell)
    {
        base.GetHit(damageAmount, attacker, spell);

        if (attacker.GetComponent<PlayerController>() && isAlive && (currentAction == AIBrain.Idle || currentAction == AIBrain.Patrol))
        {
            player = attacker.transform;
            StopCoroutine(OnPatrol());
            StopCoroutine(OnIdle());
            SetBrain(AIBrain.Chase);
        }

    }

    protected override void TakeDamage(float damage)
    {
        if (isAlive)
        {
            health -= damage;
        }
        if (health <= 0 && isAlive)
        {
            isAlive = false;
            SetBrain(AIBrain.Die);
        }

    }
    #endregion

    #region Check Functions
/*    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;
            if (VisionConeCheck(player.position))
                Handles.color = Color.red;
            if (AreaCheck(player.position))
                Handles.color = Color.blue;
            if (LoseAgro(player.position))
                Handles.color = Color.black;
            Gizmos.color = Handles.color;
        }
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, coneRadius);
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, areaRadius);
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, aggroRadius);

        float p = angleth;
        float x = Mathf.Sqrt(1 - p * p);

        Vector3 vRight = new Vector3(x, 0, p) * coneRadius;
        Vector3 vLeft = new Vector3(-x, 0, p) * coneRadius;

        Gizmos.DrawRay(Vector3.zero, vLeft);
        Gizmos.DrawRay(Vector3.zero, vRight);
    }
*/
    private Vector3 GetFlatDirection(Vector3 targetPosition, out float flatDistance)
    {
        Vector3 vecToTargetWorld = targetPosition - transform.position;
        Vector3 vecToTarget = transform.InverseTransformVector(vecToTargetWorld);
        Vector3 flatDir = vecToTarget;
        flatDir.y = 0;
        flatDistance = flatDir.magnitude;
        return flatDir;
    }

    public bool AreaCheck(Vector3 position)
    {
        float flatDistance;
        GetFlatDirection(position, out flatDistance);

        // Distance check
        return flatDistance <= areaRadius;
    }

    public bool VisionConeCheck(Vector3 position)
    {
        float flatDistance;
        Vector3 flatDir = GetFlatDirection(position, out flatDistance);
        flatDir.Normalize();

        // Angle check
        if (flatDir.z < angleth)
            return false;

        // Distance check
        return flatDistance <= coneRadius;
    }

    public bool LoseAgro(Vector3 position)
    {
        float flatDistance;
        GetFlatDirection(position, out flatDistance);
        // Distance check
        return flatDistance > aggroRadius;
    }

    public bool IsPlayerInView()
    {
        if (player == null)
            return false;
        return VisionConeCheck(player.position) || AreaCheck(player.position);
    }

    protected virtual void FindAISpots()
    {
        if (GridManager.Instance.gridDone)
        {
            spotList = new List<AISpot>();
            spotList = GridManager.Instance.enemySpots[tier];
            foundSpots = true;
        }
        else
        {
            SetBrain(AIBrain.Idle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            player = other.GetComponent<PlayerController>().transform;
        }
    }
    #endregion

    #region Boid Behavior
    private void ApplyBoidBehavior()
    {
        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;

        int neighborCount = 0;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, boidNeighborRadius);
        foreach (var hitCollider in hitColliders)
        {
            AIController neighbor = hitCollider.GetComponent<AIController>();
            if (neighbor != null && neighbor != this && neighbor.currentAction == currentAction)
            {
                // Separation
                Vector3 toNeighbor = transform.position - neighbor.transform.position;
                separation += toNeighbor / toNeighbor.sqrMagnitude;

                // Alignment
                alignment += neighbor.agent.velocity;

                // Cohesion
                cohesion += neighbor.transform.position;

                neighborCount++;
            }
        }

        if (neighborCount > 0 && neighborCount <= 3)
        {
            // Average out values
            separation /= neighborCount;
            alignment /= neighborCount;
            cohesion /= neighborCount;

            // Calculate direction to center of mass of neighbors
            cohesion = (cohesion - transform.position).normalized;

            // Apply weights
            Vector3 boidDirection = (separation * separationWeight) + (alignment.normalized * alignmentWeight) + (cohesion * cohesionWeight);

            // Clamp the boid direction to max speed
            if (boidDirection.magnitude > Speed)
            {
                boidDirection = boidDirection.normalized * Speed;
            }

            // Apply the resulting boid direction to the AI
            if (boidDirection != Vector3.zero)
            {
                agent.velocity = boidDirection;
            }
        }

        // Debugging lines to visualize the boid behavior
        Debug.DrawLine(transform.position, transform.position + separation * separationWeight, Color.red);
        Debug.DrawLine(transform.position, transform.position + alignment.normalized * alignmentWeight, Color.blue);
        Debug.DrawLine(transform.position, transform.position + cohesion * cohesionWeight, Color.green);
    }
    #endregion

}

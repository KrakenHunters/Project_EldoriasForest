using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
    private float rotationSpeed = 5f;

    protected NavMeshAgent agent;

    [SerializeField]
    private HealthCollectible healthDrop;

    [SerializeField]
    private SoulCollectible soulDrop;

    [SerializeField]
    protected float healthDropChance;

    private List<AISpot> spotList;

    [SerializeField]
    private int maxAmountInGroup;

    [SerializeField]
    private float minIdleTime;  // Duration for idle state

    [SerializeField]
    private float minIdleRadius;  // Radius within which the AI can roam while idling

    [Header("Boid Settings")]
    public float boidNeighborRadius = 5f;
    public float separationWeight = 1.5f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = _attackrange;

        playerCheckCollider = GetComponent<SphereCollider>();
        playerCheckCollider.radius = aggroRadius;

        maxHealth = health;

        SetBrain(AIBrain.Idle);
    }

    protected virtual void Update()
    {
        _attackTimer += Time.deltaTime;
        agent.speed = _speed;

        if (currentAction == AIBrain.Patrol || currentAction == AIBrain.Idle)
        {
            //ApplyBoidBehavior();
        }
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
        //if (currentAction == newState) return;
        currentAction = newState;

        Debug.Log(currentAction.ToString());
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
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * idleRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, idleRadius, 1);
            Vector3 finalPosition = hit.position;

            agent.SetDestination(finalPosition);

            while (Vector3.Distance(transform.position, finalPosition) > agent.stoppingDistance)
            {
                if (IsPlayerInView())
                {
                    SetBrain(AIBrain.Chase);
                    StopCoroutine(OnIdle());
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
            Quaternion targetRotation = Quaternion.LookRotation(target);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, player.position) > _attackrange)
            {
                SetBrain(AIBrain.Chase);
                StopCoroutine(OnCombat());
                yield break;
            }
            else
            {
                AttackPlayer();
            }
            yield return null;
        }
        yield return null;
    }
    protected virtual IEnumerator OnPatrol()
    {

        FindAISpots();

        while (AIBrain.Patrol == currentAction)
        {
            if (spotList.Count > 0)
            {
                AISpot targetSpot = spotList[UnityEngine.Random.Range(0, spotList.Count)];
                Quaternion targetRotation = Quaternion.LookRotation(targetSpot.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                agent.SetDestination(targetSpot.transform.position);

                while (Vector3.Distance(transform.position, targetSpot.transform.position) > agent.stoppingDistance)
                {
                    if (IsPlayerInView())
                    {
                        SetBrain(AIBrain.Chase);
                        StopCoroutine(OnPatrol());
                        yield break;
                    }

                    yield return null;
                }

                // Check for nearby AIs
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaRadius);
                int aiCount = 0;
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.GetComponent<AIController>() && hitCollider.GetComponent<AIController>() != this)
                    {
                        aiCount++;
                    }
                }

                if (aiCount < maxAmountInGroup)
                {
                    SetBrain(AIBrain.Idle);
                    StopCoroutine(OnPatrol());
                    yield break;
                }
            }


            if (IsPlayerInView())
            {
                SetBrain(AIBrain.Chase);
                StopCoroutine(OnPatrol());
                yield break;
            }

            yield return null;
        }
        yield return null;
    }
    protected virtual IEnumerator OnChasing()
    {
        while (AIBrain.Chase == currentAction)
        {
            Vector3 target = player.position - transform.position;
            target.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(target);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            agent.SetDestination(player.position);

            while (Vector3.Distance(transform.position, player.position) > _attackrange)
            {
                if (LoseAgro(player.position))
                {
                    SetBrain(AIBrain.Idle);
                    StopCoroutine(OnChasing());
                    yield break;
                }

                yield return null;
            }

            SetBrain(AIBrain.Combat);
            StopCoroutine(OnChasing());
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

        Instantiate(soulDrop, transform.position, Quaternion.identity);

        if (UnityEngine.Random.Range(0f,1f) <= healthDropChance)
        {
            Instantiate(healthDrop, transform.position + Vector3.forward, Quaternion.identity);
        }
        
        Destroy(this.gameObject, 1f);
    }
    public virtual void AttackPlayer() { }

    public override void GetHit(int damageAmount, GameObject attacker, SpellBook spell)
    {
        base.GetHit(damageAmount, attacker, spell);

        if (attacker.GetComponent<PlayerController>())
        {
            player = attacker.transform;
            SetBrain(AIBrain.Chase);
        }

        if (health <= 0)
        {
            SetBrain(AIBrain.Die);
        }
    }
    #endregion

    #region Check Functions
    void OnDrawGizmos()
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

    void FindAISpots()
    {
        // Initialize spotList
        spotList = new List<AISpot>();

        switch(tier)
        {
            case 1: 
                spotList = GridManager.Instance.enemySpotsTier1;
                break;
            case 2:
                spotList = GridManager.Instance.enemySpotsTier2;
                break;
            case 3:
                spotList = GridManager.Instance.enemySpotsTier3;
                break;
            default:
                break;
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

using System;
using System.Collections;
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

    protected AIBrain currentAction;

    [SerializeField]
    private float rotationSpeed = 5f;

    protected NavMeshAgent agent;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = _attackrange;

        playerCheckCollider = GetComponent<SphereCollider>();
        playerCheckCollider.radius = aggroRadius;

        SetBrain(AIBrain.Patrol);
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
        StopAllCoroutines();
        switch (currentAction)
        {
            case AIBrain.Idle:
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
        while (AIBrain.Patrol == currentAction)
        {
            bool playerInView = IsPlayerInView();

            if (playerInView)
                SetBrain(AIBrain.Chase);

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

            if (Vector3.Distance(this.transform.position, player.position) > _attackrange)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                SetBrain(AIBrain.Combat);
                Debug.Log("Combat");
            }

            if (LoseAgro(player.position))
            {
                SetBrain(AIBrain.Patrol);
            }

            yield return null;
        }
        yield return null;
    }
    protected virtual IEnumerator OnGetHit()
    {
        yield return null;
    }
    protected virtual void OnDie()
    {
        Destroy(this.gameObject, 1f);
    }
    public virtual void AttackPlayer() { }

    public override void GetHit(int damageAmount, CharacterClass attacker)
    {
        if (attacker.GetComponent<PlayerController>())
        {
            base.GetHit(damageAmount, attacker);
            player = attacker.transform;
            SetBrain(AIBrain.Chase);
        }
        if (health <= 0)
        {
            OnDie();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            player = other.GetComponent<PlayerController>().transform;
        }
    }
    #endregion


}

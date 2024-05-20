using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class AIController : CharacterClass
{



    [Header("Vision Cone"), SerializeField]
    private float coneRadius = 5f;
    [Range(0, 1)]
    public float angleth = 5f;

    [Header("Area Check"), SerializeField]
    private float areaRadius = 3f;

    [Header("Agro Check"), SerializeField]
    private float agroRadius = 7f;

    public Transform player;

    protected AIBrain currentAction;

    [SerializeField]
    private float rotationSpeed = 5f;


    protected virtual void Awake()
    {
       // player = GameManager.Instance.playerPos;
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



    private void SetBrain(AIBrain newState)
    {
        currentAction = newState;
        StopAllCoroutines();

        switch (currentAction)
        {
            case AIBrain.Idle:
                Idle();
                break;
            case AIBrain.Patrol:
                StartCoroutine(OnPatrol());
                break;
            case AIBrain.Chase:
                StartCoroutine(OnChasing());
                break;
            case AIBrain.Combat:

            default:
                break;
        }
    }

    protected virtual void Idle()
    {

    }
    protected virtual IEnumerator OnPatrol()
    {
        yield return null;
    }
    protected virtual IEnumerator OnChasing()
    {
        yield return null;  
    }
    protected virtual IEnumerator OnAttacking()
    {
        yield return null;
    }
    protected virtual IEnumerator OnGetHit()
    {
        yield return null;
    }
    protected virtual void OnDie()
    {

    }


    protected virtual void OnThinking()
    {
        //Random Brain
        // SetBrain((AIBrain)UnityEngine.Random.Range(1, Enum.GetValues(typeof(AIBrain)).Length));
    }

    protected virtual void Update()
    {
        bool playerInView = IsPlayerInView();

        if (playerInView)
            isInCombat = true;
        else if (isInCombat)
            isInCombat = !LoseAgro(player.position);

        if (isInCombat)
        {
            Vector3 target = player.position - transform.position;
            target.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(target);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    protected virtual void FixedUpdate()
    {
       if(VisionConeCheck(player.position))
        {
            SetBrain(AIBrain.Chase);
        }
        else if (AreaCheck(player.position))
        {
            SetBrain(AIBrain.Chase);
        }
        else if (LoseAgro(player.position))
        {
            SetBrain(AIBrain.Idle);
        }
    }


    #endregion
    #region Check Functions
    void OnDrawGizmos()
    {
        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;
        if (VisionConeCheck(player.position))
            Handles.color = Color.red;
        if (AreaCheck(player.position))
            Handles.color = Color.blue;
        if (LoseAgro(player.position))
            Handles.color = Color.black;
        Gizmos.color = Handles.color;

        Handles.DrawWireDisc(Vector3.zero, Vector3.up, coneRadius);
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, areaRadius);
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, agroRadius);

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
        return flatDistance > agroRadius;
    }

    public bool IsPlayerInView()
    {
        return VisionConeCheck(player.position) || AreaCheck(player.position);
    }
    #endregion


}

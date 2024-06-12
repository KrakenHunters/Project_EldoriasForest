using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerDetector))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemySpotDetector))]

public class Enemy : CharacterClass
{
    NavMeshAgent agent;
    protected PlayerDetector playerDetector;
    EnemySpotDetector spotDetector;

    Animator animator;

    [SerializeField] float wanderRadius = 10f;
    [SerializeField] float wanderTime = 10f;
    [SerializeField] int maxGroupSize = 4;
    [SerializeField] float timeBetweenAttacks = 1f;

    public float runMultiplier;

    StateMachine stateMachine;

    CountdownTimer attackTimer;
    public CountdownTimer wanderTimer;

    float deathTimer = 3f;

    protected bool canAttack;

    [Header("Drops")]
    [SerializeField]
    private SoulCollectible soulDrop;
    [SerializeField]
    private HealthCollectible healthDrop;
    [SerializeField]
    protected float healthDropChance;

    public float rotationSpeed;

    [HideInInspector]
    public AISpot AISpotSelected;

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

    private List<AISpot> spotList;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerDetector = GetComponent<PlayerDetector>();
        spotDetector = GetComponent<EnemySpotDetector>();


        initialSpeed = _speed;

        SetHealth();

        attackTimer = new CountdownTimer(timeBetweenAttacks);
        wanderTimer = new CountdownTimer(wanderTime);

        stateMachine = new StateMachine();

        var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
        var patrollingState = new EnemyPatrollingState(this, animator, agent);
        var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
        var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);
        var dieState = new EnemyDieState(this, animator, agent);

        At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(wanderState, patrollingState, new FuncPredicate(() => ShouldPatrol()));
        At(patrollingState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(patrollingState, wanderState, new FuncPredicate(() => ArrivedAtLocation()));
        At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
        At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
        Any(dieState, new FuncPredicate(() => health <= 0));

        stateMachine.SetState(wanderState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    void Update()
    {
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime);
        wanderTimer.Tick(Time.deltaTime);

    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
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

    public virtual void DropSouls()
    {
        int nSoulDrops = 0;
        switch (tier)
        {
            case 1:
                nSoulDrops = Random.Range(soulAmountMinTier1, soulAmountMaxTier1);
                break;
            case 2:
                nSoulDrops = Random.Range(soulAmountMinTier2, soulAmountMaxTier2);
                break;
            case 3:
                nSoulDrops = Random.Range(soulAmountMinTier3, soulAmountMaxTier3);
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

    public virtual void DropHealth()
    {
        if (Random.Range(0f, 1f) <= HealthDropChance())
        {
            HealthCollectible health = Instantiate(healthDrop, transform.position + Vector3.forward, Quaternion.identity);
            health.tier = tier;
        }
    }

    float HealthDropChance()
    {
        float healthRatio = health / maxHealth;
        float dropChance = healthDropChance;

        if (healthRatio <= 0.2f)
        {
            dropChance *= 2f;
        }

        return dropChance;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject, deathTimer);
    }

    bool ShouldPatrol()
    {
        return (!wanderTimer.IsRunning || CrowdedRegion());
    }

    bool CrowdedRegion()
    {
        // Get all colliders within the radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, wanderRadius);

        int enemyCount = 0;

        // Iterate through the colliders and count the ones tagged as "Enemy"
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                enemyCount++;
            }
        }

        return enemyCount >= maxGroupSize;
    }

    public AISpot SelectAISpot()
    {
        if ( GridManager.Instance.gridDone && spotList == null)
        {
            spotList = new List<AISpot>();
            spotList = GridManager.Instance.enemySpots[tier];
        }

        AISpotSelected = spotList[Random.Range(0, spotList.Count)];
        return AISpotSelected;
    }

    bool ArrivedAtLocation()
    {
        return (Vector3.Distance(transform.position, AISpotSelected.transform.position) <= Random.Range(1f, 6f));
    }

    public virtual void Attack()
    {

        if (attackTimer.IsRunning)
        {
            canAttack = false;
        }
        else
        {
            canAttack = true;
            attackTimer.Start();
        }

    }

}


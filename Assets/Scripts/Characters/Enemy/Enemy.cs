using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utilities;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerDetector))]
[RequireComponent(typeof(Animator))]

public class Enemy : CharacterClass
{
    protected NavMeshAgent agent;
    [HideInInspector]
    public PlayerDetector playerDetector;
    [HideInInspector]
    public DissolvingController dissolvingController;
    [HideInInspector]
    public Collider enemyCollider;

    protected Animator animator;

    [SerializeField] protected float wanderRadius = 10f;
    [SerializeField] protected float wanderTime = 10f;
    [SerializeField] protected float timeBetweenAttacks = 1f;

    public float runMultiplier;

    protected StateMachine stateMachine;
    
    [HideInInspector]
    public CountdownTimer attackTimer;
    [HideInInspector]
    public CountdownTimer wanderTimer;

    float deathTimer = 3f;

    [SerializeField] float playerDetectionDistance = 30f;

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

    public EnemyAudioEvent enemyEvent;

    [HideInInspector]
    public bool gotHit;
    [HideInInspector]
    public bool attacking;

    protected bool startActions;

    [SerializeField]
    private Sprite imageTier1;
    [SerializeField]
    private Sprite imageTier2;
    [SerializeField]
    private Sprite imageTier3;

    [SerializeField]
    private GameObject TierImageUI;

    [SerializeField] protected WSHealthBar healthBar;
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerDetector = GetComponent<PlayerDetector>();
        dissolvingController = GetComponent<DissolvingController>();
        enemyCollider = GetComponent<Collider>();

        soulAmountMaxTier1 = Mathf.RoundToInt(soulAmountMaxTier1 * (1 + GameManager.Instance.pData.templeSoulsDropRate));
        soulAmountMaxTier2 = Mathf.RoundToInt(soulAmountMaxTier2 * (1 + GameManager.Instance.pData.templeSoulsDropRate));
        soulAmountMaxTier3 = Mathf.RoundToInt(soulAmountMaxTier3 * (1 + GameManager.Instance.pData.templeSoulsDropRate));
        soulAmountMinTier1 = Mathf.RoundToInt(soulAmountMinTier1 * (1 + GameManager.Instance.pData.templeSoulsDropRate));
        soulAmountMinTier2 = Mathf.RoundToInt(soulAmountMinTier2 * (1 + GameManager.Instance.pData.templeSoulsDropRate));
        soulAmountMinTier3 = Mathf.RoundToInt(soulAmountMinTier3 * (1 + GameManager.Instance.pData.templeSoulsDropRate));

        initialSpeed = _speed;
        initialDamageMultiplier = damageMultiplier;

        SetHealth();
        healthBar.SetMaxHealth(maxHealth);

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
        At(attackState, chaseState, new FuncPredicate(() => /*!playerDetector.CanAttackPlayer() &&*/ !attacking));
        Any(dieState, new FuncPredicate(() => !isAlive));
        Any(chaseState, new FuncPredicate(() => !attacking && isAlive && gotHit && !playerDetector.CanAttackPlayer()));

        stateMachine.SetState(wanderState);

    }

    protected void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    protected void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    protected virtual void Update()
    {
        
        if (Vector3.Distance(playerDetector.Player.position, transform.position) <= playerDetectionDistance && CanStartAction())
        {
            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);
            wanderTimer.Tick(Time.deltaTime);
        }

    }

    private bool CanStartAction()
    {
        if (GameManager.Instance.enemyActions)
            return true;
        else
            return false;
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public virtual void SetHealth()
    {
        switch (tier)
        {
            case 1:
                health = baseHealth;

                if (TierImageUI != null)
                {
                    TierImageUI.GetComponent<UnityEngine.UI.Image>().sprite = imageTier1;
                }

                break;
            case 2:
                health = baseHealth * healthMultTier2;
                if (TierImageUI != null)
                {
                    TierImageUI.GetComponent<UnityEngine.UI.Image>().sprite = imageTier2;
                }

                break;
            case 3:
                health = baseHealth * healthMultTier3;
                if (TierImageUI != null)
                {
                    TierImageUI.GetComponent<UnityEngine.UI.Image>().sprite = imageTier3;
                }

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
        float healthRatio = playerDetector.Player.GetComponent<PlayerController>().Health / playerDetector.Player.GetComponent<PlayerController>().MaxHealth;
        float dropChance = healthDropChance;

        if (healthRatio <= 0.2f)
        {
            dropChance *= 3f;
        }

        return dropChance;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject, deathTimer);
    }

    protected bool ShouldPatrol()
    {
        return (!wanderTimer.IsRunning && SelectAISpot() != null);
    }


    public override void GetHit(float damageAmount, GameObject attacker, SpellBook spell)
    {
        base.GetHit(damageAmount, attacker, spell);
        healthBar.SetHealth(health);
        if (!gotHit)
            gotHit = true;
        if (health <= 0)
            isAlive = false;
    }

    public AISpot SelectAISpot()
    {
        if ( GridManager.Instance.gridDone && spotList == null)
        {
            spotList = new List<AISpot>();
            spotList = GridManager.Instance.enemySpots[tier];
        }

        if (spotList.Count > 0)
        {
            AISpotSelected = spotList[Random.Range(0, spotList.Count)];
            return AISpotSelected;

        }
        else
            return null;
    }

    protected bool ArrivedAtLocation()
    {
        return (Vector3.Distance(transform.position, AISpotSelected.transform.position) <= Random.Range(1f, 6f));
    }

    public virtual void Attack()
    {
        attackTimer.Start();
    }

}


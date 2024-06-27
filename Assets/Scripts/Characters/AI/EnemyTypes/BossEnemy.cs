using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Utilities;

public class BossEnemy : Enemy
{
    private int phase = 1;

    [SerializeField]
    private float phase1Health;
    [SerializeField]
    private float phase2Health;
    [SerializeField]
    private float phase3Health;

    Dictionary<int, List<SpellBook>> phaseElement;

    [SerializeField]
    List<Enemy> spawnEnemies;

    [SerializeField]
    List<SpellBook> fireCastOrder;

    [SerializeField]
    List<SpellBook> lightningCastOrder;

    [SerializeField]
    List<SpellBook> iceCastOrder;

    private int spellOrderCount = 0;
    [HideInInspector]
    public SpellBook currentSpell;

    private float defaultAttackRange;

    [SerializeField]
    int numEnemiesSpawnPhase1;
    [SerializeField]
    int numEnemiesSpawnPhase2;
    [SerializeField]
    int numEnemiesSpawnPhase3;

    int numEnemies;

    private float duration;
    private bool isAggro = false;

    [HideInInspector]
    public bool switchPhase = false;
    public float switchPhaseTime;

    [HideInInspector]
    public SpellWeapon spellWeapon;

    public BoolGameEvent OnAggroWitch;

    [SerializeField]
    public bool invulnerable;

    [SerializeField]
    private GameEvent<Empty> OnWitchDead;

    protected override void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerDetector = GetComponent<PlayerDetector>();
        dissolvingController = GetComponent<DissolvingController>();
        enemyCollider = GetComponent<Collider>();

        tier = 3;
        spellWeapon = GetComponent<SpellWeapon>();
        defaultAttackRange = playerDetector.attackRange;
        DetermineElementsOrder();
        SelectSpell();

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
        var switchPhaseState = new BossSwitchPhaseState(this, animator, agent);
        var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);
        var dieState = new BossDieState(this, animator, agent);

        At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(wanderState, patrollingState, new FuncPredicate(() => ShouldPatrol()));
        At(patrollingState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(patrollingState, wanderState, new FuncPredicate(() => ArrivedAtLocation()));
        At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
        At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer() && !attacking));
        At(switchPhaseState, chaseState, new FuncPredicate(() => !switchPhase));
        Any(dieState, new FuncPredicate(() => !isAlive));
        Any(chaseState, new FuncPredicate(() => isAlive && gotHit && !playerDetector.CanAttackPlayer()));
        Any(switchPhaseState, new FuncPredicate(() => isAlive && switchPhase));


        stateMachine.SetState(wanderState);

    }

    protected override void Update()
    {
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime);
        wanderTimer.Tick(Time.deltaTime);
        if (playerDetector.CanDetectPlayer() || gotHit)
        {
            if (!isAggro)
            {
                isAggro = true;
                OnAggroWitch.Raise(isAggro);
            }
        }
        else
        {
            if (isAggro)
            {
                isAggro = false;
                OnAggroWitch.Raise(isAggro);
            }
            phase = 1;
            SetHealth();

        }

    }


    private void DetermineElementsOrder()
    {
        phaseElement = new Dictionary<int, List<SpellBook>>();
        // Create a list of spell lists
        List<List<SpellBook>> spellLists = new List<List<SpellBook>>()
        {
            fireCastOrder,
            lightningCastOrder,
            iceCastOrder
        };

        // Shuffle the spell lists
        ShuffleList(spellLists);

        // Assign the shuffled lists to the dictionary
        for (int i = 0; i < spellLists.Count; i++)
        {
            phaseElement.Add(i + 1, spellLists[i]);
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void SelectSpell()
    {
        // Check if spellOrderCount is within the range of the spellList
        if (spellOrderCount >= phaseElement[phase].Count || (currentSpell is UltimateSpellBook && health > maxHealth * 0.30f))
        {
            SpawnEnemies();
            // Reset spellOrderCount if it exceeds or matches the list count
            spellOrderCount = 0;
        }
        currentSpell = phaseElement[phase][spellOrderCount];
        spellOrderCount++;
        // Update the attack range based on the current spell's range
        if (currentSpell.spellData.tier3.range > 1f)
            playerDetector.attackRange = currentSpell.spellData.tier3.range;
        else
            playerDetector.attackRange = defaultAttackRange;


    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            // Calculate random angle in radians
            float angle = Random.Range(0f, Mathf.PI * 2f);

            // Calculate spawn position based on angle and radius
            Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 4f;

            // Spawn enemy at calculated position
            Instantiate(spawnEnemies[Random.Range(0, spawnEnemies.Count)], spawnPosition, Quaternion.identity);
        }
    }

    protected override void TakeDamage(float damage)
    {
        if (health <= 0 && phase == 3)
        {
            isAlive = false;
            OnWitchDead.Raise(new Empty());
        }
        else if (health <= 0 && phase != 3)
        {
            switchPhase = true;
            phase++;
        }
        else if (isAlive)
        {
            if (!invulnerable)
            {
                health -= damage;
            }
        }

    }


    public override void SetHealth()
    {
        switch (phase)
        {
            case 1:
                health = phase1Health;
                numEnemies = numEnemiesSpawnPhase1;

                break;
            case 2:
                health = phase2Health;
                numEnemies = numEnemiesSpawnPhase2;
                break;
            case 3:
                health = phase3Health;
                numEnemies = numEnemiesSpawnPhase3;
                break;
            default:
                break;
        }

        maxHealth = health;
    }

    public override void Attack()
    {
        base.Attack();
        CastSpell(currentSpell, out duration);
        StartCoroutine(WaitForTime(duration));
        SelectSpell();
    }
    // Coroutine to wait for the spell duration
    private IEnumerator WaitForTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        attacking = false;
    }

    private enum ElementPhase
    {
        Fire,
        Ice,
        Lightning
    }

}

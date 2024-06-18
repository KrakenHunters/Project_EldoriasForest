using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

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

    private SpellBook currentSpell;

    private float defaultAttackRange;

    [SerializeField]
    int numEnemiesSpawnPhase1;
    [SerializeField]
    int numEnemiesSpawnPhase2;
    [SerializeField]
    int numEnemiesSpawnPhase3;

    int numEnemies;

    protected override void Start()
    {
        base.Start();
        defaultAttackRange = playerDetector.attackRange;
        DetermineElementsOrder();
        SelectSpell();
    }

    protected override void Update()
    {
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime);
        wanderTimer.Tick(Time.deltaTime);
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
        if (spellOrderCount >= phaseElement[phase].Count)
        {
            // Reset spellOrderCount if it exceeds or matches the list count
            spellOrderCount = 0;
        }
        currentSpell = phaseElement[phase][spellOrderCount];
        spellOrderCount++;
        // Update the attack range based on the current spell's range
        if (currentSpell.spellData.tier3.range > 0f)
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
        }
        else if (health <= 0 && phase != 3)
        {
            phase++;
            SetHealth();
        }
        else if (isAlive)
        {
            health -= damage;
        }

    }


    protected override void SetHealth()
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

    private enum ElementPhase
    {
        Fire,
        Ice,
        Lightning
    }

}

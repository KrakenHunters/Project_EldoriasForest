using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossEnemy : Enemy
{
    private int phase = 1;

    private float phase1Health;
    private float phase2Health;
    private float phase3Health;

    [SerializeField]
    private List<SpellBook> fireSpells = new List<SpellBook>();
    [SerializeField]
    private List<SpellBook> iceSpells = new List<SpellBook>();
    [SerializeField]
    private List<SpellBook> lightningSpells = new List<SpellBook>();

    Dictionary<int, List<SpellBook>> phaseElement;

    [SerializeField]
    List<Enemy> spawnEnemies;

    [SerializeField]
    List<SpellBook> fireCastOrder;

    [SerializeField]
    List<SpellBook> lightningCastOrder;

    [SerializeField]
    List<SpellBook> iceCastOrder;

    protected override void Start()
    {
        base.Start();
        DetermineElementsOrder();
    }

    private void DetermineElementsOrder()
    {
        phaseElement = new Dictionary<int, List<SpellBook>>();
        // Create a list of spell lists
        List<List<SpellBook>> spellLists = new List<List<SpellBook>>()
        {
            fireSpells,
            iceSpells,
            lightningSpells
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
        switch (phase)
        {
            case 1:
                health = phase1Health;
                break;
            case 2:
                health = phase2Health;
                break;
            case 3:
                health = phase3Health;
                break;
            default:
                break;
        }

        maxHealth = health;
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
                break;
            case 2:
                health = phase2Health;
                break;
            case 3:
                health = phase3Health;
                break;
            default:
                break;
        }

        maxHealth = health;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : AIController
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

    protected override void Awake()
    {
        base.Awake();
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

    }
    protected override IEnumerator OnCombat()
    {
        while (AIBrain.Combat == currentAction)
        {

        }
        yield return null;
    }

    protected override IEnumerator OnChasing()
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

    protected override void OnDie()
    {
        if (phase < 3)
        {
            phase++;
            SetHealth();
        }
        else
        {
            StopAllCoroutines();
            agent.speed = 0f;
            agent.ResetPath();

            //Win the game!

            Destroy(this.gameObject, 1f);

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

    protected override void FindAISpots()
    {
        if (GridManager.Instance.gridDone)
        {
            spotList = new List<AISpot>();
            spotList = GridManager.Instance.enemySpots[3];
            foundSpots = true;
        }
        else
        {
            SetBrain(AIBrain.Idle);
        }
    }


}

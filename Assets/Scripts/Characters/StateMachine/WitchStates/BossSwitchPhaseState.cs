using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class BossSwitchPhaseState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    readonly BossEnemy bossEnemy;


    private float timer;
    private int counter;

    public BossSwitchPhaseState(BossEnemy bossEnemy, Animator animator, NavMeshAgent agent) : base(bossEnemy, animator)
    {
        this.agent = agent;
        this.bossEnemy = bossEnemy;
    }

    public override void OnEnter()
    {
        bossEnemy.invulnerable = true;
        counter = 0;
        timer = 0f;
        agent.ResetPath();

        animator.CrossFade(switchPhaseHash, crossFadeDuration);

    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > bossEnemy.switchPhaseTime)
        {
            counter++;
            enemy.DropSouls();
            enemy.DropHealth();
            timer = 0f;
        }

        if (counter >= 3)
        {
            bossEnemy.switchPhase = false;
            bossEnemy.invulnerable = false;
            bossEnemy.SetHealth();

        }
    }

}

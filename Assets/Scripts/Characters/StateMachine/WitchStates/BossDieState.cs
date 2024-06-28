using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossDieState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    readonly BossEnemy bossEnemy;


    private float timer;
    private int counter;

    public BossDieState(BossEnemy bossEnemy, Animator animator, NavMeshAgent agent) : base(bossEnemy, animator)
    {
        this.agent = agent;
        this.bossEnemy = bossEnemy;
    }

    public override void OnEnter()
    {
        Debug.Log("DieState");
        agent.ResetPath();

        enemy.enemyCollider.enabled = false;
        animator.CrossFade(DieHash, crossFadeDuration);
        // enemy.dissolvingController.OnDead();

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
            enemy.DestroyGameObject();

        }
    }

}

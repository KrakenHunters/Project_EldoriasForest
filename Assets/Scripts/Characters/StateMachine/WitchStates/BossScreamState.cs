using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossScreamState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    readonly BossEnemy bossEnemy;

    private float timer;

    public BossScreamState(BossEnemy bossEnemy, Animator animator, NavMeshAgent agent) : base(bossEnemy, animator)
    {
        this.agent = agent;
        this.bossEnemy = bossEnemy;
    }

    public override void OnEnter()
    {
        agent.ResetPath();
        enemy.enemyEvent.OnWitchScream.Invoke(bossEnemy);
        animator.CrossFade(ScreamHash, crossFadeDuration);
        // enemy.dissolvingController.OnDead();

    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3f)
        {
            bossEnemy.scream = false;
        }
    }

}

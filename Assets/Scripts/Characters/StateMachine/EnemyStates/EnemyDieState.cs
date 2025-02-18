using UnityEngine;
using UnityEngine.AI;

public class EnemyDieState : EnemyBaseState
{
    readonly NavMeshAgent agent;

    public EnemyDieState(Enemy enemy, Animator animator, NavMeshAgent agent) : base(enemy, animator)
    {
        this.agent = agent;
    }

    public override void OnEnter()
    {
        enemy.enemyCollider.enabled = false;
        animator.CrossFade(IdleHash, crossFadeDuration);
        enemy.dissolvingController.OnDead();
        agent.ResetPath();

        enemy.DropSouls();
        enemy.DropHealth();

        enemy.DestroyGameObject();
        
    }

}

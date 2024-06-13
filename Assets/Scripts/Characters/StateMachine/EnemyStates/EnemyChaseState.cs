using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    readonly Transform player;

    public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
    {
        this.agent = agent;
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log("Chase");
        enemy.gotHit = false;
        //animator.CrossFade(RunHash, crossFadeDuration);
        agent.speed = enemy.Speed * enemy.runMultiplier;
    }



    public override void Update()
    {
        agent.SetDestination(player.position);

    }
}

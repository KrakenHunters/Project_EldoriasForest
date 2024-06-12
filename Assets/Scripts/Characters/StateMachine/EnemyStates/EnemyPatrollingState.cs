using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrollingState : EnemyBaseState
{
    readonly NavMeshAgent agent;


    public EnemyPatrollingState(Enemy enemy, Animator animator, NavMeshAgent agent) : base(enemy, animator)
    {
        this.agent = agent;
    }

    public override void OnEnter()
    {
        //animator.CrossFade(WalkHash, crossFadeDuration);
        agent.speed = enemy.Speed;
        agent.SetDestination(enemy.SelectAISpot().transform.position);

    }

    public override void OnExit()
    {

    }


    public override void Update()
    {

    }

}

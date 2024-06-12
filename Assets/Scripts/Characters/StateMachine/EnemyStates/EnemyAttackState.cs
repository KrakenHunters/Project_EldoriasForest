using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    readonly Transform player;
    readonly float fieldOfViewAngle = 5f;

    public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
    {
        this.agent = agent;
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log("AttackState");
        //animator.CrossFade(AttackHash, crossFadeDuration);
        agent.ResetPath();
    }

    public override void Update()
    {
        Vector3 directionToPlayer = (player.position - enemy.transform.position).normalized;
        float angle = Vector3.Angle(enemy.transform.forward, directionToPlayer);

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, enemy.rotationSpeed * Time.deltaTime);


        if (angle < fieldOfViewAngle)
        {
            enemy.Attack();
        }
    }
}

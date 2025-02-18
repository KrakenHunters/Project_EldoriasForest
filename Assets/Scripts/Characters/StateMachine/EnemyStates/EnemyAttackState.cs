using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    readonly NavMeshAgent agent;
    readonly Transform player;
    readonly float fieldOfViewAngle = 10f;
    private bool hasAttacked;

    public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
    {
        this.agent = agent;
        this.player = player;
    }

    public override void OnEnter()
    {
        animator.CrossFade(IdleHash, crossFadeDuration);

        hasAttacked = false;
        enemy.attacking = true;
        agent.ResetPath();
    }

    public override void Update()
    {
        Vector3 directionToPlayer = (player.position - enemy.transform.position).normalized;
        float angle = Vector3.Angle(enemy.transform.forward, directionToPlayer);

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, enemy.rotationSpeed * Time.deltaTime);

        if (angle / 2 < fieldOfViewAngle && !enemy.attackTimer.IsRunning && !hasAttacked)
        {
            hasAttacked = true; // Set the flag to true to prevent multiple calls
            if (enemy is BossEnemy bossEnemy)
            {
                bossEnemy.spellTarget = bossEnemy.playerDetector.Player.position;
                bossEnemy.spellWeapon.InstantiateIndicator(bossEnemy.currentSpell, enemy);
                if (bossEnemy.currentSpell is SpecialSpellBook)
                {
                    animator.CrossFade(BossSpecialAttackHash, crossFadeDuration);
                }
                else if(bossEnemy.currentSpell is BaseSpellBook)
                {
                    animator.CrossFade(BossBaseAttackHash, crossFadeDuration);
                }
                else
                {
                    animator.CrossFade(BossUltimateAttackHash, crossFadeDuration);
                }
            }
            else
            {
                animator.CrossFade(AttackHash, crossFadeDuration);
            }
        }


        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash == BossSpecialAttackHash || stateInfo.shortNameHash == BossBaseAttackHash || stateInfo.shortNameHash == BossUltimateAttackHash || stateInfo.shortNameHash == AttackHash) // Ensure this matches the animation state name
        {
            if (stateInfo.normalizedTime >= 0.6f && hasAttacked && !enemy.attackTimer.IsRunning && enemy.attacking)
            {
                hasAttacked = false;
                enemy.Attack();
                animator.CrossFade(IdleHash, crossFadeDuration);
                enemy.attacking = false;
            }
        }
    }
}

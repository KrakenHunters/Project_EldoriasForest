using UnityEngine;

public abstract class EnemyBaseState : IState
{
    protected readonly Enemy enemy;
    protected readonly Animator animator;

    protected static readonly int IdleHash = Animator.StringToHash("Idle");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int WalkHash = Animator.StringToHash("Walk");
    protected static readonly int AttackHash = Animator.StringToHash("Attack");

    protected static readonly int BossBaseAttackHash = Animator.StringToHash("BaseSpell");
    protected static readonly int BossSpecialAttackHash = Animator.StringToHash("SpecialSpell");
    protected static readonly int BossUltimateAttackHash = Animator.StringToHash("UltimateSpell");

    protected static readonly int ScreamHash = Animator.StringToHash("Scream");
    protected static readonly int DieHash = Animator.StringToHash("Die");

    protected const float crossFadeDuration = 0.2f;

    protected EnemyBaseState(Enemy enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void OnExit()
    {

    }
}

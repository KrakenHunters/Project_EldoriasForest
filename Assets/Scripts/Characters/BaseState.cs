using UnityEngine;

public abstract class BaseState
{
    protected PlayerController player;

    protected float nextDirection;

    protected Vector3 _direction;
    protected LayerMask groundLayer = LayerMask.GetMask("Ground");
    protected float timer;

    protected float initialSpeed;
    protected float currentSpeed;
    protected float lerpDuration = 0.3f; // The duration over which to interpolate speed
    protected float lerpTimer;

    protected static readonly int MovementHash = Animator.StringToHash("Movement");
    protected static readonly int AttackHash = Animator.StringToHash("Attack");
    protected static readonly int SpecialAttackHash = Animator.StringToHash("SpecialAttack");

    protected static readonly int InteractHash = Animator.StringToHash("Interact");
    protected static readonly int DieHash = Animator.StringToHash("Death");


    public CharacterClass character { get; set; }
    public InputManager inputManager { get; set; }

    protected BaseState currentState;

    public virtual void EnterState() 
    {
        player = character.GetComponent<PlayerController>();
        lerpTimer = 0f;
        initialSpeed = player.c.velocity.magnitude; // Start from 0 speed

    }
    public virtual void ExitState() { }
    public virtual void StateFixedUpdate() 
    {
        lerpTimer += Time.fixedDeltaTime;

    }
    public virtual void StateUpdate() { }
    public virtual void HandleMovement(Vector2 dir) { }
    public virtual void HandleAttack() 
    {
        player.ChangeState(new PlayerAttackState());
    }

    public virtual void HandleAttackCancel()
    {
        player.ChangeState(new PlayerMoveInCombatState());
    }

    public virtual void HandleInteract() 
    {
        player.ChangeState(new PlayerInteractState());

    }

    public virtual void HandleSpecialAttack()
    {
        player.ChangeState(new PlayerSpecialAttack());

    }
    public virtual void StopInteract() { }

    public virtual void HandleDeath() { }
}
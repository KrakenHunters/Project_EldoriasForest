using UnityEngine;

public abstract class BaseState
{
    protected PlayerController player;

    protected float nextDirection;

    protected Vector3 _direction;
    protected LayerMask groundLayer = LayerMask.GetMask("Ground");
    protected float timer;

    public CharacterClass character { get; set; }
    public InputManager inputManager { get; set; }

    protected BaseState currentState;

    public virtual void EnterState() 
    {
        player = character.GetComponent<PlayerController>();
    }
    public virtual void ExitState() { }
    public virtual void StateFixedUpdate() { }
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
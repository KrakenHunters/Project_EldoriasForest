using UnityEngine;

public abstract class BaseState
{

    protected float nextDirection;
    protected Vector3 _direction;
    protected LayerMask groundLayer = LayerMask.GetMask("Ground");
    public CharacterClass character { get; set; }
    public InputManager inputManager { get; set; }

    protected BaseState currentState;

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateUpdate() { }
    public virtual void HandleMovement(Vector2 dir) { }
    public virtual void HandleMousePosition(Vector2 mousePos){ }
    public virtual void HandleAttack() { }

    public virtual void HandleAttackCancel()
    {
    }

    public virtual void HandleInteract() { }
    public virtual void StopInteract() { }

    public virtual void HandleDeath() { }
    

    protected virtual void RotateWithMove()
    {
        
    }
}
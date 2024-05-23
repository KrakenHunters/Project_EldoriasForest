using UnityEngine;

public abstract class BaseState
{

    protected float nextDirection;

    public CharacterClass character { get; set; }
    public InputManager inputManager { get; set; }

    protected BaseState currentState;

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateUpdate() { }
    public virtual void HandleMovement(Vector2 dir) { }
    public virtual void HandleAttack() { }
    public virtual void HandleInteract() { }
    public virtual void StopInteract() { }

    public virtual void HandleDeath() { }
}
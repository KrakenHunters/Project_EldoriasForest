using System.Collections;
using System.Collections.Generic;
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
    public virtual void HandleMovement() { }
    public virtual void HandleSpecialAttack() { }
    public virtual void HandleUltimateAttack() { }
    public virtual void HandleBaseAttack() { }
    public virtual void HandleInteract() { }
    public virtual void HandleDeath() { }
}
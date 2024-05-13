using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterClass
{
    private BaseState currentState;

  private void Update() => currentState?.StateUpdate();
  private void FixedUpdate() => currentState?.StateFixedUpdate();

    #region character Actions
    public void HandleMove()
    {
      currentState?.HandleMovement();
    }
    public void HandleInteract()
    {
       currentState?.HandleInteract();
    }
   public void HandleBaseAttack()
    {
        currentState?.HandleBaseAttack();
    }
    public void HandleSpecialAttack()
    {
        currentState?.HandleSpecialAttack();
    }  public void HandleUltimateAttack()
    {
        currentState?.HandleUltimateAttack();
    }
   
    #endregion
 
}

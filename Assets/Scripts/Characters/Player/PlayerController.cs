using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : CharacterClass
{
    [HideInInspector]
    public CharacterController c;

    private void Awake()
    {
        c = GetComponent<CharacterController>();
        ChangeState(new pIdleState());
    }

    private void Update() => currentState?.StateUpdate();
    private void FixedUpdate() => currentState?.StateFixedUpdate();

    #region character Actions
    public void HandleMove(Vector2 dir)
    {
        Debug.Log("Moving");
        currentState?.HandleMovement(dir);
    }
    public void HandleInteract()
    {
       currentState?.HandleInteract();
    }
   public void HandleBaseAttack()
    {
        Debug.Log("BaseAttack");
        currentState?.HandleBaseAttack();
    }
    public void HandleSpecialAttack()
    {
        currentState?.HandleSpecialAttack();
    }  
    public void HandleUltimateAttack()
    {
        currentState?.HandleUltimateAttack();
    }
   
    #endregion
 
}

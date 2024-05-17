using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : CharacterClass
{
    [HideInInspector]
    public CharacterController c;
    
    [SerializeField]
    private float combatCooldown = 5f;
    float combatTimer = 0;

    [SerializeField]
    protected float _rotationSpeed;
    public float RotationSpeed { get { return _speed; } }

    public AttackType attackType;

    public TemporaryDataContainer tempData;

    private InputManager inputManager;

    private bool isMoving;


    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        c = GetComponent<CharacterController>();
        ChangeState(new pIdleState());

    }

    private void Update()
    {
        if (isInCombat)
        {
           combatTimer += Time.deltaTime;
            if (combatTimer >= combatCooldown)
            {
                isInCombat = false;
                combatTimer = 0;
            }
        }

        if (inputManager.Movement != new Vector2(0f, 0f) || !isMoving)
        {
            HandleMove(inputManager.Movement);
            isMoving = true;
        }

        if (inputManager.Movement == new Vector2(0f, 0f) && isMoving)
        {
            isMoving = false;
        }


            currentState?.StateUpdate();
    }
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
        currentState?.HandleAttack();
        attackType = AttackType.Base;
    }
    public void HandleSpecialAttack()
    {
        currentState?.HandleAttack();
        attackType = AttackType.Special;
    }  
    public void HandleUltimateAttack()
    {
        currentState?.HandleAttack();
        attackType = AttackType.Ultimate;
    }

    #endregion


    public void ResetCombatTimer()
    {
        isInCombat = true;
        combatTimer = 0f;
    }

    public enum AttackType
    {
        Base,
        Special,
        Ultimate
    }

   

}

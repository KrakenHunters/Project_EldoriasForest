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

    private float baseAttackTimer;
    private float specialAttackTimer;
    private float ultimateAttackTimer;

    [HideInInspector]
    public bool canInteract;

    [HideInInspector]
    public Interactable interactableObj;
    private void Awake()
    {
        health = tempData.startHealth;
        inputManager = GetComponent<InputManager>();
        c = GetComponent<CharacterController>();
        ChangeState(new pIdleState());


        baseAttackTimer = 100f;
        specialAttackTimer = 100f;

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

        baseAttackTimer += Time.deltaTime;
        specialAttackTimer += Time.deltaTime;

        currentState?.StateUpdate();
    }
    private void FixedUpdate() => currentState?.StateFixedUpdate();



    #region character Actions
    public void HandleMove(Vector2 dir)
    {
        currentState?.HandleMovement(dir);
    }
    
    public void HandleInteract()
    {
        if (interactableObj != null)
            currentState?.HandleInteract();
    }

    public void CancelInteract()
    {
        currentState?.StopInteract();
    }

    public void HandleBaseAttack()
    {
        if (baseAttackTimer > tempData.baseSpell.cooldown)
        {
            currentState?.HandleAttack();
            attackType = AttackType.Base;
            baseAttackTimer = 0f;
        }
    }
    public void HandleSpecialAttack()
    {
        if (tempData.specialSpell != null)
        {
            if (specialAttackTimer > tempData.specialSpell.cooldown)
            {
                currentState?.HandleAttack();
                attackType = AttackType.Special;
                specialAttackTimer = 0f;
            }
        }
    }
    public void HandleUltimateAttack()
    {
        if (tempData.ultimateSpell != null)
        {
            currentState?.HandleAttack();
            attackType = AttackType.Ultimate;
            Invoke("RemoveUltimateSpell", 3f);
        }
    }

    #endregion

    public override void GetHit(int damageAmount,CharacterClass attacker, SpellBook spell)
    {
        base.GetHit(damageAmount, attacker, spell);
        if (health <= 0)
        {
            Debug.Log("Dead");
            //Time.timeScale = 0f;
        }

    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Interactable>())
        {
            if (other.GetComponent<Interactable>().canInteract)
                interactableObj = other.GetComponent<Interactable>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>())
        {
            interactableObj = null;
        }
    }

    private void RemoveUltimateSpell()
    {
        //Turn it on later
        //tempData.ultimateSpell = null;
    }




}

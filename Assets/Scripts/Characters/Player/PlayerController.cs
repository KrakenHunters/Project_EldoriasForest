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


    [HideInInspector]
    public bool canInteract;

    [HideInInspector]
    public Interactable interactableObj;
    private void Awake()
    {
        health = tempData.startHealth;
        PlayerGUIManager.Instance.SetHealthValues(health);
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
        if (tempData.baseSpell != null)
            PlayerSpellCastManager.Instance.CastBaseSpell();

    }
    public void HandleSpecialAttack()
    {
        if (tempData.specialSpell != null)
            PlayerSpellCastManager.Instance.CastSpecialSpell();
    }
    public void HandleUltimateAttack()
    {
        if (tempData.ultimateSpell != null)
            PlayerSpellCastManager.Instance.CastUltimateSpell();
    }

    #endregion

    public override void GetHit(int damageAmount,GameObject attacker, SpellBook spell)
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

    protected override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        PlayerGUIManager.Instance.SetHealthValues(health);
    }

    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
        PlayerGUIManager.Instance.SetHealthValues(health);
    }

}

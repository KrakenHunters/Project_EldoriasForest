using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : CharacterClass
{
    
    protected LayerMask groundLayer;
    [HideInInspector]
    public CharacterController c;
    
    [SerializeField]
    private float combatCooldown = 5f;
    float combatTimer = 0;

    [SerializeField]
    protected float _rotationSpeed;
    
    [field: SerializeField]
    public PlayerInteractionInformationSO PlayerInteraction { get; private set; }
    public AttackType attackType;
    
    public TemporaryDataContainer tempData;
    public Vector3 MouseWorldPosition { get; private set; }
    public Quaternion PlayerRotation { get; private set; }
    private InputManager inputManager;

    private bool isMoving;


    [HideInInspector]
    public bool canInteract;

    [HideInInspector]
    public Interactable interactableObj;
    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
        health = tempData.startHealth;
        maxHealth = tempData.startHealth;
        
        PlayerGUIManager.Instance.SetHealthValues(health);
        inputManager = GetComponent<InputManager>();
        c = GetComponent<CharacterController>();
        ChangeState(new PlayerIdleState());

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

    public void HandlePointerDirection(Vector2 mousePos)
    {
        Vector3 newMousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(newMousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            MouseWorldPosition = hit.point;
        }
        Vector3 target = MouseWorldPosition;
        Vector3 direction = target - transform.position;
        direction.y = 0;
        PlayerRotation = Quaternion.LookRotation(direction);
    }

    public void HandleControllerDirection(Vector2 move)
    {
        //calculate the PlayerRotation based on the movement of the joystick
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

    public override void GetHit(float damageAmount,GameObject attacker, SpellBook spell)
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
    public void RotateToTarget()
    {
        transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, PlayerRotation, RotationSpeed);
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

    public void HandleCancelBaseAttack()
    {
        currentState?.HandleAttackCancel();
        //throw new NotImplementedException();
    }
}

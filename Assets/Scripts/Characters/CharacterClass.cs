using System.Collections;
using UnityEngine;

public class CharacterClass : BaseObject
{
    [SerializeField]
    protected float health;
    public float Health { get { return health; } }

    protected float maxHealth;

    [SerializeField]
    protected float _speed;
    [HideInInspector]
    public float initialSpeed;
    public float Speed { get { return _speed; } set { _speed = value; } }
    [SerializeField]
    protected float _speedModifier;
    public float SpeedModifier { get { return _speedModifier; } }

    [HideInInspector]
    public bool isInCombat = false;

    [SerializeField]
    protected Transform castPos;


    [HideInInspector]
    public BaseState currentState;

    public float damageMultiplier = 1.0f;

    protected bool isAlive = true;



    public virtual void ChangeState(BaseState newState)
    {
        StartCoroutine(WaitFixedFrame(newState));
    }

    public void CastSpell(SpellBook spell, out float duration)
    {
        SpellBook spellBook;
        duration = 1f;
        switch (spell.castOrigin)
        {
            case SpellBook.castType.projectile:
                spellBook = Instantiate(spell, castPos.position, Quaternion.identity);
                spellBook.Shoot(transform.forward, this.gameObject);
                duration = spellBook.ReturnDuration();

                break;

            case SpellBook.castType.groundPos:
                Vector3 mousePos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;
                LayerMask groundLayer = LayerMask.GetMask("Ground");

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    Vector3 target = hit.point;

                    spellBook = Instantiate(spell, target, Quaternion.identity);
                    spellBook.Shoot(transform.forward, this.gameObject);
                    duration = spellBook.ReturnDuration();


                }
                break;
            case SpellBook.castType.skyToGroundPos:
                Vector3 mousePos2 = Input.mousePosition;
                Ray ray2 = Camera.main.ScreenPointToRay(mousePos2);
                RaycastHit hit2;
                LayerMask groundLayer2 = LayerMask.GetMask("Ground");

                if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, groundLayer2))
                {
                    Vector3 target = hit2.point;

                    spellBook = Instantiate(spell, new Vector3(transform.position.x, 30f, transform.position.z), Quaternion.identity);
                    spellBook.Shoot(target, this.gameObject);
                    duration = spellBook.ReturnDuration();


                }
                break;
            case SpellBook.castType.self:
                spellBook = Instantiate(spell, transform.position, Quaternion.identity, transform);
                spellBook.Shoot(transform.forward, this.gameObject);
                duration = spellBook.ReturnDuration();

                break;
        }

    }

    private IEnumerator WaitFixedFrame(BaseState newState)
    {

        yield return new WaitForFixedUpdate();
        currentState?.ExitState();
        currentState = newState;
        currentState.character = this;
        currentState.EnterState();

    }

    public virtual void GetHit(float damageAmount, GameObject attacker, SpellBook spellBook)
    {
      
        if (attacker != this.gameObject)
        {
            TakeDamage(damageAmount * damageMultiplier);
        }

    }

   protected virtual void TakeDamage(float damage)
    {
        if (health <= 0)
            isAlive = false;
        else if(isAlive)
            health -= damage;

    }

    public virtual void Heal(float healAmount)
    {
        if (isAlive)
        {
            if (healAmount + health > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += healAmount;
            }
        }
    }
}

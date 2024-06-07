using System.Collections;
using UnityEngine;
using static AIController;

public class CharacterClass : BaseObject
{
    [SerializeField]
    protected float health;
    public float Health { get { return health; } }

    protected float maxHealth;

    [SerializeField]
    protected float _speed;
    public float Speed { get { return _speed; } }
    [SerializeField]
    protected float _speedModifier;
    public float SpeedModifier { get { return _speedModifier; } }

    [HideInInspector]
    public bool isInCombat = false;

    [SerializeField]
    protected Transform castPos;


    [HideInInspector]
    public BaseState currentState;

    private float damageMultiplier = 1.0f;
    private float originalSpeed;

    protected bool isAlive = true;

    public enum StatusEffect
    {
        LightningEffect,
        FireEffect,
        IceEffect,
        None
    }

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

            if (spellBook != null)
            {
                if (Random.value <= spellBook.statusEffectChance)
                    ApplyStatusEffect(spellBook);
            }
        }

    }

   protected virtual void TakeDamage(float damage)
    {
        if (isAlive)
        {
            health -= damage;
        }
        if (health <= 0 && isAlive)
        {
            isAlive = false;
        }

    }
    private void ApplyStatusEffect(SpellBook spellBook)
    {

        //  SpellBook newSpellBook = new SpellBook();
        SpellBook newSpellBook;
        if (spellBook == null)
        {
            newSpellBook = new SpellBook();
            newSpellBook.statusEffect = SpellBook.StatusEffect.None;
        }
        else
        {
            newSpellBook = spellBook;
        }

        StopCoroutine(OnFrozen(newSpellBook.statusEffectTimer, newSpellBook.statusEffectDamage));
        StopCoroutine(OnBurning(newSpellBook.statusEffectTimer, newSpellBook.statusEffectDamage));
        StopCoroutine(OnStunned(newSpellBook.statusEffectTimer));

        switch (newSpellBook.statusEffect)
        {
            case SpellBook.StatusEffect.IceEffect:
                StartCoroutine(OnFrozen(newSpellBook.statusEffectTimer, newSpellBook.statusEffectDamage));
                break;
            case SpellBook.StatusEffect.FireEffect:
                StartCoroutine(OnBurning(newSpellBook.statusEffectTimer, newSpellBook.statusEffectDamage));
                break;
            case SpellBook.StatusEffect.LightningEffect:
                StartCoroutine(OnStunned(newSpellBook.statusEffectTimer));
                break;
            case SpellBook.StatusEffect.None:
                break;


        }
    }

    private IEnumerator OnFrozen(float effectTimer,float damageMult)
    {
        float timer = 0f;

        while (timer < effectTimer)
        {
            timer += 1f;

            damageMultiplier = damageMult;
            yield return new WaitForSeconds(1f);
        }

        damageMultiplier = 1f;


        yield return null;
    }

    private IEnumerator OnStunned(float effectTimer)
    {
        originalSpeed = _speed;
        float timer = 0f;
        while (timer < effectTimer)
        {
            timer += 1f;

            _speed = 0f;

            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Exit Stun");
        _speed = originalSpeed;

        yield return null;
    }


    private IEnumerator OnBurning(float effectTimer, float damage)
    {
        float timer = 0f;

        while (timer < effectTimer)
        {
            yield return new WaitForSeconds(1f);

            timer += 1f;
            Debug.Log("Burning");

            TakeDamage(damage);
        }

        yield return null;
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

using System.Collections;
using UnityEngine;

public class CharacterClass : BaseObject
{
    [SerializeField]
    protected float health;
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

    public void CastSpell(SpellBook spell)
    {
        switch (spell.castOrigin)
        {
            case SpellBook.castType.projectile:
                SpellBook spellBook = Instantiate(spell, castPos.position, Quaternion.identity);
                spellBook.Shoot(transform.forward, this.gameObject);
                break;

            case SpellBook.castType.groundPos:
                Vector3 mousePos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;
                LayerMask groundLayer = LayerMask.GetMask("Ground");

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    Vector3 target = hit.point;

                    SpellBook spellBook2 = Instantiate(spell, target, Quaternion.identity);
                    spellBook2.Shoot(transform.forward, this.gameObject);

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

                    SpellBook spellBook3 = Instantiate(spell, new Vector3(transform.position.x, 30f, transform.position.z), Quaternion.identity);
                    spellBook3.Shoot(target, this.gameObject);

                }
                break;
            case SpellBook.castType.self:
                SpellBook spellBook4 = Instantiate(spell, transform.position, Quaternion.identity, transform);
                spellBook4.Shoot(transform.forward, this.gameObject);
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

    public virtual void GetHit(int damageAmount, GameObject attacker, SpellBook spellBook)
    {
        if (attacker != this.gameObject)
        {
            health -= Mathf.RoundToInt(damageAmount * damageMultiplier);

            if (spellBook != null)
            {
                if (Random.value <= spellBook.statusEffectChance)
                    ApplyStatusEffect(spellBook);
            }
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
        if(_speed != 0f)
        originalSpeed = _speed;
        float timer = 0f;
        while (timer < effectTimer)
        {
            timer += 1f;

            _speed = 0f;

            yield return new WaitForSeconds(1f);
        }

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

            health -= damage;
        }

        yield return null;
    }

    public virtual void Heal(int healAmount)
    {
        health += healAmount;
    }
}

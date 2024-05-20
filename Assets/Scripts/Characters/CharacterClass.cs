using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class CharacterClass : MonoBehaviour
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

    public virtual void ChangeState(BaseState newState)
    {
        StartCoroutine(WaitFixedFrame(newState));

    }

    public void CastSpell(SpellBook spell)
    {
        Debug.Log("Players Forward" + transform.forward);
        SpellBook spellBook = Instantiate(spell, castPos.position, Quaternion.identity);
        spellBook.Shoot(transform.forward);
    }

    private IEnumerator WaitFixedFrame(BaseState newState)
    {

        yield return new WaitForFixedUpdate();
        currentState?.ExitState();
        currentState = newState;
        currentState.character = this;
        currentState.EnterState();

    }

    public virtual void GetHit(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0)
        {
            Debug.Log("Dead");
        }
    }

    public virtual void Heal(int healAmount)
    {
        health += healAmount;
    }
}

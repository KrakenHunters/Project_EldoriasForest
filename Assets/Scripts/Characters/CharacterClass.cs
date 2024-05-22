using System.Collections;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

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
        if (spell.castOrigin == SpellBook.castType.projectile)
        {
            SpellBook spellBook = Instantiate(spell, castPos.position, Quaternion.identity);
            spellBook.Shoot(transform.forward, this);
        }
        else if (spell.castOrigin == SpellBook.castType.groundPos)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            LayerMask groundLayer = LayerMask.GetMask("Ground");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 target = hit.point;

                SpellBook spellBook = Instantiate(spell, target, Quaternion.identity);
                spellBook.Shoot(transform.forward, this);

            }

        }
        else if (spell.castOrigin == SpellBook.castType.skyToGroundPos)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            LayerMask groundLayer = LayerMask.GetMask("Ground");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 target = hit.point;

                SpellBook spellBook = Instantiate(spell, new Vector3(transform.position.x, 30f, transform.position.z), Quaternion.identity);
                spellBook.Shoot(target, this);

            }

        }
        else if(spell.castOrigin == SpellBook.castType.self)
        {
            SpellBook spellBook = Instantiate(spell, transform.position, Quaternion.identity, transform);
            spellBook.Shoot(transform.forward, this);

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

    public virtual void GetHit(int damageAmount, CharacterClass attacker)
    {
        health -= damageAmount;
    }

    public virtual void Heal(int healAmount)
    {
        health += healAmount;
    }
}

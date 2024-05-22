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
        switch (spell.castOrigin)
        {
            case SpellBook.castType.projectile:
                SpellBook spellBook = Instantiate(spell, castPos.position, Quaternion.identity);
                spellBook.Shoot(transform.forward, this);
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
                    spellBook2.Shoot(transform.forward, this);

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
                    spellBook3.Shoot(target, this);

                }
                break;
            case SpellBook.castType.self:
                SpellBook spellBook4 = Instantiate(spell, transform.position, Quaternion.identity, transform);
                spellBook4.Shoot(transform.forward, this);
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

    public virtual void GetHit(int damageAmount, CharacterClass attacker)
    {
        health -= damageAmount;
    }

    public virtual void Heal(int healAmount)
    {
        health += healAmount;
    }
}

using System.Collections;
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


    [HideInInspector]
    public BaseState currentState;

    public void ChangeState(BaseState newState)
    {
        StartCoroutine(WaitFixedFrame(newState));
    }

    public void CastSpell(SpellBook spell)
    {
        Instantiate(spell, transform.position, transform.rotation);
    }

    private IEnumerator WaitFixedFrame(BaseState newState)
    {

        yield return new WaitForFixedUpdate();
        currentState?.ExitState();
        currentState = newState;
        currentState.character = this;
        currentState.EnterState();

    }
}

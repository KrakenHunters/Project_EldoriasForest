using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    protected int tier = 1;
    public float cooldown;
    protected CharacterClass charAttacker;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Spell");
        CastSpell(tier);
    }
    protected virtual void UpgradeTier()
    {
        if (tier < 3)
        {
            tier++;
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void CastSpell(int tier)
    {

    }

    public virtual void Shoot(Vector3 direction,CharacterClass attacker)
    {

      charAttacker = attacker;

    }
}

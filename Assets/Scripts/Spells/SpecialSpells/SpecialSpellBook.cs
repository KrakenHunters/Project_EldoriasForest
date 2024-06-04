using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSpellBook : SpellBook
{

    protected virtual void SpellCollected()
    {
        if (tier > spellData.tierUnlocked)
        {
            spellData.tierUnlocked = tier;
        }
    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction,attacker);
        if (attacker.GetComponent<PlayerController>())
        {
            attacker.GetComponent<PlayerSpellCastManager>().currentSpecialSpellCooldown = cooldown;
        }

    }
}

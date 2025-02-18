using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSpellBook : SpellBook
{

    public virtual void SpellCollected(int tierUnlock)
    {
        if (tierUnlock > spellData.tierUnlocked)
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
            attacker.GetComponent<PlayerSpellCastManager>().currentSpecialSpellDuration = duration;
            attacker.GetComponent<PlayerSpellCastManager>().StartCoroutine("SpecialSpellCooldownTimer");
        }
        else
        {
            tier = charAttacker.GetComponent<CharacterClass>().tier;
            SetDataFromSpellContainer();

        }

    }
}

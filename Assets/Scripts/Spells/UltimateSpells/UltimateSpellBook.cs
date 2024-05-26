using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSpellBook : SpellBook
{
    public override void Shoot(Vector3 direction, CharacterClass attacker)
    {
        base.Shoot(direction, attacker);
        if (attacker.GetComponent<PlayerController>())
        {
            PlayerSpellCastManager.Instance.currentUltimateSpellCooldown = cooldown;
        }

    }
}

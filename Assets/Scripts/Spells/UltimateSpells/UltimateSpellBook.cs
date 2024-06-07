using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSpellBook : SpellBook
{
    protected override void Awake()
    {
        tier = 3;
        base.Awake();
    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction, attacker);

    }
}

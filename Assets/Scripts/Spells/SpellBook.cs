using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    protected int tier = 1;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        CastSpell(tier);
    }
    protected virtual void UpgradeTier()
    {
        if (tier < 3)
        {
            tier += 1;
        }
    }

    protected virtual void CastSpell(int tier)
    {

    }
}

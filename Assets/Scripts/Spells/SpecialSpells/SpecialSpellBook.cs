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


}

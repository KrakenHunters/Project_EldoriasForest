using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSpellBook : SpellBook
{
    public SpecialSpellsTierContainer tierUnlocked;

    protected virtual void SpellCollected()
    {
        if (tier > tierUnlocked.tierUnlocked)
        {
            tierUnlocked.tierUnlocked = tier;
        }
    }


}

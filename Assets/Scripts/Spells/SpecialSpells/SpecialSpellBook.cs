using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSpellBook : SpellBook
{
    public SpecialSpellsTierContainer tierUnlocked;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void SpellCollected()
    {
        if (tier > tierUnlocked.tierUnlocked)
        {
            tierUnlocked.tierUnlocked = tier;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

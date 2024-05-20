using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu,Serializable]

public class PermanentDataContainer : ScriptableObject
{
    public int totalSouls;
    public List<SpellBook> spellBooksUnlocked;
    public int rune = 0;
    public float cooldownReduction = 0;
    public int baseAttackTier = 1;
    public BaseSpellBook prefBaseSpell;
    public float templeSoulsDropRate;

    public int healthBonus;



}




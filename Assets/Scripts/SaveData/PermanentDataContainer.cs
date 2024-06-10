using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data Containers/PermanentData"),Serializable]

public class PermanentDataContainer : ScriptableObject
{
    public int totalSouls;
    public List<SpellBook> spellBooksUnlocked;
    public int baseAttackTier = 1;
    public BaseSpellBook prefBaseSpell;

    public float templeSoulsDropRate;
    public int rune = 0;
    public float cooldownReduction = 0;
    public int healthBonus;
    public bool IsUltimateSpellSlotUnlocked;



}




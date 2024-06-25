using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data Containers/PermanentData"),Serializable]
public class PermanentDataContainer : ScriptableObject
{
    public int totalSouls;
    public List<SpecialSpellBook> spellBooksUnlocked = new List<SpecialSpellBook>();
    public List<int> spellBookIDs = new List<int>(); // Store unique IDs here
    public int baseAttackTier;
    public BaseSpellBook prefBaseSpell;
    public float templeSoulsDropRate;
    public int rune;
    public float cooldownReduction;
    public int healthBonus;
    public bool IsUltimateSpellSlotUnlocked;
    public bool InitializeData;

[Header("CostUpgrades")]
    public int baseUpgradeCost;
    public int healthUpgradeCost;
    public int defensiveRuneCost;
    public int soulDropUpgradeCost;
    public int spellCooldownCost;

}




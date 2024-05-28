using System;
using UnityEngine;
public class SpellStatsContainer : ScriptableObject
{
    public int tierUnlocked = 1;

    [HideInInspector]
    public TierData currentTierData;

    public void SetTierData(int currentTier)
    {
        switch (currentTier)
        {
            case 1:
                currentTierData = tier1;
                break;
            case 2:
                currentTierData = tier2;
                break;
            case 3:
                currentTierData = tier3;
                break;
            default:
                break;

        }
    }

    public virtual TierData tier1
    {
        get { return tier1; }
        set { tier1 = value; }
    }

    public virtual TierData tier2
    {
        get { return tier2; }
        set { tier2 = value; }
    }

    public virtual TierData tier3
    {
        get { return tier3; }
        set { tier3 = value; }
    }



}

[Serializable]
public class TierData
{
    public int damage;
    public float cooldown;
    public float statusEffectTimer;
    public float statusEffectDamage;
    public float statusEffectChance;

}

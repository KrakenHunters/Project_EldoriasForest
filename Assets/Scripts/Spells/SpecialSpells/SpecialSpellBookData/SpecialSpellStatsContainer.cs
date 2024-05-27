using System;
using UnityEngine;

public class SpecialSpellStatsContainer
{
    //hi look below
}

#region Fire Shotgun Spell Container

[CreateAssetMenu(fileName = "FireShotgunSpell", menuName = "Spells/FireShotgun")]
public class FireShotgunSpellStatsContainer : SpellStatsContainer
{
    public FireShotgunTierData SpellTier1;
    public FireShotgunTierData SpellTier2;
    public FireShotgunTierData SpellTier3;

    public override TierData tier1
    {
        get { return SpellTier1; }
        set { SpellTier1 = value as FireShotgunTierData; }
    }

    public override TierData tier2
    {
        get { return SpellTier2; }
        set { SpellTier2 = value as FireShotgunTierData; }
    }

    public override TierData tier3
    {
        get { return SpellTier3; }
        set { SpellTier3 = value as FireShotgunTierData; }
    }

}

[Serializable]
public class FireShotgunTierData : TierData
{
    public float range;
    public float angle;
}
#endregion  

#region Fire Ring Spell Container

[CreateAssetMenu(fileName = "FireRingSpell", menuName = "Spells/FireRing")]
public class FireRingSpellStatsContainer : SpellStatsContainer
{
    public FireRingTierData SpellTier1;
    public FireRingTierData SpellTier2;
    public FireRingTierData SpellTier3;

    public override TierData tier1
    {
        get { return SpellTier1; }
        set { SpellTier1 = value as FireRingTierData; }
    }

    public override TierData tier2
    {
        get { return SpellTier2; }
        set { SpellTier2 = value as FireRingTierData; }
    }

    public override TierData tier3
    {
        get { return SpellTier3; }
        set { SpellTier3 = value as FireRingTierData; }
    }

}

[Serializable]
public class FireRingTierData : TierData
{
    public int healAmount;
}
#endregion  

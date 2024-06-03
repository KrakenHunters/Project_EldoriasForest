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

#region Frosty Push Spell Container

[CreateAssetMenu(fileName = "FrostyPushSpell", menuName = "Spells/FrostyPush")]
public class FrostyPushSpellStatsContainer : SpellStatsContainer
{
    public FrostyPushTierData SpellTier1;
    public FrostyPushTierData SpellTier2;
    public FrostyPushTierData SpellTier3;

    public override TierData tier1
    {
        get { return SpellTier1; }
        set { SpellTier1 = value as FrostyPushTierData; }
    }

    public override TierData tier2
    {
        get { return SpellTier2; }
        set { SpellTier2 = value as FrostyPushTierData; }
    }

    public override TierData tier3
    {
        get { return SpellTier3; }
        set { SpellTier3 = value as FrostyPushTierData; }
    }

}

[Serializable]
public class FrostyPushTierData : TierData
{
    public int radius;
}
#endregion  

#region Tesla Machine Spell Container

[CreateAssetMenu(fileName = "TeslaMachineSpell", menuName = "Spells/TeslaMachine")]
public class TeslaMachineSpellStatsContainer : SpellStatsContainer
{
    public TeslaMachineTierData SpellTier1;
    public TeslaMachineTierData SpellTier2;
    public TeslaMachineTierData SpellTier3;

    public override TierData tier1
    {
        get { return SpellTier1; }
        set { SpellTier1 = value as TeslaMachineTierData; }
    }

    public override TierData tier2
    {
        get { return SpellTier2; }
        set { SpellTier2 = value as TeslaMachineTierData; }
    }

    public override TierData tier3
    {
        get { return SpellTier3; }
        set { SpellTier3 = value as TeslaMachineTierData; }
    }

}

[Serializable]
public class TeslaMachineTierData : TierData
{
    public float radius;
}
#endregion  

#region Lightning Beam Spell Container

[CreateAssetMenu(fileName = "LightningBeamSpell", menuName = "Spells/LightningBeam")]
public class LightningBeamSpellStatsContainer : SpellStatsContainer
{
    public LightningBeamTierData SpellTier1;
    public LightningBeamTierData SpellTier2;
    public LightningBeamTierData SpellTier3;

    public override TierData tier1
    {
        get { return SpellTier1; }
        set { SpellTier1 = value as LightningBeamTierData; }
    }

    public override TierData tier2
    {
        get { return SpellTier2; }
        set { SpellTier2 = value as LightningBeamTierData; }
    }

    public override TierData tier3
    {
        get { return SpellTier3; }
        set { SpellTier3 = value as LightningBeamTierData; }
    }

}

[Serializable]
public class LightningBeamTierData : TierData
{
    public float duration;
}
#endregion  

#region FireTrail Spell Container

[CreateAssetMenu(fileName = "FireTrailSpell", menuName = "Spells/FireTrail")]
public class FireTrailSpellStatsContainer : SpellStatsContainer
{
    public FireTrailTierData SpellTier1;
    public FireTrailTierData SpellTier2;
    public FireTrailTierData SpellTier3;

    public override TierData tier1
    {
        get { return SpellTier1; }
        set { SpellTier1 = value as FireTrailTierData; }
    }

    public override TierData tier2
    {
        get { return SpellTier2; }
        set { SpellTier2 = value as FireTrailTierData; }
    }

    public override TierData tier3
    {
        get { return SpellTier3; }
        set { SpellTier3 = value as FireTrailTierData; }
    }

}

[Serializable]
public class FireTrailTierData : TierData
{
    public float duration;
}
#endregion 





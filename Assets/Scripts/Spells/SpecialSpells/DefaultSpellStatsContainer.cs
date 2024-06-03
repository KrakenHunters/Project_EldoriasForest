using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSpell", menuName = "Spells/DefaultSpell")]
public class DefaultSpellStatsContainer : SpellStatsContainer
{
    public TierData SpellTier1;
    public TierData SpellTier2;
    public TierData SpellTier3;

    public override TierData tier1
    {
        get { return SpellTier1; }
        set { SpellTier1 = value; }
    }

    public override TierData tier2
    {
        get { return SpellTier2; }
        set { SpellTier2 = value; }
    }

    public override TierData tier3
    {
        get { return SpellTier3; }
        set { SpellTier3 = value; }
    }

}

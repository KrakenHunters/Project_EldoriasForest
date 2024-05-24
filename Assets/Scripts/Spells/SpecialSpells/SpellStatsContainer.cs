using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
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

    public TierData tier1;

    public TierData tier2;

    public TierData tier3;


}

[Serializable]
public struct TierData
{
    public int damage;
    public float cooldown;
    public float statusEffectTimer;
    public float statusEffectDamage;

}

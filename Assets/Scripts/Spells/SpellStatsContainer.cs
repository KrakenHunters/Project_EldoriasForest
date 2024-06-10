using System;
using UnityEngine;
[CreateAssetMenu(fileName = "SpellStatContainer")]
public class SpellStatsContainer : ScriptableObject
{
    [HideInInspector]
    public int tierUnlocked = 1;

    [HideInInspector]
    public TierData currentTierData;

    public string shortDescription;

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
public class TierData
{
    public float damage;
    public float cooldown;
    public float statusEffectTimer;
    public float statusEffectDamage;
    public float statusEffectChance;
    public float range;
    public float healAmount;
    public float radius;
    public float duration;
    public float speed; 
}

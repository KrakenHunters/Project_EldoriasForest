using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCollectible : Collectible
{
    [SerializeField]
    public int soulAmountMaxTier1;
    [SerializeField]
    public int soulAmountMinTier1;
    [SerializeField]
    public int soulAmountMaxTier2;
    [SerializeField]
    public int soulAmountMinTier2;
    [SerializeField]
    public int soulAmountMaxTier3;
    [SerializeField]
    public int soulAmountMinTier3;

    [SerializeField]
    private GameEvent OnSoulCollected;


    protected override void ItemCollected(PlayerController player)
    {
        base.ItemCollected(player);

        switch (tier)
        {
            case 0:
                player.tempData.collectedSouls += Random.Range(soulAmountMinTier1, soulAmountMaxTier1);
                break;
            case 1:
                player.tempData.collectedSouls += Random.Range(soulAmountMinTier1, soulAmountMaxTier1);
                break;
            case 2:
                player.tempData.collectedSouls += Random.Range(soulAmountMinTier2, soulAmountMaxTier2);
                break;
            case 3:
                player.tempData.collectedSouls += Random.Range(soulAmountMinTier3, soulAmountMaxTier3);
                break;

        }

        OnSoulCollected.Raise();
    }
}

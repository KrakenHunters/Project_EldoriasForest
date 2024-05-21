using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCollectible : Collectible
{
    [SerializeField]
    public int soulAmountMax;
    [SerializeField]
    public int soulAmountMin;

    protected override void ItemCollected(PlayerController player)
    {
        base.ItemCollected(player);
        player.tempData.collectedSouls += Random.Range(soulAmountMin, soulAmountMax);
    }
}

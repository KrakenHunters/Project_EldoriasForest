using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : Collectible
{
    [SerializeField]
    private float healthAmountMax;
    [SerializeField]
    private float healthAmountMin;

    protected override void ItemCollected(PlayerController player)
    {
        base.ItemCollected(player);
        player.tempData.currentHealth += Random.Range(healthAmountMin, healthAmountMax);
    }
}

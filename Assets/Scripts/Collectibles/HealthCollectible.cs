using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : Collectible
{
    [SerializeField]
    private int healthAmountMax;
    [SerializeField]
    private int healthAmountMin;

    protected override void ItemCollected(PlayerController player)
    {
        base.ItemCollected(player);
        player.Heal(Random.Range(healthAmountMin, healthAmountMax));
    }
}

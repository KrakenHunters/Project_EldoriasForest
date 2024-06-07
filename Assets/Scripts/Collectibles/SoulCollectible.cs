using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCollectible : Collectible
{

    [SerializeField]
    private GameEvent<Empty> OnSoulCollected;

    [SerializeField]
    private int soulAmountTier1;
    [SerializeField] private int soulAmountTier2;
    [SerializeField] private int soulAmountTier3;


    protected override void ItemCollected(PlayerController player)
    {
        base.ItemCollected(player);

        switch (tier)
        {
            case 0:
                player.tempData.collectedSouls += soulAmountTier1;
                break;
            case 1:
                player.tempData.collectedSouls += soulAmountTier1;
                break;
            case 2:
                player.tempData.collectedSouls += soulAmountTier2;
                break;
            case 3:
                player.tempData.collectedSouls += soulAmountTier3;
                break;

        }

        //OnSoulCollected.Raise(new Empty());
    }
}

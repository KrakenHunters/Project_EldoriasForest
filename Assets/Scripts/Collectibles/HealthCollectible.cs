using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : Collectible
{
    [SerializeField]
    private int healthAmountMaxTier1;
    [SerializeField]
    private int healthAmountMinTier1;
    [SerializeField]
    private int healthAmountMaxTier2;
    [SerializeField]
    private int healthAmountMinTier2;
    [SerializeField]
    private int healthAmountMaxTier3;
    [SerializeField]
    private int healthAmountMinTier3;

    [SerializeField] private AudioClip collectClip;
    public CollectableAudioEvent collectableAudioEvent;

    protected override void ItemCollected(PlayerController player)
    {
        collectableAudioEvent.ItemCollected.Invoke(collectClip);
        base.ItemCollected(player);
        switch (tier)
        {
            case 0:
                player.Heal(Random.Range(healthAmountMinTier1, healthAmountMaxTier1));
                break;
            case 1:
                player.Heal(Random.Range(healthAmountMinTier1, healthAmountMaxTier1));
                break;
            case 2:
                player.Heal(Random.Range(healthAmountMinTier2, healthAmountMaxTier2));
                break;
            case 3:
                player.Heal(Random.Range(healthAmountMinTier3, healthAmountMaxTier3));
                break;

        }
    }
}

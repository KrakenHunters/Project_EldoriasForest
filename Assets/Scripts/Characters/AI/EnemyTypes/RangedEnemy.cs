using UnityEngine;

public class RangedEnemy : AIController
{
    [SerializeField]
    private BaseSpellBook spellBook;

    protected override void Awake()
    {
        base.Awake();
        if(playerInteractionInformation)
            playerInteractionInformation.playerLocationChange += (location)=>Debug.Log($"I am a ranged enemy and I managed to hear a player movement at location {location}");
    }
    
    public override void AttackPlayer()
    {
        if (spellBook.cooldown <= _attackTimer)
        {
            CastSpell(spellBook);
            _attackTimer = 0;
        }
    }

}

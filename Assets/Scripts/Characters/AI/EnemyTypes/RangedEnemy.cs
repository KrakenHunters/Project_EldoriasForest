using UnityEngine;

public class RangedEnemy : AIController
{
    [SerializeField]
    private BaseSpellBook spellBook;

    public override void AttackPlayer()
    {
        if (spellBook.cooldown <= _attackTimer)
        {
            CastSpell(spellBook);
            _attackTimer = 0;
        }
    }

}

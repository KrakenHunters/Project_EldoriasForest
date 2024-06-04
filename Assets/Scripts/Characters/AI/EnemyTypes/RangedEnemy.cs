using UnityEngine;

public class RangedEnemy : AIController
{
    [SerializeField]
    private BaseSpellBook spellBook;
    private float duration;

    public override void AttackPlayer()
    {
        if (spellBook.cooldown <= _attackTimer)
        {
            CastSpell(spellBook, out duration);
            _attackTimer = 0;
        }
    }

}

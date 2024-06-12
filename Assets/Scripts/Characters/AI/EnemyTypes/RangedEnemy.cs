using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private BaseSpellBook spellBook;

    private float duration;

    public override void Attack()
    {
        base.Attack();
        if (canAttack)
            CastSpell(spellBook, out duration);
    }

}

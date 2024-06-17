using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private BaseSpellBook spellBook;

    private float duration;

    public override void Attack()
    {
        base.Attack();
        CastSpell(spellBook, out duration);
    }

}

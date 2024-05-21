using UnityEngine;

public class MeleeEnemy : AIController
{
    [SerializeField]
    private int _damage = 10;
    [SerializeField]
    private int _cooldown = 10;
    public override void AttackPlayer()
    {
        if (_attackTimer >= _cooldown)
        {
            player.GetComponent<CharacterClass>().GetHit(_damage, this);
            _attackTimer = 0;
        }
    }
}

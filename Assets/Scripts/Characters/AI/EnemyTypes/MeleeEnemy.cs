using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Damage")]
    [SerializeField]
    private float damageTier1;
    [SerializeField]
    private float damageTier2;
    [SerializeField]
    private float damageTier3;

    private float _damage = 0f;

    protected override void Update()
    {
        base.Update();
        if (!playerDetector.CanAttackPlayer())
            attacking = false;
    }

    public override void Attack()
    {
        base.Attack();
        if (_damage == 0f)
            GetDamage();

        Debug.Log("Attacking");
        playerDetector.controller.GetHit(_damage, this.gameObject, null);

    }

    void GetDamage()
    {
        switch (tier)
        {
            case 1:
                _damage = damageTier1;
                break;
            case 2:
                _damage = damageTier2; 
                break;
            case 3:
                _damage = damageTier3; 
                break;
        }
    }
    
}

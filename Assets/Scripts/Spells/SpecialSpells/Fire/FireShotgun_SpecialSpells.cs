using UnityEngine;

public class FireShotgun_SpecialSpells : SpecialSpellBook
{
    [SerializeField]
    private float angle = 45f; // Angle of the cone

    private SphereCollider damageCollider;

    protected override void CastSpell(int tier)
    {
        Destroy(gameObject, duration);

        damageCollider = GetComponent<SphereCollider>();
        damageCollider.radius = range;
    }
    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction, attacker);
        transform.forward = direction;

    }

    protected override void Update()
    {
        base.Update();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterClass>() && other.gameObject != charAttacker) // Ensure only enemies are affected
        {

            Vector3 directionToEnemy = (other.transform.localPosition - charAttacker.transform.position).normalized;
            float angleToEnemy = Vector3.Angle(charAttacker.transform.forward, directionToEnemy);
            if (angleToEnemy < angle / 2) // Check if the enemy is within the cone angle
            {
                other.GetComponent<CharacterClass>().GetHit(damage, charAttacker, this);
                SetStatusEffect(other.gameObject);

            }
        }

    }

}

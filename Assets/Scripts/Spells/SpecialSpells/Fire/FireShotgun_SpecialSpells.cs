using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireShotgun_SpecialSpells : SpecialSpellBook
{
    [SerializeField]
    private float angle = 45f; // Angle of the cone

    private SphereCollider damageCollider;

    protected override void CastSpell(int tier)
    {
        damageCollider = GetComponent<SphereCollider>();
        damageCollider.radius = range;
    }


    protected override void Update()
    {
        base.Update();

        Destroy(gameObject, duration);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterClass>() && other.gameObject != charAttacker) // Ensure only enemies are affected
        {

            Vector3 directionToEnemy = (other.transform.position - charAttacker.transform.position).normalized;
            float angleToEnemy = Vector3.Angle(charAttacker.transform.forward, directionToEnemy);
            Debug.Log(angleToEnemy);

            if (angleToEnemy < angle / 2) // Check if the enemy is within the cone angle
            {
                Debug.Log("EnemyGot Hit");
                other.GetComponent<CharacterClass>().GetHit(damage, charAttacker, this);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.transform.position, range);

    }
}

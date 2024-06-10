using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Thunder_UltimateSpell : UltimateSpellBook
{
    [SerializeField]
    private Thunder_GameObject thunderEffects;

    private float nextLightningTime;
    [SerializeField]
    private float damageInterval;

    private SphereCollider sphereCollider;

    protected override void CastSpell(int tier)
    {
        base.CastSpell(tier);
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
        Destroy(this.gameObject, duration);
    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction, attacker);
    }


    protected override void Update()
    {
        base.Update();
        if (Time.time >= nextLightningTime)
        {
            // Calculate a random position within the sphere collider's radius
            Vector3 randomPosition = GetRandomPositionWithinRadius();
            Instantiate(thunderEffects, randomPosition, Quaternion.identity);
            nextLightningTime = Time.time + damageInterval;
        }


    }

    private void OnTriggerStay(Collider other)
    {

        if (Time.time >= nextLightningTime && other.GetComponent<CharacterClass>() && other.gameObject != charAttacker)
        {
            CharacterClass enemy = other.GetComponent<CharacterClass>();
            if (enemy != null)
            {
                enemy.GetHit(damage, charAttacker, this);
            }
        }

        //Check if it is an enemy to call a damage function
        //Call an explosion or the after effect for the destroyed object.
    }

    private Vector3 GetRandomPositionWithinRadius()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y;  // Keep the lightning effect on the same Y plane
        return randomDirection;
    }

}

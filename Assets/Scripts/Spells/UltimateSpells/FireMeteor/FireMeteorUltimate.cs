using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMeteorUltimate : UltimateSpellBook
{
    private Vector3 directionToTarget;

    protected override void CastSpell(int tier)
    {
        base.CastSpell(tier);
    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction, attacker);
        directionToTarget = (direction - transform.position).normalized;
    }


    protected override void Update()
    {
        base.Update();
        transform.Translate(directionToTarget * projectileSpeed * Time.deltaTime);
        //Stop when reaching upper limit

        if (transform.position.y <= -20)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CharacterClass>() && other.gameObject != charAttacker)
        {
            other.GetComponent<CharacterClass>().GetHit(damage, charAttacker, this);

        }

        //Check if it is an enemy to call a damage function
        //Call an explosion or the after effect for the destroyed object.
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireMeteorUltimate : UltimateSpellBook
{
    private Vector3 directionToTarget;
    [SerializeField]
    private GameObject explosionEffect;

    private bool explosionCalled;
    private Vector3 targetPos;

    protected override void CastSpell(int tier)
    {
        base.CastSpell(tier);
    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction, attacker);
        targetPos = direction;
        directionToTarget = (direction - transform.position).normalized;

    }


    protected override void Update()
    {
        base.Update();
        transform.Translate(directionToTarget * projectileSpeed * Time.deltaTime);
        //Stop when reaching upper limit

        if (transform.position.y <= radius && !explosionCalled)
        {
            GameObject explosion = Instantiate(explosionEffect, targetPos, Quaternion.identity);
            Destroy(explosion, 3f);
            explosionCalled = true;
        }

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

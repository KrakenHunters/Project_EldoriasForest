using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard_UltimateSpell : UltimateSpellBook
{
    private Quaternion fixedRotation;
    protected override void CastSpell(int tier)
    {
        base.CastSpell(tier);
        Destroy(this.gameObject, duration);
        fixedRotation = transform.rotation;
    }

    protected override void Update()
    {
        base.Update();
        transform.rotation = fixedRotation;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CharacterClass>() && other.gameObject != charAttacker)
        {
            other.GetComponent<CharacterClass>().GetHit(damage * Time.deltaTime, charAttacker, this);

        }


    }

}

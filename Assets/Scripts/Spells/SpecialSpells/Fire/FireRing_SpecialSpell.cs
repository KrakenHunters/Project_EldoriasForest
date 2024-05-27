using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireRing_SpecialSpell : SpecialSpellBook
{

    [SerializeField]
    private float spellTimer;

    [SerializeField]
    private float ringRadius = 5f;

    [SerializeField]
    private int healAmount= 10;


    private SphereCollider damageCollider;

    protected override void CastSpell(int tier)
    {
        damageCollider = GetComponent<SphereCollider>();
        damageCollider.radius = ringRadius;
        //HealCaster();
        charAttacker.Heal(healAmount);
    }


    protected override void Update()
    {
        base.Update();

        if (timer >= spellTimer)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Ensure only enemies are affected
        {
            other.GetComponent<CharacterClass>().GetHit(damage, charAttacker, this);

        }

    }

}

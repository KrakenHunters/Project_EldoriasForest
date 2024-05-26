using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShield_SpecialSpell : SpecialSpellBook
{
    private BoxCollider damageTrigger;

    [SerializeField]
    private float spellTimer;

    [SerializeField]
    private float _distance;

    protected override void CastSpell(int tier)
    {
        damageTrigger = GetComponentInChildren<BoxCollider>();
        
    }

    protected override void Update()
    {
        base.Update();
        
        transform.position = charAttacker.transform.position + charAttacker.transform.forward * _distance;

        if (timer >= spellTimer)
        {
           Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemySpell"))
        {
            Destroy(other.gameObject);
        }
    }

}

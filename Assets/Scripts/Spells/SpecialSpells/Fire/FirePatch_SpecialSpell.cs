using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePatch_SpecialSpell : BaseSpellBook
{
    private float damagePerSecond;
    private float damageInterval = 1f; // Time interval for each tick of damage
    private float nextDamageTime = 0f;

    private GameObject attacker;
    private SpecialSpellBook spellbook;


    protected override void Awake()
    {

    }
    public override void UpgradeTier()
    {

    }

    protected override void Update()
    {

    }

    public void Initialize(float damage, float lifetime, GameObject charAttacker, SpecialSpellBook spell)
    {
        damagePerSecond = damage;
        spellbook = spell;
        attacker = charAttacker;
        Destroy(gameObject, lifetime);
    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time >= nextDamageTime && other.GetComponent<CharacterClass>() && other.gameObject != attacker)
        {
            CharacterClass enemy = other.GetComponent<CharacterClass>();
            if (enemy != null)
            {
                enemy.GetHit(damagePerSecond, attacker, spellbook);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }

}

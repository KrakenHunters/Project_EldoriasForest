using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePatch_SpecialSpell : MonoBehaviour
{
    private float damagePerSecond;
    private float damageInterval = 1f; // Time interval for each tick of damage
    private float nextDamageTime = 0f;

    private GameObject attacker;
    private SpecialSpellBook spellbook;

    public void Initialize(float damage, float lifetime, GameObject charAttacker, SpecialSpellBook spell)
    {
        damagePerSecond = damage;
        spellbook = spell;
        attacker = charAttacker;
        Destroy(gameObject, lifetime);
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
}

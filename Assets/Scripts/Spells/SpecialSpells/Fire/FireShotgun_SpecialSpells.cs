using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireShotgun_SpecialSpells : SpecialSpellBook
{
    private float maxRange = 10f; // Maximum range of the spell
    private float angle = 45f; // Angle of the cone

    [SerializeField]
    private float spellTimer;

    private Collider damageCollider;

    protected override void CastSpell(int tier)
    {
        FireShotgunSpellStatsContainer fireShotgunContainer = spellData as FireShotgunSpellStatsContainer;
        fireShotgunContainer.SetTierData(tier);
        FireShotgunTierData fireShotgunTierData = fireShotgunContainer.currentTierData as FireShotgunTierData;

        maxRange = fireShotgunTierData.range;
        angle = fireShotgunTierData.angle;

        damageCollider = GetComponent<Collider>();

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
            Vector3 directionToEnemy = (other.transform.position - transform.position).normalized;
            float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

            if (angleToEnemy < angle / 2) // Check if the enemy is within the cone angle
            {
                other.GetComponent<CharacterClass>().GetHit(damage, charAttacker, this);
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IceMachineGun_SpecialSpell : SpecialSpellBook
{
    private float spellTimer;

    [SerializeField]
    private float interval;

    [SerializeField]
    private SpellBook iceBullet;

    private Vector3 shotDirection;

    protected override void CastSpell(int tier)
    {

        LightningBeamSpellStatsContainer container = spellData as LightningBeamSpellStatsContainer;
        container.SetTierData(tier);
        LightningBeamTierData iceMachineGunContainer = container.currentTierData as LightningBeamTierData;

        spellTimer = iceMachineGunContainer.duration;

    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction, attacker);
        transform.SetParent(charAttacker.transform);
        StartCoroutine(FireProjectiles());
        Destroy(gameObject, spellTimer);

    }

    IEnumerator FireProjectiles()
    {
        float endTime = Time.time + spellTimer;

        while (Time.time < endTime)
        {
            shotDirection = charAttacker.transform.forward;

            SpellBook bullet = Instantiate(iceBullet, transform.position, transform.rotation); // Instantiate the projectile
            bullet.tier = this.tier;
            bullet.Shoot(shotDirection, charAttacker);
            yield return new WaitForSeconds(interval);
        }
    }
}

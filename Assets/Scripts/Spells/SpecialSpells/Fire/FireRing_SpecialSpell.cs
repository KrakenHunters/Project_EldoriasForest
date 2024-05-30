using System.Collections;
using UnityEngine;

public class FireRing_SpecialSpell : SpecialSpellBook
{

    [SerializeField]
    private float spellTimer;

    [SerializeField]
    private float ringRadius = 5f;

    private int HealAmount;


    private SphereCollider damageCollider;

    protected override void CastSpell(int tier)
    {
        FireRingSpellStatsContainer f = spellData as FireRingSpellStatsContainer;
        f.SetTierData(tier);
        FireRingTierData c = f.currentTierData as FireRingTierData;
        HealAmount = c.healAmount;

        damageCollider = GetComponentInChildren<SphereCollider>();
        damageCollider.radius = ringRadius;
        if(GetComponentInParent<CharacterClass>() != null)
        {
            charAttacker = GetComponentInParent<CharacterClass>().gameObject;
            StartCoroutine(HealCaster());
        }
    }


    private IEnumerator HealCaster()
    {
        int playerHealed = 0;

        while (playerHealed < HealAmount)
        {
            playerHealed += 1;
            charAttacker.GetComponent<CharacterClass>()?.Heal(playerHealed);

            yield return new WaitForSeconds(1f);
        }
        yield return null;
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

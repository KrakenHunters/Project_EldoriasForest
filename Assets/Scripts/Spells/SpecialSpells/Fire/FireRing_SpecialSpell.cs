using System.Collections;
using UnityEngine;

public class FireRing_SpecialSpell : SpecialSpellBook
{

    private float healAmount;

    private SphereCollider damageCollider;

    protected override void CastSpell(int tier)
    {

        healAmount = spellData.currentTierData.healAmount;
        damageCollider = GetComponentInChildren<SphereCollider>();
        damageCollider.radius = radius;
        if(GetComponentInParent<CharacterClass>() != null)
        {
            charAttacker = GetComponentInParent<CharacterClass>().gameObject;
            StartCoroutine(HealCaster());
        }
    }


    private IEnumerator HealCaster()
    {
        int playerHealed = 0;

        while (playerHealed < healAmount)
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
        Destroy(gameObject, duration);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Ensure only enemies are affected
        {
            other.GetComponent<CharacterClass>().GetHit(damage, charAttacker, this);

        }

    }

}

using System.Collections;
using UnityEngine;

public class FireRing_SpecialSpell : SpecialSpellBook
{

    private float healAmount;

    private SphereCollider damageCollider;

    private Quaternion initialRotation;

    protected override void CastSpell(int tier)
    {
        Destroy(gameObject, duration);
        initialRotation = transform.rotation;
        healAmount = spellData.currentTierData.healAmount;
        damageCollider = GetComponentInChildren<SphereCollider>();
        damageCollider.radius = radius;
        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.ShapeModule shapeModule = particle.shape;

        shapeModule.radius = radius;
        if(GetComponentInParent<CharacterClass>() != null)
        {
            charAttacker = GetComponentInParent<CharacterClass>().gameObject;
            StartCoroutine(HealCaster());
        }
    }

    protected override void Update()
    {
        base.Update();
        transform.rotation = initialRotation;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterClass>() && charAttacker != other) // Ensure only enemies are affected
        {
            other.GetComponent<CharacterClass>().GetHit(damage, charAttacker, this);
            SetStatusEffect(other.gameObject);

        }

    }

}

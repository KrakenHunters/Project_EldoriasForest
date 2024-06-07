using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail_SpecialSpell : SpecialSpellBook
{
    [SerializeField]
    private GameObject firePatchPrefab;  // Prefab of the fire patch with collider
    [SerializeField]
    private float firePatchInterval = 0.5f; // Interval between fire patches

    private float patchLifetime;    // Lifetime of each fire patch
    private bool isCasting = false;
    private Vector3 castDirection;

    protected override void CastSpell(int tier)
    {
        patchLifetime = spellData.currentTierData.duration;

    }

    private IEnumerator CreateFireTrail()
    {
        isCasting = true;

        Vector3 startPosition = new Vector3 (transform.position.x, -0.7f, transform.position.z);
        Vector3 forwardDirection = castDirection;

        int numberOfPatches = Mathf.CeilToInt(range / firePatchInterval);

        for (int i = 0; i < numberOfPatches; i++)
        {
            Vector3 firePatchPosition = startPosition + forwardDirection * (i * firePatchInterval);
            CreateFirePatch(firePatchPosition);

            yield return new WaitForSeconds(firePatchInterval);
        }

        isCasting = false;
    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction, attacker);
        castDirection = direction;
        if (!isCasting)
        {
            StartCoroutine(CreateFireTrail());
        }
    }


    private void CreateFirePatch(Vector3 position)
    {
        GameObject firePatch = Instantiate(firePatchPrefab, position, Quaternion.identity);
        FirePatch_SpecialSpell firePatchScript = firePatch.GetComponent<FirePatch_SpecialSpell>();
        firePatchScript.Initialize(damage, patchLifetime, charAttacker, this);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightningBeam_SpecialSpell : SpecialSpellBook
{
    [SerializeField]
    private LayerMask obstacleLayer;
    [SerializeField]
    private LayerMask enemyLayer;

    private float damagePerSecond;
    private LineRenderer lineRenderer;
    private Vector3 castDirection;

    private float duration;

    private Vector3 startPos;

    [SerializeField]
    private float maxDistance;

    protected override void CastSpell(int tier)
    {
        LightningBeamSpellStatsContainer container = spellData as LightningBeamSpellStatsContainer;
        container.SetTierData(tier);
        LightningBeamTierData beamContainer = container.currentTierData as LightningBeamTierData;

        damagePerSecond = beamContainer.damage;
        duration = beamContainer.duration;

        lineRenderer = GetComponent<LineRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        castDirection = charAttacker.transform.forward;
        startPos = transform.position;


    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction, attacker);
        transform.SetParent(charAttacker.transform);
        startPos = transform.position;
        //castDirection = direction;
        StartCoroutine(LightningCoroutine());
    }

    IEnumerator LightningCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 end = startPos + castDirection * maxDistance;
            if (Physics.Raycast(startPos, castDirection, out RaycastHit hit, maxDistance, obstacleLayer))
            {
                end = hit.point;
            }

            // Draw the lightning bolt
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, end);

            // Handle damage to enemies
            RaycastHit[] hits = Physics.RaycastAll(startPos, castDirection, (end - startPos).magnitude, enemyLayer);
            foreach (RaycastHit enemyHit in hits)
            {
                CharacterClass enemy = enemyHit.collider.GetComponent<CharacterClass>();
                if (enemy != null)
                {
                    enemy.GetHit(damagePerSecond * Time.deltaTime, charAttacker, this);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;

        }

        Destroy(gameObject);

    }
}

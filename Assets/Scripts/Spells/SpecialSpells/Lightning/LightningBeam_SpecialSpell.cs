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

    private LineRenderer lineRenderer;
    private Vector3 castDirection;

    private Vector3 startPos;
    protected override void CastSpell(int tier)
    {
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
        timer = 0f;
        StartCoroutine(LightningCoroutine());
    }

    IEnumerator LightningCoroutine()
    {
        while (timer <= duration)
        {
            Vector3 end = startPos + castDirection * range;

            // Perform a single raycast that checks for both obstacles and enemies
            int combinedLayerMask = obstacleLayer | enemyLayer;
            if (Physics.Raycast(startPos, castDirection, out RaycastHit hit, range, combinedLayerMask, QueryTriggerInteraction.Ignore))
            {
                end = hit.point;
                Debug.Log(hit.collider);

                // Check if the hit is an enemy
                if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
                {
                    CharacterClass enemy = hit.collider.GetComponent<CharacterClass>();
                    if (enemy != null)
                    {
                        enemy.GetHit(damage * Time.deltaTime, charAttacker, this);
                        SetStatusEffect(enemy.transform);

                    }
                }
            }

            // Draw the lightning bolt
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, end);

            yield return null;
        }
        Destroy(gameObject);


    }
}

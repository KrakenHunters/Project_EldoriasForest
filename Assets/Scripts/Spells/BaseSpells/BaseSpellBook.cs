using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpellBook : SpellBook
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float range;

    [SerializeField]
    private float aimRange = 10f; // Range within which the auto-aim checks for enemies
    [SerializeField]
    private float aimAngle = 15f; // Cone angle in degrees for auto-aim

    protected Vector3 startPos;

    private Transform target;

    protected override void Awake()
    {
        tier = GameManager.Instance.pdata.baseAttackTier;
        base.Awake();

    }
    protected override void UpgradeTier()
    {
        base.UpgradeTier();
        GameManager.Instance.pdata.baseAttackTier = tier;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        startPos = transform.position;
        target = FindClosestEnemyWithinCone();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (target != null)
        {
            Debug.Log("Aim-Assist");
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }


        if (Vector3.Distance(transform.position, startPos) > range)
        {
            Destroy(this.gameObject);
        }
    }

    private Transform FindClosestEnemyWithinCone()
    {
        Collider[] hitColliders = Physics.OverlapSphere(startPos, aimRange);
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 forward = transform.forward;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Vector3 directionToTarget = hitCollider.transform.position - startPos;
                float angle = Vector3.Angle(forward, directionToTarget);

                if (angle < aimAngle / 2)
                {
                    float distanceSqr = directionToTarget.sqrMagnitude;
                    if (distanceSqr < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr;
                        closestEnemy = hitCollider.transform;
                    }
                }
            }
        }

        return closestEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if it is an enemy to call a damage function
        //Call an explosion or the after effect for the destroyed object.
        Destroy(this.gameObject);
    }
}

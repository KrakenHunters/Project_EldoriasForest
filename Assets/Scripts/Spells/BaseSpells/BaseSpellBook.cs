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

    private Vector3 targetDirection;

    protected override void Awake()
    {
        castOrigin = castType.projectile;
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

    }

    // Update is called once per frame
    protected override void Update()
    {
        //transform.Translate(targetDirection * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, startPos) > range)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Shoot(Vector3 direction, GameObject attacker)
    {
        base.Shoot(direction, attacker);
        startPos = transform.position;
        if (attacker.GetComponent<PlayerController>())
        {
            PlayerSpellCastManager.Instance.currentBaseSpellCooldown = cooldown;
            targetDirection = FindClosestEnemyWithinCone(direction);
            tier = GameManager.Instance.pdata.baseAttackTier;

        }
        else if (attacker.GetComponent<SpecialSpellBook>())
        {
            targetDirection = FindClosestEnemyWithinCone(direction);
            tier = charAttacker.gameObject.GetComponent<SpellBook>().tier;

        }
        else
        {
            tier = charAttacker.gameObject.GetComponent<CharacterClass>().tier;
            targetDirection = direction;
        }

        GetComponent<Rigidbody>().velocity = targetDirection * speed;
    }

    private Vector3 FindClosestEnemyWithinCone(Vector3 direction)
    {
        Collider[] hitColliders = Physics.OverlapSphere(startPos, aimRange);
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 forward = direction;
        Vector3 returnDirection = forward;


        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<AIController>())
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
                        returnDirection = (closestEnemy.transform.position - transform.position).normalized;
                    }
                }
            }
        }

        return returnDirection;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CharacterClass>())
        {
            other.GetComponent<CharacterClass>().GetHit(damage, charAttacker, this);
            Debug.Log("Got Hit");


        }
        Destroy(this.gameObject);

        //Check if it is an enemy to call a damage function
        //Call an explosion or the after effect for the destroyed object.
    }
}
;
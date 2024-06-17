using UnityEngine;

public class BaseSpellBook : SpellBook
{
    [SerializeField]
    private float autoAimRange = 10f; // Range within which the auto-aim checks for enemies
    [SerializeField]
    private float autoAimAngle = 15f; // Cone angle in degrees for auto-aim

    protected Vector3 startPos;

    private Vector3 targetDirection;

    protected override void Awake()
    {
        castOrigin = castType.projectile;
        base.Awake();
    }
    public override void UpgradeTier()
    {
        base.UpgradeTier();
        GameManager.Instance.pData.baseAttackTier = tier;
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
            attacker.GetComponent<PlayerSpellCastManager>().currentBaseSpellCooldown = cooldown;
            targetDirection = FindClosestEnemyWithinCone(direction);
            tier = GameManager.Instance.pData.baseAttackTier;

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

        GetComponent<Rigidbody>().velocity = targetDirection * projectileSpeed;
    }

    private Vector3 FindClosestEnemyWithinCone(Vector3 direction)
    {
        Collider[] hitColliders = Physics.OverlapSphere(startPos, autoAimRange);
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 forward = direction;
        Vector3 returnDirection = forward;


        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Enemy>())
            {
                Vector3 directionToTarget = hitCollider.transform.position - startPos;
                float angle = Vector3.Angle(forward, directionToTarget);

                if (angle < autoAimAngle / 2)
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

        if (other.GetComponent<CharacterClass>() && other.gameObject != charAttacker)
        {
            other.GetComponent<CharacterClass>().GetHit(damage, charAttacker, this);
            SetStatusEffect(other.transform);

        }
        Destroy(this.gameObject);

        //Check if it is an enemy to call a damage function
        //Call an explosion or the after effect for the destroyed object.
    }
}
;
using UnityEngine;

public class LightningBall_SpecialSpell : SpecialSpellBook
{
    private SphereCollider damageTrigger;

    [SerializeField]
    private float _rotateRadius;
    private float angle;

    protected override void CastSpell(int tier)
    {
        damageTrigger = GetComponentInChildren<SphereCollider>();
    }

    protected override void Update()
    {
        base.Update();
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
            damageTrigger.enabled = false;
            return;
        }
        OrbitAroundParent();
    }

    private void OrbitAroundParent()
    {
        angle += projectileSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * _rotateRadius;
        float z = Mathf.Sin(angle) * _rotateRadius;
        transform.position = new Vector3(x, 0, z) + transform.parent.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        var character = other.GetComponent<CharacterClass>();
        if (character != null)
        {
            character.GetHit(damage, charAttacker, this);
            SetStatusEffect(other.transform);

        }
    }
}

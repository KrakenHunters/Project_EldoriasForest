using UnityEngine;

public class LightningBall_SpecialSpell : SpecialSpellBook
{
    private SphereCollider damageTrigger;

    [SerializeField]
    private float spellTimer;

    [SerializeField]
    private float _rotateRadius;
    [SerializeField]
    private float speed;

    private float angle;

    protected override void CastSpell(int tier)
    {
        damageTrigger = GetComponentInChildren<SphereCollider>();
    }

    protected override void Update()
    {
        base.Update();
        spellTimer -= Time.deltaTime;
        if (spellTimer <= 0)
        {
            Destroy(gameObject);
            damageTrigger.enabled = false;
            return;
        }
        OrbitAroundParent();
    }

    private void OrbitAroundParent()
    {
        angle += speed * Time.deltaTime;
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
            Debug.Log("Got Hit");
        }
    }
}

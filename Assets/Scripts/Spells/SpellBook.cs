using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    protected int tier = 1;
    public float cooldown;
    protected CharacterClass charAttacker;
    public castType castOrigin;

    [SerializeField]
    protected int damage;

    protected float timer;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Spell");
        CastSpell(tier);
    }
    protected virtual void UpgradeTier()
    {
        if (tier < 3)
        {
            tier++;
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;

    }

    protected virtual void CastSpell(int tier)
    {

    }

    public virtual void Shoot(Vector3 direction,CharacterClass attacker)
    {

      charAttacker = attacker;

    }

    public enum castType
    {
        groundPos,
        skyToGroundPos,
        self,
        projectile
    }
}

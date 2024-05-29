using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    protected int tier = 1;
    [HideInInspector]
    public float cooldown;
    protected CharacterClass charAttacker;
    public castType castOrigin;

    protected int damage;

    protected float timer;

    [SerializeField]
    protected SpellStatsContainer spellData;

    #region StatusEffect
    public StatusEffect statusEffect;

    [HideInInspector]
    public float statusEffectTimer;
    [HideInInspector]
    public float statusEffectDamage;
    [HideInInspector]
    public float statusEffectChance;

    #endregion


    public enum StatusEffect
    {
        LightningEffect,
        FireEffect,
        IceEffect,
        None
    }


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Spell");
        SetDataFromSpellContainer();
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

    protected void SetDataFromSpellContainer()
    {
        spellData.SetTierData(tier);
        damage = spellData.currentTierData.damage;
        statusEffectTimer = spellData.currentTierData.statusEffectTimer;
        statusEffectDamage = spellData.currentTierData.statusEffectDamage;
        statusEffectChance = spellData.currentTierData.statusEffectChance;

        cooldown = spellData.currentTierData.cooldown;

    }



    public enum castType
    {
        groundPos,
        skyToGroundPos,
        self,
        projectile
    }
}

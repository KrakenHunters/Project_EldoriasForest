using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpellBook : SpellBook
{
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSpecials : MonoBehaviour
{
    [Header("Special Attack Upgrade")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int upgradeCost = 100;

    private void Start()
    {
        ShopManager.Instance.CheckButtonInteraction(upgradeButton, CanUpgradeSpecialSpell(), 0);
    }

    public void UpgradeTier(SpecialSpellBook spell)
    {
        ShopManager.Instance.CheckButtonInteraction(upgradeButton, CanUpgradeSpecialSpell(), upgradeCost);
       // ShopManager.Instance.permData.spellBooksUnlocked.Contains();
    }

    public bool CanUpgradeSpecialSpell()
    {
       
        return ShopManager.Instance.permData.baseAttackTier <= 2 && ShopManager.Instance.permData.totalSouls >= upgradeCost;
    }


}

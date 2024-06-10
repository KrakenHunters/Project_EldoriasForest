using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopCharacterStats : MonoBehaviour
{
    PermanentDataContainer permData;
    [Header("Character Stats")]
    [SerializeField] private CharacterUpgrade ultimateSpellSlot;
    [SerializeField] private CharacterUpgrade healthUpgrade;
    [SerializeField] private CharacterUpgrade DefenseRune;
    [SerializeField] private CharacterUpgrade LootDropRate;
    [SerializeField] private CharacterUpgrade CoolDownReduction;
    private void Awake()
    {
        permData = ShopManager.Instance.permData;
        SetUpValues();
    }

    private void SetUpValues()
    {
        CoolDownReduction.cost = 100; 
        CoolDownReduction.maxUnlock = 5;

        LootDropRate.maxUnlock = 5;
        LootDropRate.cost = 100;

        DefenseRune.maxUnlock = 3;
        DefenseRune.cost = 100;

        healthUpgrade.maxUnlock = 5;
        healthUpgrade.cost = 100;

        ultimateSpellSlot.maxUnlock = 1;
        ultimateSpellSlot.cost = 100;

        UpdateUI();
        ShopManager.Instance.CheckButtonInteraction(ultimateSpellSlot.upgradeButton, CanUpgradeUltimateSpellSlot(), ultimateSpellSlot.cost);
        ShopManager.Instance.CheckButtonInteraction(healthUpgrade.upgradeButton, CanUpgradeHealth(), healthUpgrade.cost);
        ShopManager.Instance.CheckButtonInteraction(DefenseRune.upgradeButton, CanUpgradeDefenseRune(), DefenseRune.cost);
        ShopManager.Instance.CheckButtonInteraction(LootDropRate.upgradeButton, CanUpgradeLootDropRate(), LootDropRate.cost);
        ShopManager.Instance.CheckButtonInteraction(CoolDownReduction.upgradeButton, CanUpgradeCoolDownReduction(), CoolDownReduction.cost);
    }

    public void UpdateUI()
    {
        ultimateSpellSlot.currentUnlock = permData.IsUltimateSpellSlotUnlocked ? 1 : 0;
        ultimateSpellSlot.unlockText.text = $"{ultimateSpellSlot.currentUnlock} / {ultimateSpellSlot.maxUnlock}";
        ultimateSpellSlot.costText.text = ultimateSpellSlot.cost.ToString();

        healthUpgrade.currentUnlock = permData.healthBonus;
        healthUpgrade.unlockText.text = $"{healthUpgrade.currentUnlock} / {healthUpgrade.maxUnlock}";
        healthUpgrade.costText.text = healthUpgrade.cost.ToString();

        DefenseRune.currentUnlock = permData.rune;
        DefenseRune.unlockText.text = $"{DefenseRune.currentUnlock} / {DefenseRune.maxUnlock}";
        DefenseRune.costText.text = DefenseRune.cost.ToString();

        LootDropRate.currentUnlock = Mathf.RoundToInt(permData.templeSoulsDropRate);
        LootDropRate.unlockText.text = $"{LootDropRate.currentUnlock} / {LootDropRate.maxUnlock}";
        LootDropRate.costText.text = LootDropRate.cost.ToString();

        CoolDownReduction.currentUnlock = Mathf.RoundToInt(permData.cooldownReduction);
        CoolDownReduction.unlockText.text = $"{CoolDownReduction.currentUnlock} / {CoolDownReduction.maxUnlock}";
        CoolDownReduction.costText.text = CoolDownReduction.cost.ToString();
    }
    public void OnUltimateSpellSlotUpgrade()
    {
            ShopManager.Instance.permData.totalSouls -= ultimateSpellSlot.cost;
            ShopManager.Instance.permData.IsUltimateSpellSlotUnlocked = true;
            ultimateSpellSlot.currentUnlock = 1;
            ShopManager.Instance.CheckButtonInteraction(ultimateSpellSlot.upgradeButton, CanUpgradeUltimateSpellSlot(), ultimateSpellSlot.cost);
            UpdateUI();
    }

    public void OnHealthUpgrade()
    {
        permData.totalSouls -= healthUpgrade.cost;
        permData.healthBonus++;
        healthUpgrade.currentUnlock++;
        healthUpgrade.cost += (3*healthUpgrade.cost);
        ShopManager.Instance.CheckButtonInteraction(healthUpgrade.upgradeButton, CanUpgradeHealth(), healthUpgrade.cost);
        UpdateUI();
    }

    public void OnDefenseRuneUpgrade()
    {
        permData.totalSouls -= DefenseRune.cost;
        permData.rune++;
        DefenseRune.currentUnlock++;
        DefenseRune.cost += (3*DefenseRune.cost);
        ShopManager.Instance.CheckButtonInteraction(DefenseRune.upgradeButton, CanUpgradeDefenseRune(), DefenseRune.cost);
        UpdateUI();
    }

    public void OnLootDropRateUpgrade()
    {
        permData.totalSouls -= LootDropRate.cost;
        permData.templeSoulsDropRate += 0.25f;
        LootDropRate.currentUnlock++;
        LootDropRate.cost += (3*LootDropRate.cost);
        ShopManager.Instance.CheckButtonInteraction(LootDropRate.upgradeButton, CanUpgradeLootDropRate(), LootDropRate.cost);
        UpdateUI();
    }

    public void OnCoolDownReductionUpgrade()
    {
        permData.totalSouls -= CoolDownReduction.cost;
        permData.cooldownReduction += 0.25f;
        CoolDownReduction.currentUnlock++;
        CoolDownReduction.cost += (3*CoolDownReduction.cost);
        ShopManager.Instance.CheckButtonInteraction(CoolDownReduction.upgradeButton, CanUpgradeCoolDownReduction(), CoolDownReduction.cost);
        UpdateUI();
    }

    #region Upgrade Checks 

    public bool CanUpgradeUltimateSpellSlot()
    {
        return permData.totalSouls >= ultimateSpellSlot.cost && ultimateSpellSlot.currentUnlock == 0;
    }

    public bool CanUpgradeHealth()
    {
        return permData.totalSouls >= healthUpgrade.cost && healthUpgrade.currentUnlock < healthUpgrade.maxUnlock;
    }

    public bool CanUpgradeDefenseRune()
    {
        return permData.totalSouls >= DefenseRune.cost && DefenseRune.currentUnlock < DefenseRune.maxUnlock;
    }

    public bool CanUpgradeLootDropRate()
    {
        return permData.totalSouls >= LootDropRate.cost && LootDropRate.currentUnlock < LootDropRate.maxUnlock;
    }

    public bool CanUpgradeCoolDownReduction()
    {
        return permData.totalSouls >= CoolDownReduction.cost && CoolDownReduction.currentUnlock < CoolDownReduction.maxUnlock;
    }

    #endregion


}

[Serializable]
public class CharacterUpgrade
{
    public int currentUnlock;
    public int maxUnlock;
    public int cost;
    public Button upgradeButton;
    public TMPro.TextMeshProUGUI unlockText;
    public TMPro.TextMeshProUGUI costText;
}


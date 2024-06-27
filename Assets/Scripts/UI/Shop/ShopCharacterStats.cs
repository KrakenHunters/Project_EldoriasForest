using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopCharacterStats : MonoBehaviour, IShoppable
{
    
    [Header("Character Stats")]
    [SerializeField] private CharacterUpgrade ultimateSpellSlot;
    [SerializeField] private CharacterUpgrade healthUpgrade;
    [SerializeField] private CharacterUpgrade DefenseRune;
    [SerializeField] private CharacterUpgrade LootDropRate;
    [SerializeField] private CharacterUpgrade CoolDownReduction;

    [SerializeField] private IntGameEvent OnUpdateUISouls;
    [SerializeField] private EmptyGameEvent OnBuyStuff;

    PermanentDataContainer permData;
    private void Awake()
    {
        permData = ShopManager.Instance.permData;
        SetUpValues();
    }

    public void UpdateButtonInteractions()
    {
        UpdateUI();
        CheckAllButtonInteraction();
    }

    public void UpdateSoulsCountUI(int cost)
    {
        ShopManager.Instance.CostUIUpdate(cost);
    }

    private void SetUpValues()
    {
        CoolDownReduction.cost = permData.spellCooldownCost;
        CoolDownReduction.maxUnlock = 3;

        LootDropRate.maxUnlock = 5;
        LootDropRate.cost = permData.soulDropUpgradeCost;

        DefenseRune.maxUnlock = 3;
        DefenseRune.cost = permData.defensiveRuneCost;

        healthUpgrade.maxUnlock = 5;
        healthUpgrade.cost = permData.healthUpgradeCost;

        ultimateSpellSlot.maxUnlock = 1;
        ultimateSpellSlot.cost = 500;

        UpdateButtonInteractions();

    }

    public void UpdateUI()
    {

        ultimateSpellSlot.currentUnlock = permData.IsUltimateSpellSlotUnlocked ? 1 : 0;
        ultimateSpellSlot.unlockText.text = $"{ultimateSpellSlot.currentUnlock} / {ultimateSpellSlot.maxUnlock}";
        ultimateSpellSlot.costText.text = ultimateSpellSlot.cost.ToString();
        if (ultimateSpellSlot.currentUnlock == 1)
            ultimateSpellSlot.costText.text = "Max";


        healthUpgrade.currentUnlock = Mathf.FloorToInt(permData.healthBonus/permData.healthBonusIncrement) < 0 ? 0 : Mathf.FloorToInt(permData.healthBonus / permData.healthBonusIncrement);
        healthUpgrade.unlockText.text = $"{healthUpgrade.currentUnlock} / {healthUpgrade.maxUnlock}";
        healthUpgrade.costText.text = healthUpgrade.cost.ToString();
        if (healthUpgrade.currentUnlock == healthUpgrade.maxUnlock)
            healthUpgrade.costText.text = "Max";

        DefenseRune.currentUnlock = Mathf.FloorToInt(permData.rune / permData.runeIncrement) < 0 ? 0 : Mathf.FloorToInt(permData.rune / permData.runeIncrement);
        DefenseRune.unlockText.text = $"{DefenseRune.currentUnlock} / {DefenseRune.maxUnlock}";
        DefenseRune.costText.text = DefenseRune.cost.ToString();
        if (DefenseRune.currentUnlock == DefenseRune.maxUnlock)
            DefenseRune.costText.text = "Max";

        LootDropRate.currentUnlock =  Mathf.FloorToInt(permData.templeSoulsDropRate / permData.templeSoulsDropRateIncrement) < 0 ? 0 : Mathf.FloorToInt(permData.templeSoulsDropRate / permData.templeSoulsDropRateIncrement);
        LootDropRate.unlockText.text = $"{LootDropRate.currentUnlock} / {LootDropRate.maxUnlock}";
        LootDropRate.costText.text = LootDropRate.cost.ToString();
        if (LootDropRate.currentUnlock == LootDropRate.maxUnlock)
            LootDropRate.costText.text = "Max";

        CoolDownReduction.currentUnlock = Mathf.FloorToInt(permData.cooldownReduction / permData.cooldownReductionIncrement) < 0 ? 0 : Mathf.FloorToInt(permData.cooldownReduction / permData.cooldownReductionIncrement);
        CoolDownReduction.unlockText.text = $"{CoolDownReduction.currentUnlock} / {CoolDownReduction.maxUnlock}";
        CoolDownReduction.costText.text = CoolDownReduction.cost.ToString();
        if (CoolDownReduction.currentUnlock == CoolDownReduction.maxUnlock)
            CoolDownReduction.costText.text = "Max";
    }
    public void OnUltimateSpellSlotUpgrade()
    {
        ShopManager.Instance.permData.totalSouls -= ultimateSpellSlot.cost;
        ShopManager.Instance.permData.IsUltimateSpellSlotUnlocked = true;
        ultimateSpellSlot.currentUnlock = 1;
        UpdateSoulsCountUI(ultimateSpellSlot.cost);
        OnBuyStuff.Raise(new Empty());
    }

    public void OnHealthUpgrade()
    {
        permData.totalSouls -= healthUpgrade.cost;
        permData.healthBonus+= permData.healthBonusIncrement;
        healthUpgrade.currentUnlock++;
        UpdateSoulsCountUI(healthUpgrade.cost);
        permData.healthUpgradeCost += (3 * healthUpgrade.cost);
        healthUpgrade.cost = permData.healthUpgradeCost;

        OnBuyStuff.Raise(new Empty());

    }

    public void OnDefenseRuneUpgrade()
    {
        permData.totalSouls -= DefenseRune.cost;
        permData.rune += permData.runeIncrement;
        DefenseRune.currentUnlock++;
        UpdateSoulsCountUI(DefenseRune.cost);
        permData.defensiveRuneCost += (3 * DefenseRune.cost);
        DefenseRune.cost = permData.defensiveRuneCost;

        OnBuyStuff.Raise(new Empty());
    }

    public void OnLootDropRateUpgrade()
    {
        permData.totalSouls -= LootDropRate.cost;
        permData.templeSoulsDropRate += permData.templeSoulsDropRateIncrement;
        LootDropRate.currentUnlock++;
        UpdateSoulsCountUI(LootDropRate.cost);
        permData.soulDropUpgradeCost += (3 * LootDropRate.cost);
        LootDropRate.cost = permData.soulDropUpgradeCost;

        OnBuyStuff.Raise(new Empty());
    }

    public void OnCoolDownReductionUpgrade()
    {
        permData.totalSouls -= CoolDownReduction.cost;
        permData.cooldownReduction += permData.cooldownReductionIncrement;
        CoolDownReduction.currentUnlock++;
        UpdateSoulsCountUI(CoolDownReduction.cost);
        permData.spellCooldownCost += (3 * CoolDownReduction.cost);
        CoolDownReduction.cost = permData.spellCooldownCost;
        OnBuyStuff.Raise(new Empty());
    }

    private void CheckAllButtonInteraction()
    {

        ShopManager.Instance.CheckButtonInteraction(ultimateSpellSlot.upgradeButton, CanUpgradeUltimateSpellSlot());
        ShopManager.Instance.CheckButtonInteraction(healthUpgrade.upgradeButton, CanUpgradeHealth());
        ShopManager.Instance.CheckButtonInteraction(DefenseRune.upgradeButton, CanUpgradeDefenseRune());
        ShopManager.Instance.CheckButtonInteraction(LootDropRate.upgradeButton, CanUpgradeLootDropRate());
        ShopManager.Instance.CheckButtonInteraction(CoolDownReduction.upgradeButton, CanUpgradeCoolDownReduction());


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


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

        UpdateButtonInteractions();

    }

    public void UpdateUI()
    {

        ultimateSpellSlot.currentUnlock = permData.IsUltimateSpellSlotUnlocked ? 1 : 0;
        ultimateSpellSlot.unlockText.text = $"{ultimateSpellSlot.currentUnlock} / {ultimateSpellSlot.maxUnlock}";
        ultimateSpellSlot.costText.text = ultimateSpellSlot.cost.ToString();
        if (ultimateSpellSlot.currentUnlock == 1)
            ultimateSpellSlot.costText.text = "Max";


        healthUpgrade.currentUnlock = permData.healthBonus;
        healthUpgrade.unlockText.text = $"{healthUpgrade.currentUnlock} / {healthUpgrade.maxUnlock}";
        healthUpgrade.costText.text = healthUpgrade.cost.ToString();
        if (healthUpgrade.currentUnlock == healthUpgrade.maxUnlock)
            healthUpgrade.costText.text = "Max";

        DefenseRune.currentUnlock = permData.rune;
        DefenseRune.unlockText.text = $"{DefenseRune.currentUnlock} / {DefenseRune.maxUnlock}";
        DefenseRune.costText.text = DefenseRune.cost.ToString();
        if (DefenseRune.currentUnlock == DefenseRune.maxUnlock)
            DefenseRune.costText.text = "Max";

        LootDropRate.currentUnlock =  Mathf.FloorToInt(permData.templeSoulsDropRate / 0.25f);
        LootDropRate.unlockText.text = $"{LootDropRate.currentUnlock} / {LootDropRate.maxUnlock}";
        LootDropRate.costText.text = LootDropRate.cost.ToString();
        if (LootDropRate.currentUnlock == LootDropRate.maxUnlock)
            LootDropRate.costText.text = "Max";

        CoolDownReduction.currentUnlock = Mathf.FloorToInt(permData.cooldownReduction / 0.25f);
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
        permData.healthBonus++;
        healthUpgrade.currentUnlock++;
        UpdateSoulsCountUI(healthUpgrade.cost);
        healthUpgrade.cost += (3 * healthUpgrade.cost);
        OnBuyStuff.Raise(new Empty());

    }

    public void OnDefenseRuneUpgrade()
    {
        permData.totalSouls -= DefenseRune.cost;
        permData.rune++;
        DefenseRune.currentUnlock++;
        UpdateSoulsCountUI(DefenseRune.cost);
        DefenseRune.cost += (3 * DefenseRune.cost);
        OnBuyStuff.Raise(new Empty());
    }

    public void OnLootDropRateUpgrade()
    {
        permData.totalSouls -= LootDropRate.cost;
        permData.templeSoulsDropRate += 0.25f;
        LootDropRate.currentUnlock++;
        UpdateSoulsCountUI(LootDropRate.cost);
        LootDropRate.cost += (3 * LootDropRate.cost);
        OnBuyStuff.Raise(new Empty());
    }

    public void OnCoolDownReductionUpgrade()
    {
        permData.totalSouls -= CoolDownReduction.cost;
        permData.cooldownReduction += 0.25f;
        CoolDownReduction.currentUnlock++;
        UpdateSoulsCountUI(CoolDownReduction.cost);
        CoolDownReduction.cost += (3 * CoolDownReduction.cost);
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


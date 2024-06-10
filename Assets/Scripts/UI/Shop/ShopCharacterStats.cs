using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopCharacterStats : MonoBehaviour
{

    [Header("Character Stats")]
    [SerializeField] private CharacterUpgrade ultimateSpellSlot;
    [SerializeField] private CharacterUpgrade healthUpgrade;
    [SerializeField] private CharacterUpgrade DefenseRune;
    [SerializeField] private CharacterUpgrade LootDropRate;
    [SerializeField] private CharacterUpgrade CoolDownReduction;

    private void SetUpValues()
    {
        var permData = ShopManager.Instance.permData;

        ultimateSpellSlot.maxUnlock = 1;
        ultimateSpellSlot.currentUnlock = permData.IsUltimateSpellSlotUnlocked ? 1 : 0;
        ultimateSpellSlot.cost = 100;
        ultimateSpellSlot.unlockText.text = $"{ultimateSpellSlot.currentUnlock} / {ultimateSpellSlot.maxUnlock}";
        ultimateSpellSlot.costText.text = ultimateSpellSlot.cost.ToString();

        healthUpgrade.maxUnlock = 5;
        healthUpgrade.currentUnlock = permData.healthBonus;
        healthUpgrade.cost = 100;
        healthUpgrade.unlockText.text = $"{healthUpgrade.currentUnlock} / {healthUpgrade.maxUnlock}";
        healthUpgrade.costText.text = healthUpgrade.cost.ToString();

        DefenseRune.maxUnlock = 3;
        DefenseRune.currentUnlock = permData.rune;
        DefenseRune.cost = 100;
        DefenseRune.unlockText.text = $"{DefenseRune.currentUnlock} / {DefenseRune.maxUnlock}";
        DefenseRune.costText.text = DefenseRune.cost.ToString();

        LootDropRate.maxUnlock = 5;
        LootDropRate.currentUnlock = Mathf.RoundToInt(permData.templeSoulsDropRate);
        LootDropRate.cost = 100;
        LootDropRate.unlockText.text = $"{LootDropRate.currentUnlock} / {LootDropRate.maxUnlock}";
        LootDropRate.costText.text = LootDropRate.cost.ToString();

        CoolDownReduction.maxUnlock = 5;
        CoolDownReduction.currentUnlock = Mathf.RoundToInt(permData.cooldownReduction);
        CoolDownReduction.cost = 100;
        CoolDownReduction.unlockText.text = $"{CoolDownReduction.currentUnlock} / {CoolDownReduction.maxUnlock}";
        CoolDownReduction.costText.text = CoolDownReduction.cost.ToString();
    }

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


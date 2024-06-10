using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSpecials : MonoBehaviour
{
    [Header("Special Spell Upgrades")]
    [SerializeField] private int upgradeCost = 100;
    [SerializeField] private int upgradeCostMultiplier = 2;

    [Header("Re-Rolling Spells")]
    [SerializeField] private int reRollMax = 3;
    [SerializeField] private int reRollCost = 100;
    [SerializeField] private int reRollMultiplier = 2;

    [Header("UI Elements")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button reRollButton;
    [SerializeField] private TMPro.TextMeshProUGUI upgradeCostText;
    [SerializeField] private TMPro.TextMeshProUGUI reRollCostText;

    [Header("Spell Selection")]
    [SerializeField] private Button spell1Button;
    [SerializeField] private Button spell2Button;
    [SerializeField] private Button spell3Button;
    [SerializeField] private Image spell1Image;
    [SerializeField] private Image spell2Image;
    [SerializeField] private Image spell3Image;

    private SpecialSpellBook spell1;
    private SpecialSpellBook spell2;
    private SpecialSpellBook spell3;

    private List<SpecialSpellBook> availableSpells = new List<SpecialSpellBook>();
    private List<SpecialSpellBook> usedSpells = new List<SpecialSpellBook>();

    private void Start()
    {
        // Add button listeners
        spell1Button.onClick.AddListener(() => OnPlayerChooseSpell(1));
        spell2Button.onClick.AddListener(() => OnPlayerChooseSpell(2));
        spell3Button.onClick.AddListener(() => OnPlayerChooseSpell(3));
        upgradeButton.onClick.AddListener(OnSpecialSpellUpgrade);
        reRollButton.onClick.AddListener(OnReRollSpells);

        // Initialize costs
        upgradeCostText.text = upgradeCost.ToString();
        reRollCostText.text = reRollCost.ToString();

        // Load and display initial set of spells
        LoadAvailableSpells();
        SelectRandomSpecialSpells();
        UpdateButtonInteractivity();
    }

    private void LoadAvailableSpells()
    {
        availableSpells.Clear();
        var spellBooks = ShopManager.Instance.permData.spellBooksUnlocked;
        foreach (var spellBook in spellBooks)
        {
            if (spellBook is SpecialSpellBook specialSpell)
            {
                availableSpells.Add(specialSpell);
            }
        }
    }

    private void SelectRandomSpecialSpells()
    {
        usedSpells.Clear();
        spell1 = null;
        spell2 = null;
        spell3 = null;

        if (availableSpells.Count == 0)
        {
            Debug.Log("No special spells unlocked");
            return;
        }

        if (availableSpells.Count > 0)
        {
            spell1 = GetRandomSpecialSpell();
            spell1Image.sprite = spell1.spellIcon;
        }

        if (availableSpells.Count > 0)
        {
            spell2 = GetRandomSpecialSpell();
            spell2Image.sprite = spell2.spellIcon;
        }

        if (availableSpells.Count > 0)
        {
            spell3 = GetRandomSpecialSpell();
            spell3Image.sprite = spell3.spellIcon;
        }

        UpdateButtonInteractivity();
    }

    private SpecialSpellBook GetRandomSpecialSpell()
    {
        int index = Random.Range(0, availableSpells.Count);
        SpecialSpellBook spell = availableSpells[index];
        availableSpells.RemoveAt(index);
        usedSpells.Add(spell);
        return spell;
    }

    public void OnSpecialSpellUpgrade()
    {
        if (ShopManager.Instance.tempData.specialSpell != null)
        {
            UpgradeTier(ShopManager.Instance.tempData.specialSpell);
        }
    }

    private void UpgradeTier(SpecialSpellBook spell)
    {
        if (spell.tier < 3 && ShopManager.Instance.permData.totalSouls >= upgradeCost)
        {
            spell.tier++;
            ShopManager.Instance.permData.totalSouls -= upgradeCost;
            upgradeCost *= upgradeCostMultiplier;
            upgradeCostText.text = upgradeCost.ToString();
            UpdateButtonInteractivity();
        }
    }

    public void OnReRollSpells()
    {
        if (reRollMax > 0 && ShopManager.Instance.permData.totalSouls >= reRollCost)
        {
            reRollMax--;
            ShopManager.Instance.permData.totalSouls -= reRollCost;
            reRollCost *= reRollMultiplier;
            reRollCostText.text = reRollCost.ToString();

            LoadAvailableSpells();

            if (spell1 != null && availableSpells.Count > 0)
            {
                spell1 = GetRandomSpecialSpell();
                spell1Image.sprite = spell1.spellIcon;
            }

            if (spell2 != null && availableSpells.Count > 0)
            {
                spell2 = GetRandomSpecialSpell();
                spell2Image.sprite = spell2.spellIcon;
            }

            if (spell3 != null && availableSpells.Count > 0)
            {
                spell3 = GetRandomSpecialSpell();
                spell3Image.sprite = spell3.spellIcon;
            }

            UpdateButtonInteractivity();
        }
    }

    public void OnPlayerChooseSpell(int spellNum)
    {
        switch (spellNum)
        {
            case 1:
                ShopManager.Instance.tempData.specialSpell = spell1;
                break;
            case 2:
                ShopManager.Instance.tempData.specialSpell = spell2;
                break;
            case 3:
                ShopManager.Instance.tempData.specialSpell = spell3;
                break;
        }
        UpdateButtonInteractivity();
    }

    private void UpdateButtonInteractivity()
    {
        upgradeButton.interactable = CanUpgradeSpecialSpell();
        reRollButton.interactable = CanReRollSpells();

        spell1Button.interactable = spell1 != null;
        spell2Button.interactable = spell2 != null;
        spell3Button.interactable = spell3 != null;
    }

    private bool CanUpgradeSpecialSpell()
    {
        return ShopManager.Instance.tempData.specialSpell != null &&
               ShopManager.Instance.tempData.specialSpell.tier < 3 &&
               ShopManager.Instance.permData.totalSouls >= upgradeCost &&
               spell1 != spell2 != spell3;
    }

    private bool CanReRollSpells()
    {
        return reRollMax > 0 && ShopManager.Instance.permData.totalSouls >= reRollCost && (spell1 != null || spell2 != null || spell3 != null) && availableSpells.Count > 0;
    }
}

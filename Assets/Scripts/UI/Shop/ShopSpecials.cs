using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSpecials : MonoBehaviour, IShoppable
{

    [Header("Special Spell Upgrades")]
    [SerializeField] private int upgradeCost = 100;
    [SerializeField] private int upgradeCostMultiplier = 2;

    [Header("Re-Rolling Spells")]
    [SerializeField] private int reRollMax = 3;
    [SerializeField] private int reRollCost = 100;
    [SerializeField] private int reRollMultiplier = 2;

    [Header("UI Elements")]
    [SerializeField] private Button buyButton;
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

    [Header("Locked Spells")]
    [SerializeField] private GameObject lockedSpell1;
    [SerializeField] private GameObject lockedSpell2;
    [SerializeField] private GameObject lockedSpell3;

    [Header("Current Selected Spell Icons")]
    [SerializeField] private Image spell1SelectedIcon;
    [SerializeField] private Image spell2SelectedIcon;
    [SerializeField] private Image spell3SelectedIcon;

    [Header("Tiers")]
    [SerializeField] private Image tierImage;
    [SerializeField] private Sprite tier1;
    [SerializeField] private Sprite tier2;
    [SerializeField] private Sprite tier3;

    [Header("Events")]
    [SerializeField] private EmptyGameEvent OnBuyStuff;

    private ShopManager shopManager;

    [HideInInspector]
    public SpecialSpellBook spell1;
    [HideInInspector]
    public SpecialSpellBook spell2;
    [HideInInspector]
    public SpecialSpellBook spell3;

    private SpecialSpellBook selectedSpell;
    private SpecialSpellBook boughtSpell;

    private List<SpecialSpellBook> availableSpells = new List<SpecialSpellBook>();
    private List<SpecialSpellBook> usedSpells = new List<SpecialSpellBook>();


    public GameEvent<Empty> OnStartShopKeeper;


    private void Start()
    {
        shopManager = ShopManager.Instance;

        // Add button listeners
        spell1Button.onClick.AddListener(() => OnPlayerChooseSpell(1));
        spell2Button.onClick.AddListener(() => OnPlayerChooseSpell(2));
        spell3Button.onClick.AddListener(() => OnPlayerChooseSpell(3));

        // Initialize costs
        upgradeCostText.text = upgradeCost.ToString();
        reRollCostText.text = reRollCost.ToString();

        buyButton.interactable = false;


        ResetAllSpellTiers();

        DisableSelectionIcons();


        // Load and display initial set of spells
        LoadAvailableSpells();
        SelectRandomSpecialSpells();

        lockedSpell1.SetActive(spell1 == null);
        lockedSpell2.SetActive(spell2 == null);
        lockedSpell3.SetActive(spell3 == null);

        UpdateButtonInteractions();

       OnStartShopKeeper.Raise(new Empty());
    }
    public void SetTier(SpellBook spell)
    {
        switch (spell.tier)
        {
            case 1:
                tierImage.sprite = tier1;
                break;
            case 2:
                tierImage.sprite = tier2;
                break;
            case 3:
                tierImage.sprite = tier3;
                break;
        }
    }
    private void ResetAllSpellTiers()
    {
        foreach (var spell in shopManager.permData.spellBooksUnlocked)
        {
            if (spell is SpecialSpellBook specialSpell)
            {
                specialSpell.tier = 1;
            }
        }
    }

    private void LoadAvailableSpells()
    {
        availableSpells.Clear();
        var spellBooks = shopManager.permData.spellBooksUnlocked;
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

        UpdateButtonInteractions();
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
        if (shopManager.tempData.specialSpell != null && CanUpgradeSpecialSpell())
        {
            UpdateSoulsCountUI(upgradeCost);

            UpgradeTier(shopManager.tempData.specialSpell);



            if (shopManager.tempData.specialSpell.tier == 3)
            {
                upgradeCostText.text = "Max";
            }
        }
    }

    private void UpgradeTier(SpecialSpellBook spell)
    {
        if (spell.tier < 3) //&& ShopManager.Instance.permData.totalSouls >= upgradeCost
        {
            spell.tier++;
            shopManager.permData.totalSouls -= upgradeCost;
            upgradeCost *= upgradeCostMultiplier;
            upgradeCostText.text = upgradeCost.ToString();
            SetTier(spell);
            OnBuyStuff.Raise(new Empty());
        }
    }

    public void OnReRollSpells()
    {
        if (reRollMax > 0 && shopManager.permData.totalSouls >= reRollCost)
        {
            reRollMax--;
            UpdateSoulsCountUI(reRollCost);
            reRollCost *= reRollMultiplier;
            reRollCostText.text = reRollCost.ToString();
            if (reRollMax == 0)
            {
                reRollCostText.text = "Max";
            }
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

            OnBuyStuff.Raise(new Empty());

        }
    }

    public void OnBuySpell()
    {
        ShopManager.Instance.permData.totalSouls -= upgradeCost;

        UpdateSoulsCountUI(upgradeCost);
        boughtSpell = selectedSpell;
        shopManager.tempData.specialSpell = boughtSpell;
        UpdateButtonInteractions();
        lockedSpell1.SetActive(spell1 != boughtSpell);
        lockedSpell2.SetActive(spell2 != boughtSpell);
        lockedSpell3.SetActive(spell3 != boughtSpell);

        spell1Button.interactable = spell1 == boughtSpell;
        spell2Button.interactable = spell2 == boughtSpell;
        spell3Button.interactable = spell3 == boughtSpell;
        SetTier(boughtSpell);

    }

    public void OnPlayerChooseSpell(int spellNum)
    {
        DisableSelectionIcons();
        switch (spellNum)
        {
            case 1:
                selectedSpell = spell1;
                spell1SelectedIcon.gameObject.SetActive(true);
                break;
            case 2:
                selectedSpell = spell2;
                spell2SelectedIcon.gameObject.SetActive(true);
                break;
            case 3:
                selectedSpell = spell3;
                spell3SelectedIcon.gameObject.SetActive(true);
                break;
        }

        OnBuyStuff.Raise(new Empty());
    }

    public void UpdateButtonInteractions()
    {
        upgradeButton.interactable = CanUpgradeSpecialSpell();
        reRollButton.interactable = CanReRollSpells();
        buyButton.interactable = CanBuySpell();

        spell1Button.interactable = (spell1 != null) && (spell1 == selectedSpell || boughtSpell == null);
        spell2Button.interactable = (spell2 != null) && (spell2 == selectedSpell || boughtSpell == null);
        spell3Button.interactable = (spell3 != null) && (spell3 == selectedSpell || boughtSpell == null);
    }

    public void UpdateSoulsCountUI(int cost)
    {
        shopManager.CostUIUpdate(cost);
    }

    private void DisableSelectionIcons()
    {
        spell1SelectedIcon.gameObject.SetActive(false);
        spell2SelectedIcon.gameObject.SetActive(false);
        spell3SelectedIcon.gameObject.SetActive(false);
    }

    private bool CanBuySpell()
    {
        return shopManager.permData.totalSouls >= upgradeCost &&
            (spell1 != null || spell2 != null || spell3 != null) &&
             selectedSpell != null;
    }

    private bool CanUpgradeSpecialSpell()
    {
        return shopManager.tempData.specialSpell != null &&
               shopManager.tempData.specialSpell.tier < 3 &&
               shopManager.permData.totalSouls >= upgradeCost;
    }

    private bool CanReRollSpells()
    {
        return reRollMax > 0 &&
            shopManager.permData.totalSouls >= reRollCost &&
            (spell1 != null || spell2 != null || spell3 != null) &&
            availableSpells.Count > 0;
    }
}

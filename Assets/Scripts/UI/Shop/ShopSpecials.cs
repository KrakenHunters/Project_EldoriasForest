using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSpecials : MonoBehaviour
{
    [Header("Special Spell Upgrades")]
    [SerializeField] private int upgradeCost = 100;
    [SerializeField] private int upgradeCostMultiplyer = 2;

    [SerializeField] private int reRollMax = 3;
    [SerializeField] private int reRollCost = 100;
    [SerializeField] private int reRollMultiplyer = 2;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button reRollButton;

    [SerializeField] private TMPro.TextMeshProUGUI upgradeCostText;
    [SerializeField] private TMPro.TextMeshProUGUI reRollCostText;

    [Header("Spell Shop Images")]
    [SerializeField] private Image Spell_1_Image;
    [SerializeField] private Image Spell_2_Image;
    [SerializeField] private Image Spell_3_Image;

    private SpecialSpellBook spell_1;
    private SpecialSpellBook spell_2;
    private SpecialSpellBook spell_3;

    private List<SpecialSpellBook> specialSpells = new();
    private List<SpecialSpellBook> usedSpecialSpells = new();

    private void Start()
    {
        ShopManager.Instance.CheckButtonInteraction(upgradeButton, CanUpgradeSpecialSpell(), 0);
        ShopManager.Instance.CheckButtonInteraction(reRollButton, CanReRollSpells(), 0);
        SelectRandomSpecialSpells();
    }

    private void SelectRandomSpecialSpells()
    {
        int spellBookCount = ShopManager.Instance.permData.spellBooksUnlocked.Count;
        if(spellBookCount == 0)
        {  
            Debug.Log("No special spells unlocked");
            return;
        }
        for (int i = 0; i < spellBookCount; i++)
        {
            if (ShopManager.Instance.permData.spellBooksUnlocked[i] is SpecialSpellBook)
            {
                specialSpells.Add((SpecialSpellBook)ShopManager.Instance.permData.spellBooksUnlocked[i]);
                Debug.Log("count is " + specialSpells.Count);
            }
        }

        SpecialSpellBook GetRandomSpecialSpell(List<SpecialSpellBook> spells, List<SpecialSpellBook> used)
        {
            SpecialSpellBook spell = spells[Random.Range(0, spells.Count)];
            do
            {
                spell = spells[Random.Range(0, spells.Count)];
                if (!used.Contains(spell))
                {
                    used.Add(spell);
                    specialSpells.Remove(spell);
                }
            } while (!used.Contains(spell));
            return spell;
        }

        spell_1 = GetRandomSpecialSpell(specialSpells, usedSpecialSpells);
        Spell_1_Image.sprite = spell_1.spellIcon;

        spell_2 = GetRandomSpecialSpell(specialSpells, usedSpecialSpells);
        Spell_2_Image.sprite = spell_2.spellIcon;

        spell_3 = GetRandomSpecialSpell(specialSpells, usedSpecialSpells);
        Spell_3_Image.sprite = spell_3.spellIcon;
    }

    public void UpgradeTier(SpecialSpellBook spell)
    {
        upgradeCost *= upgradeCostMultiplyer;
        upgradeCostText.text = upgradeCost.ToString();
        ShopManager.Instance.CheckButtonInteraction(upgradeButton, CanUpgradeSpecialSpell(), upgradeCost);
        ShopManager.Instance.tempData.specialSpell.tier++;
    }

    public void OnReRollSpells()
    {
        reRollMax--;
        ShopManager.Instance.permData.totalSouls -= reRollCost;
        reRollCost *= reRollMultiplyer;
        upgradeCostText.text = upgradeCost.ToString();
        ShopManager.Instance.CheckButtonInteraction(reRollButton, CanReRollSpells(), reRollCost);
        SelectRandomSpecialSpells();
    }
    public void OnSpecialSpellUpgrade()
    {
        UpgradeTier(ShopManager.Instance.tempData.specialSpell);
    }

    public void OnPlayerChooseSpell(int spellnum)
    {

        switch (spellnum)
        {
            case 1:
                ShopManager.Instance.tempData.specialSpell = spell_1;
                break;
            case 2:
                ShopManager.Instance.tempData.specialSpell = spell_2;
                break;
            case 3:
                ShopManager.Instance.tempData.specialSpell = spell_3;
                break;
        }

        ShopManager.Instance.CheckButtonInteraction(upgradeButton, CanUpgradeSpecialSpell(), 0);
    }

    public bool CanUpgradeSpecialSpell()
    {

        //check if the tier for that spell is unlocked 
        return ShopManager.Instance.tempData.specialSpell != null && ShopManager.Instance.tempData.specialSpell.tier <= 2 && ShopManager.Instance.permData.totalSouls >= upgradeCost;
    }

    public bool CanReRollSpells()
    {
        return ShopManager.Instance.permData.totalSouls >= reRollCost && reRollMax > 0;
    }


}

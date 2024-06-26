using UnityEngine;
using UnityEngine.UI;

public class ShopBase : MonoBehaviour , IShoppable
{

    [Header("Base Shop Selected")]
    [SerializeField] private Image baseFireSelected;
    [SerializeField] private Image baseIceSelected;
    [SerializeField] private Image baseLightningSelected;

    [Header("Upgrade Cost")]
    [SerializeField] private int upgradeCost = 100;
    [SerializeField] private int costMultiplyer;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMPro.TextMeshProUGUI upgradeCostText;

    [Header("Base Shop Items")]
    public BaseFireSpell fireSpell;
    public BaseFrostSpell iceSpell;
    public BaseElectricityspell lightningSpell;

    //[SerializeField] private Image baseSpellIcon;


    public BaseShopItems currentbasespell = BaseShopItems.None;
    [SerializeField] private EmptyGameEvent OnBuyStuff;

    private void Start()
    {
        upgradeCost = ShopManager.Instance.permData.baseUpgradeCost;
        ConvertSpellToType(ShopManager.Instance.permData.prefBaseSpell);
        ShopManager.Instance.CheckButtonInteraction(upgradeButton, CanUpgradeBaseSpell());
        upgradeCostText.text = upgradeCost.ToString();
        if (ShopManager.Instance.permData.baseAttackTier == 3)
            upgradeCostText.text = "Max";
    }

    public void OnSelectBaseSpell()
    {
        baseFireSelected.gameObject.SetActive(false);
        baseIceSelected.gameObject.SetActive(false);
        baseLightningSelected.gameObject.SetActive(false);

        switch (currentbasespell)
        {
            case BaseShopItems.Fire:
                baseFireSelected.gameObject.SetActive(true);
                break;
            case BaseShopItems.Ice:
                baseIceSelected.gameObject.SetActive(true);
                break;
            case BaseShopItems.Lightning:
                baseLightningSelected.gameObject.SetActive(true);
                break;
        }
        //baseSpellIcon.sprite = ShopManager.Instance.permData.prefBaseSpell.spellIcon;
    }

    public void ConvertSpellToType(BaseSpellBook spell)
    {
        switch (spell)
        {
            case BaseFireSpell fire:
                currentbasespell = BaseShopItems.Fire;
                OnSelectBaseSpell();
                break;
            case BaseFrostSpell ice:
                currentbasespell = BaseShopItems.Ice;
                OnSelectBaseSpell();
                break;
            case BaseElectricityspell lightning:
                currentbasespell = BaseShopItems.Lightning;
                OnSelectBaseSpell();
                break;
            default:
                currentbasespell = BaseShopItems.None;
                break;

        }
    }

    public void OnSelectFire()
    {
        currentbasespell = BaseShopItems.Fire;
        ShopManager.Instance.permData.prefBaseSpell = fireSpell;
        OnSelectBaseSpell();
    }

    public void OnSelectIce()
    {
        currentbasespell = BaseShopItems.Ice;
        ShopManager.Instance.permData.prefBaseSpell = iceSpell;
        OnSelectBaseSpell();
    }

    public void OnSelectLightning()
    {
        currentbasespell = BaseShopItems.Lightning;
        ShopManager.Instance.permData.prefBaseSpell = lightningSpell;
        OnSelectBaseSpell();
    }

    public void UpgradeTier()
    {
        ShopManager.Instance.permData.baseAttackTier++;
        ShopManager.Instance.permData.totalSouls -= upgradeCost;

        //Upgrade cost event
        UpdateSoulsCountUI(upgradeCost);
        upgradeCost *= costMultiplyer;
        ShopManager.Instance.permData.baseUpgradeCost = upgradeCost;
        upgradeCostText.text = upgradeCost.ToString();
        if (ShopManager.Instance.permData.baseAttackTier == 3)
            upgradeCostText.text = "Max";



        OnBuyStuff.Raise(new Empty());
    }

    public void UpdateButtonInteractions()
    {
        ShopManager.Instance.CheckButtonInteraction(upgradeButton, CanUpgradeBaseSpell());

    }
    public void UpdateSoulsCountUI(int cost)
    {
        ShopManager.Instance.CostUIUpdate(upgradeCost);
    }
    public bool CanUpgradeBaseSpell()
    {
        return ShopManager.Instance.permData.baseAttackTier <= 2 && ShopManager.Instance.permData.totalSouls >= upgradeCost;
    }

    public enum BaseShopItems
    {
        Fire,
        Ice,
        Lightning,
        None
    }
}

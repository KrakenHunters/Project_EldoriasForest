using UnityEngine;
using UnityEngine.UI;

public class ShopBase : MonoBehaviour
{
    [Header("Base Shop Selected")]
    [SerializeField] private Image baseFireSelected;
    [SerializeField] private Image baseIceSelected;
    [SerializeField] private Image baseLightningSelected;

    [Header("Upgrade Cost")]
    [SerializeField] private int upgradeCost = 100;
    [SerializeField] private Button upgradeButton;

    [Header("Base Shop Items")]
    [SerializeField] private BaseFireSpell fireSpell;
    [SerializeField] private BaseFrostSpell iceSpell;
    [SerializeField] private BaseElectricityspell lightningSpell;

    public BaseShopItems currentbasespell = BaseShopItems.None;

    private void Start()
    {
        ConvertSpellToType(ShopManager.Instance.permData.prefBaseSpell);
        ShopManager.Instance.CheckButtonInteraction(upgradeButton, CanUpgradeBaseSpell(), 0);
    }

    private void Update()
    {
        
    }


    public void OnSelectBaseSpell()
    {
        switch (currentbasespell)
        {
            case BaseShopItems.Fire:
                baseFireSelected.gameObject.SetActive(true);
                baseIceSelected.gameObject.SetActive(false);
                baseLightningSelected.gameObject.SetActive(false);
                break;
            case BaseShopItems.Ice:
                baseFireSelected.gameObject.SetActive(false);
                baseIceSelected.gameObject.SetActive(true);
                baseLightningSelected.gameObject.SetActive(false);
                break;
            case BaseShopItems.Lightning:
                baseFireSelected.gameObject.SetActive(false);
                baseIceSelected.gameObject.SetActive(false);
                baseLightningSelected.gameObject.SetActive(true);
                break;
        }
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
        ShopManager.Instance.CheckButtonInteraction(upgradeButton, CanUpgradeBaseSpell(), upgradeCost);

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

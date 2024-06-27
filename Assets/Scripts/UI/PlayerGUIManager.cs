using UnityEngine;
using UnityEngine.UI;

public class PlayerGUIManager : MonoBehaviour
{
    [SerializeField]
    private Image currentBaseSpell;
    [SerializeField]
    private Image currentSpecialSpell;
    [SerializeField]
    private Image currentUltimateSpell;

    public TMPro.TextMeshProUGUI soulCountText;

    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TMPro.TextMeshProUGUI healthAmountNum;

    [SerializeField]
    private float speedBar = 2f;

    [SerializeField]
    private TMPro.TextMeshProUGUI specialSpellCooldownText;
    [SerializeField]
    private Image specialSpellCooldownGreyImage;

    [SerializeField]
    private Image ultLockIcon;

    [Header("Tiers")]
    [SerializeField] private Image baseTierImage;
    [SerializeField] private Image specialTierImage;
    [SerializeField] private Sprite tier1;
    [SerializeField] private Sprite tier2;
    [SerializeField] private Sprite tier3;

    [SerializeField]
    private TemporaryDataContainer tempData;

    [SerializeField]
    private DoubleFloatEvent onCooldownChange;
    private float targetHealth;

    private void OnEnable()
    {
        onCooldownChange.OnValueChanged.AddListener(SetCooldown);
    }

    private void OnDisable()
    {
        onCooldownChange.OnValueChanged.RemoveListener(SetCooldown);
    }

    private void Start()
    {
        ultLockIcon.enabled = !GameManager.Instance.pData.IsUltimateSpellSlotUnlocked;
        SetSpellIcons();
        SetSoulCount();
        SetTier();
    }

    public void SetTier()
    {
        baseTierImage.gameObject.SetActive(true);
        switch (GameManager.Instance.pData.baseAttackTier)
        {
            case 1:
                baseTierImage.sprite = tier1;
                break;
            case 2:
                baseTierImage.sprite = tier2;
                break;
            case 3:
                baseTierImage.sprite = tier3;
                break;
            default:
                baseTierImage.gameObject.SetActive(false);
                break;
        }

        specialTierImage.gameObject.SetActive(true);
        int? specialtier = GameManager.Instance.tData.specialSpell?.tier ?? 0;
        if(specialtier != 0)
        specialTierImage.sprite = GetSpriteByTier(specialtier.Value);
        else
            specialTierImage.gameObject.SetActive(false);


    }

    public Sprite GetSpriteByTier(int tier)
    {
        switch (tier)
        {
            case 1:
                return tier1;
            case 2:
                return tier2;
            case 3:
                return tier3;
            default:
                return null;
        }
    }

    public void SetSpellIcons()
    {
        if (tempData.baseSpell != null)
            currentBaseSpell.sprite = tempData.baseSpell.spellIcon;
        if (tempData.specialSpell != null)
            currentSpecialSpell.sprite = tempData.specialSpell.spellIcon;
        if (tempData.ultimateSpell != null)
            currentUltimateSpell.sprite = tempData.ultimateSpell.spellIcon;
    }

    public void SetSoulCount()
    {
        soulCountText.text = tempData.collectedSouls.ToString();
    }

    public void SetHealthValues(float health)
    {
        healthBar.maxValue = tempData.startHealth + GameManager.Instance.pData.healthBonus;
        targetHealth = health;
    }

    private void Update()
    {
        healthBar.value = Mathf.Lerp(healthBar.value, targetHealth, speedBar * Time.deltaTime);
        healthAmountNum.text = Mathf.Round(healthBar.value).ToString() + " / " + Mathf.Round(healthBar.maxValue).ToString();
    }

    public void SetCooldown(float cooldown, float maxCooldown)
    {

        if (cooldown < 0)
        {
            specialSpellCooldownGreyImage.enabled = true;
        }
        else if (cooldown > 0)
        {
            specialSpellCooldownGreyImage.enabled = true;
            specialSpellCooldownText.enabled = true;

            specialSpellCooldownGreyImage.fillAmount = cooldown / maxCooldown;
            specialSpellCooldownText.text = Mathf.Round(cooldown).ToString();
        }
        else
        {
            specialSpellCooldownGreyImage.enabled = false;
            specialSpellCooldownText.enabled = false;
        }
    }

}

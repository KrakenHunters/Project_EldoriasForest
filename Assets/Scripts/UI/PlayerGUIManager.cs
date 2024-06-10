using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
    private TMPro.TextMeshProUGUI specialSpellCooldownText;
    [SerializeField]
    private Image specialSpellCooldownGreyImage;

    [SerializeField]
    private TemporaryDataContainer tempData;

    [SerializeField]
    private DoubleFloatEvent onCooldownChange;


    private void OnEnable()
    {
        onCooldownChange.AddListener(SetCooldown);
    }

    private void OnDisable()
    {
        onCooldownChange.RemoveListener(SetCooldown);
    }

    private void Start()
    {
        SetSpellIcons();
        SetSoulCount();
    }

    public void SetSpellIcons()
    {
        if(tempData.baseSpell != null)
        currentBaseSpell.sprite = tempData.baseSpell.spellIcon;
        if(tempData.specialSpell != null)
        currentSpecialSpell.sprite = tempData.specialSpell.spellIcon;
        if(tempData.ultimateSpell != null)
            currentUltimateSpell.sprite = tempData.ultimateSpell.spellIcon;
    }

    public void SetSoulCount()
    {
        soulCountText.text = tempData.collectedSouls.ToString(); 
    }

    public void SetHealthValues(float health)
    {
        healthBar.maxValue = tempData.startHealth;
        healthBar.value = health;
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

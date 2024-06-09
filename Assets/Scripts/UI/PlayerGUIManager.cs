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

    private void Start()
    {
        SetSpellIcons();
        SetSoulCount();
    }

    public void SetSpellIcons()
    {
        currentBaseSpell.sprite = tempData.baseSpell.spellIcon;
        currentSpecialSpell.sprite = tempData.specialSpell.spellIcon;
        currentUltimateSpell.sprite = tempData.ultimateSpell.spellIcon;
    }

    public void SetSoulCount()
    {
        //string formatedCount = String.Format("{0:N0}", tempData.collectedSouls;
        soulCountText.text = tempData.collectedSouls.ToString(); 
    }

    public void SetHealthValues(float health)
    {
        healthBar.maxValue = tempData.startHealth;
        healthBar.value = health;
    }

    public void SetCooldown(float cooldown)
    {
        specialSpellCooldownGreyImage.enabled = true;
        specialSpellCooldownText.enabled = true;

        if (cooldown < 0)
        {
            specialSpellCooldownText.text = "Active";
        }
        else if (cooldown > 0)
        {
            specialSpellCooldownText.text = Mathf.RoundToInt(cooldown).ToString();
        }
        else
        {
            specialSpellCooldownGreyImage.enabled = false;
            specialSpellCooldownText.enabled = false;
        }
    }

}

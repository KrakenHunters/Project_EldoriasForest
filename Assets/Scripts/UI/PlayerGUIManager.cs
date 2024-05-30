using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUIManager : Singleton<PlayerGUIManager>
{
    [SerializeField]
    private Image currentBaseSpell;
    [SerializeField]
    private Image currentSpecialSpell;
    [SerializeField]
    private Image currentUltimateSpell;

    [SerializeField]
    private TMPro.TextMeshProUGUI soulCountText;

    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private TemporaryDataContainer tempData;

    private void Start()
    {
        SetSpellIcons();
        SetSoulCount();
    }

    private void SetSpellIcons()
    {
        currentBaseSpell.sprite = tempData.baseSpell.spellIcon;
        currentSpecialSpell.sprite = tempData.specialSpell.spellIcon;
        currentUltimateSpell.sprite = tempData.ultimateSpell.spellIcon;
    }

    public void SetSoulCount()
    {
        string formatedCount = String.Format("{0:N0}", tempData.collectedSouls);
        soulCountText.text = formatedCount;
    }

    public void SetHealthValues(float health)
    {
        healthBar.maxValue = tempData.startHealth;
        healthBar.value = health;
    }
}

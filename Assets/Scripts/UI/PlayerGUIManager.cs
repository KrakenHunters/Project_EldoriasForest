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
    private TemporaryDataContainer tempData;

    [SerializeField]
    private GameEventListener OnSoulsChangedEvent;
    [SerializeField]
    private GameEventListener OnHealthChangedEvent;
    private void OnEnable()
    {
        OnSoulsChangedEvent.Response.AddListener(SetSoulCount);
        OnHealthChangedEvent.FloatResponse.AddListener(SetHealthValues);
    }
    private void OnDisable()
    {
        OnSoulsChangedEvent.Response.RemoveListener(SetSoulCount);
        OnHealthChangedEvent.FloatResponse.RemoveListener(SetHealthValues);
    }
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
}

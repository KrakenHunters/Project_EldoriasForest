using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempleUIManager : Singleton<TempleUIManager>
{
    [SerializeField] private GameObject templeUI;

   // [SerializeField] private GameEvent InteractTemple;

    [Header("Temple UI")]
    [SerializeField] private Image Hearts;
    [SerializeField] private Image Souls;
    [SerializeField] private Image SpecialSpell;

    [Header("Temple UI Text")]
    [SerializeField] private TMPro.TextMeshProUGUI templeHealthText;
    [SerializeField] private TMPro.TextMeshProUGUI templeSoulsText;
    [SerializeField] private TMPro.TextMeshProUGUI templeSpecialSpellText;

    [Header("Temple Tier 1")]
    [SerializeField] private int minTempleTier1Health;
    [SerializeField] private int maxTempleTier1Health;
    [SerializeField] private int minTempleTier1Souls;
    [SerializeField] private int maxTempleTier1Souls;
    [Header("Temple Tier 2")]
    [SerializeField] private int minTempleTier2Health;
    [SerializeField] private int maxTempleTier2Health;
    [SerializeField] private int minTempleTier2Souls;
    [SerializeField] private int maxTempleTier2Souls;
    [Header("Temple Tier 3")]
    [SerializeField] private int minTempleTier3Health;
    [SerializeField] private int maxTempleTier3Health;
    [SerializeField] private int minTempleTier3Souls;
    [SerializeField] private int maxTempleTier3Souls;

    [Header("All SpellBooks")]
    [SerializeField] private List<SpellBook> templeSpecialSpellList;

    [SerializeField]
    private FloatGameEvent OnHealPlayer;
    [SerializeField]
    private EmptyGameEvent OnSoulCollect;
    [SerializeField]
    private EmptyGameEvent OnPlayerPickSpell;


    private int templeHealth;
    private int templeSouls;
    private SpellBook currentTempleSpell;


    private int templeTier;

    public void SetTempleOptions(int tier)
    {
        templeTier = tier;
        switch (tier)
        {
            
            case 1:
                templeHealth = Random.Range(minTempleTier1Health, maxTempleTier1Health);
                templeHealthText.text = templeHealth.ToString();
                templeSouls = Random.Range(minTempleTier1Souls, maxTempleTier1Souls);
                templeSoulsText.text = templeSouls.ToString();
                templeSpecialSpellText.text = "Tier 1";
                break;
            case 2:
                templeHealth = Random.Range(minTempleTier2Health, maxTempleTier2Health);
                templeHealthText.text = templeHealth.ToString();
                templeSouls = Random.Range(minTempleTier2Souls, maxTempleTier2Souls);
                templeSoulsText.text = templeSouls.ToString();
                templeSpecialSpellText.text = "Tier 2";
                break;
            case 3:
                templeHealth = Random.Range(minTempleTier3Health, maxTempleTier3Health);
                templeHealthText.text = templeHealth.ToString();
                templeSouls = Random.Range(minTempleTier3Souls, maxTempleTier3Souls);
                templeSoulsText.text = templeSouls.ToString();
                templeSpecialSpellText.text = "Tier 3";
                break;
        }
       
        templeUI.SetActive(true);

        Time.timeScale = 0.0f;
        //Randomize the values and the objects for the menu
        //Set the values and the objects for the menu
    }

    public void OnHealthButton()
    {
        Time.timeScale = 1.0f;
        OnHealPlayer.Raise(templeHealth);
        templeUI.SetActive(false);   
    }

    public void OnSoulsButton()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.tData.collectedSouls += templeSouls;
        OnSoulCollect.Raise(new Empty());
        templeUI.SetActive(false);

    }

    public void OnSpellButton()
    {
        Time.timeScale = 1.0f;

        if (templeTier == 3)
        {
            currentTempleSpell = templeSpecialSpellList[Random.Range(0, templeSpecialSpellList.Count)];
        }
        else
        {
            while (currentTempleSpell is not SpecialSpellBook)
            {
                currentTempleSpell = templeSpecialSpellList[Random.Range(0, templeSpecialSpellList.Count)];
            }
        }

        if (currentTempleSpell is SpecialSpellBook)
        {
            currentTempleSpell.tier = templeTier;
            GameManager.Instance.tData.specialSpell = currentTempleSpell as SpecialSpellBook;
        }
        else
        {
            GameManager.Instance.tData.ultimateSpell = currentTempleSpell as UltimateSpellBook;
        }


        GameManager.Instance.tData.collectedSpells.Add(currentTempleSpell);

        OnPlayerPickSpell.Raise(new Empty());
        templeUI.SetActive(false);
    }

}

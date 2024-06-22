using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempleUIManager : Singleton<TempleUIManager>
{
    [SerializeField] private GameObject templeUI;
    [SerializeField] private GameObject confirmationSpellScreen;
    [SerializeField] private GameObject celebrationScreen;

    // [SerializeField] private GameEvent InteractTemple;
    [Header("Temple Spell pick")]
    [SerializeField] private Image currentSpellImage;
    [SerializeField] private Image newSpellImage;

    [Header("Temple Spell celebration")]
    [SerializeField] private Image celebrationImage;
    [SerializeField] private TMPro.TextMeshProUGUI celebrationText;



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

    [field: SerializeField]
    public bool keep { get; set; }

    public void SetTempleOptions(int tier)
    {
        templeTier = tier;
        switch (tier)
        {

            case 1:
                templeHealth = Random.Range(minTempleTier1Health, maxTempleTier1Health);
                templeHealthText.text = $"{minTempleTier1Health}-{maxTempleTier1Health}";
                templeSouls = Random.Range(minTempleTier1Souls, maxTempleTier1Souls);
                templeSoulsText.text = $"{minTempleTier1Souls}-{maxTempleTier1Souls}";
                templeSpecialSpellText.text = "Tier 1";
                break;
            case 2:
                templeHealth = Random.Range(minTempleTier2Health, maxTempleTier2Health);
                templeHealthText.text = $"{minTempleTier2Health}-{maxTempleTier2Health}";
                templeSouls = Random.Range(minTempleTier2Souls, maxTempleTier2Souls);
                templeSoulsText.text = $"{minTempleTier2Souls}-{maxTempleTier2Souls}";
                templeSpecialSpellText.text = "Tier 2";
                break;
            case 3:
                templeHealth = Random.Range(minTempleTier3Health, maxTempleTier3Health);
                templeHealthText.text = $"{minTempleTier3Health}-{maxTempleTier3Health}";
                templeSouls = Random.Range(minTempleTier3Souls, maxTempleTier3Souls);
                templeSoulsText.text = $"{minTempleTier3Souls}-{maxTempleTier3Souls}";
                templeSpecialSpellText.text = "Tier 3";
                break;
        }

        templeUI.SetActive(true);

        Time.timeScale = 0.0f;
        //Randomize the values and the objects for the menu
        //Set the values and the objects for the menu
    }

    private SpecialSpellBook CheckSpecialSpellOnPlayer()
    {
        if (GameManager.Instance.tData.specialSpell != null)
            return GameManager.Instance.tData.specialSpell;

        return null;
    }

    private UltimateSpellBook CheckUltimateSpellOnPlayer()
    {
        if (GameManager.Instance.tData.ultimateSpell != null)
            return GameManager.Instance.tData.ultimateSpell;

        return null;
    }


    private void GetSpell()
    {
        int EquippedSpecialSpellTier = 0;
            currentTempleSpell = templeSpecialSpellList[Random.Range(0, templeSpecialSpellList.Count)];
        if (CheckSpecialSpellOnPlayer() != null)
        {
            EquippedSpecialSpellTier = CheckSpecialSpellOnPlayer().tier;
        }

        if (templeTier > EquippedSpecialSpellTier)
        {
            //tier check
            if (templeTier < 3)
            {
                while (currentTempleSpell is not SpecialSpellBook)
                {
                    currentTempleSpell = templeSpecialSpellList[Random.Range(0, templeSpecialSpellList.Count)];
                }
            }
        }
        else
        {
            if (templeTier == 3 && GameManager.Instance.pData.IsUltimateSpellSlotUnlocked)
            {
                while (CheckSpecialSpellOnPlayer() == currentTempleSpell || CheckUltimateSpellOnPlayer() == currentTempleSpell)
                {
                    currentTempleSpell = templeSpecialSpellList[Random.Range(0, templeSpecialSpellList.Count)];
                }
            }
            else
            {
                while (currentTempleSpell is not SpecialSpellBook || CheckSpecialSpellOnPlayer() == currentTempleSpell)
                {
                    currentTempleSpell = templeSpecialSpellList[Random.Range(0, templeSpecialSpellList.Count)];
                }
            }
        }
    }

    /*    private SpellBook GetXSpell()
        {
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
                GameManager.Instance.tData.specialSpell = currentTempleSpell as SpecialSpellBook; //fixthis to optional
            }
            else
            {
                GameManager.Instance.tData.ultimateSpell = currentTempleSpell as UltimateSpellBook;
            }
            return currentTempleSpell;
        }
    */
    public void OnHealthButton()
    {

        OnHealPlayer.Raise(templeHealth);
        //celebrationImage.sprite = AddHeartIcon;
        celebrationText.text = $"You have been healed for {templeHealth} health!";
        celebrationScreen.SetActive(true);
        templeUI.SetActive(false);
    }

    public void OnSoulsButton()
    {

        GameManager.Instance.tData.collectedSouls += templeSouls;
        OnSoulCollect.Raise(new Empty());

        //celebrationImage.sprite = AddSoulsIcon;
        celebrationText.text = $"You have gained {templeSouls} souls!";
        celebrationScreen.SetActive(true);

        templeUI.SetActive(false);

    }

    public void OnSpellButton()
    {

        GetSpell();
        //Show player the spell with UI
        if (currentTempleSpell is SpecialSpellBook)
        {
            if (GameManager.Instance.tData.specialSpell != null)
            {
                currentSpellImage.sprite = GameManager.Instance.tData.specialSpell.spellIcon;
                newSpellImage.sprite = currentTempleSpell.spellIcon;

                confirmationSpellScreen.SetActive(true);
                templeUI.SetActive(false);
            }
            else
            {
                SetCelebrationScreen();
            }
        }
        else if (currentTempleSpell is UltimateSpellBook)
        {
            if (GameManager.Instance.tData.ultimateSpell != null)
            {
                currentSpellImage.sprite = GameManager.Instance.tData.ultimateSpell.spellIcon;
                newSpellImage.sprite = currentTempleSpell.spellIcon;

                confirmationSpellScreen.SetActive(true);
                templeUI.SetActive(false);
            }
            else
            {
                SetCelebrationScreen();
            }

        }

    }

    private void SetCelebrationScreen()
    {
        if (currentTempleSpell is SpecialSpellBook)
        {
            GameManager.Instance.tData.collectedSpells.Add(currentTempleSpell);
            GameManager.Instance.tData.specialSpell = (SpecialSpellBook)currentTempleSpell;
        }
        else
        {
            GameManager.Instance.tData.ultimateSpell = (UltimateSpellBook)currentTempleSpell;
        }
        celebrationImage.sprite = currentTempleSpell.spellIcon;
        celebrationText.text = $"{currentTempleSpell.spellData.templeDescription}";
        celebrationScreen.SetActive(true);
        templeUI.SetActive(false);
        OnPlayerPickSpell.Raise(new Empty());
    }

    public void OnAcceptSpellButton()
    {

        SetCelebrationScreen();

        confirmationSpellScreen.SetActive(false);
    }

    public void OnDenySpellButton()
    {
        Time.timeScale = 1.0f;
        confirmationSpellScreen.SetActive(false);
    }

    public void ChooseAction()
    {
        if (!keep)
        {
            OnAcceptSpellButton();
        }
        else
        {
            OnDenySpellButton();
        }

    }

    public void CloseCelebrationScreen()
    {
        celebrationScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

}

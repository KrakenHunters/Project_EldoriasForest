using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperManager : MonoBehaviour
{
    public PermanentDataContainer pData;
    public GameObject[] highlightAreas; // Areas to highlight (Base Spells, Special Spell Books, etc.)
    private int currentDialogueIndex = 0;
    private string[] currentDialogueArray;

    public ShopBase shopBase;
    public ShopSpecials shopSpecials;

    private bool fullTutorial = false;

    private int currentTutorialIndex = 0;

    [SerializeField]
    GameObject blockShop;

    [SerializeField]
    private int limitToWitch = 2;
    private int chatCounter;

    [SerializeField]
    private GameObject YesButton;
    [SerializeField]
    private GameObject NoButton;

    [SerializeField]
    private GameObject YesWitch;
    [SerializeField]
    private GameObject NoWitch;

    private string[] newGameTexts = new string[]
   {
        "Oh, hello there, magician!",
        "I'm Jolly, the shopkeeper of this village.",
        "Welcome to Eldoria, our charming magical village!",
        "Our village thrives with the presence of many animals around us.",
        "Our magic comes from the souls of these living forest animals.",
        "These souls are our primary source of energy, sustaining our magic and way of life.",
        "However, it's been difficult to sense the animals' souls since the witch appeared.",
        "You know about the witch, don't you?",
        "She resides in the forest, corrupting and feeding on animal souls.",
        "Her dark magic has cursed our land, draining its life and disrupting our lives.",
        "Many warriors have tried to defeat her, but none have succeeded.",
        "But there's something different about you...",
        "Legends speak of a magician destined to break this curse.",
        "Perhaps you are the one!",
        "If you bring me souls, I'll help you grow stronger to defeat her.",
        "Would you like me to explain how the shop works?"
   };

    private string[] witchStoryTexts = new string[]
    {
        "Long ago, she was a powerful magician who sought eternal life.",
        "She discovered a forbidden ritual that required the souls of living beings.",
        "Driven by her desire, she performed the ritual, transforming herself into a witch.",
        "Her newfound power came at a terrible cost: the corruption of her soul.",
        "Now, she roams the forest, stealing the souls of animals to maintain her dark magic.",
        "Her curse not only drains the life from the animals but also weakens our village.",
        "The once vibrant forest is now a place of fear and darkness.",
        "It's said that only a magician with a pure heart can break this curse.",
        "You might be the one to restore balance and save Eldoria! Who knows?",
        "If you are not maybe we can try hiring a knight or something..."
    };

    private string witchStoryConfirmation = "Would you like to learn more about the witch?";



    private string[] tutorialBaseSpellTexts = new string[]
    {
        "These are your base spells.",
        "You can choose one of the three, but you can't change a base spell inside the forest, only here in the village.",
        "If you bring me souls, I can upgrade them for you.",
        "Use the left mouse-click to attack with your base spell.",
        "Fire spells can ignite enemies, causing damage over time.",
        "Ice spells can freeze enemies, weakening them temporarily.",
        "Lightning spells can stun enemies, immobilizing them for a short duration."
    };

    private string[] tutorialSpecialSpellTexts = new string[]
    {
        "These are the special spells.",
        "In the forest, you'll find temples containing spell books.",
        "Once found, you can use these special spells during your run, but you'll lose them when you return to the village due to the witch's curse.",
        "However, you can buy them back here in the shop for a certain amount of souls.",
        "Use the right mouse-click to cast your special spell."
    };

    private string[] tutorialPermanentUpgradesTexts = new string[]
    {
        "These are some upgrades for yourself.",
        "The ultimate spell slot holds a powerful spell found deep in the forest.",
        "Use the middle mouse-click to cast it, but use it wisely, as it can only be cast once.",
        "Health upgrade increases your health to better face enemies!",
        "Defensive runes reduce the amount of damage you take from enemies!",
        "Soul Drop increases the number of souls dropped by enemies and gathered from temples.",
        "Cooldown reduction decreases the time it takes for you to use a special spell again."
    };

    private string[] endTutorialTexts = new string[]
    {
        "If you need me to explain something about the shop again, please hit the question mark buttons on the shop sections.",
        "While in the forest, find the purple portal whenever you need to return to the shop to upgrade yourself.",
        "The arrow on your screen will point to the closest one.",
        "Good luck, magician! We're counting on you to restore peace to Eldoria!"
    };


    private string[] introTexts = new string[]
    {
        "Oh, you again! How has the hunt been?",
        "Welcome back, magician! Found any souls?",
        "Good to see you! How's the quest?",
        "Ah, there you are! Any luck in the forest?",
        "Back so soon? How was the journey?",
        "Hello again! Any new discoveries?",
        "Welcome, friend! How fares the battle with the witch?",
        "You’ve returned! News from the forest?",
        "Hey there! Gathered any souls?"
    };

    private string[] chatTexts = new string[]
    {
        "Fire spells can ignite enemies, causing damage over time.",
        "Ice spells can freeze enemies, weakening them temporarily.",
        "Lightning spells can stun enemies, immobilizing them briefly.",
        "You know, if the screen darkens while in the forest, your health might be low.",
        "Be careful, enemies get stronger as you venture deeper into the forest, but the rewards also improve.",
        "Ultimate spells can only be used once, so use them wisely.",
        "Pigs are slower but hit harder.",
        "I've heard the witch is pretty strong and knows all spells from the temples in the forest.",
        "Do you like pineapple? My favorite fruit.",
        "The animals are fast, so keep moving to stay alive.",
        "Have you tried combining spells?",
        "I once saw a chicken cast a spell. No, really, it was trying to turn a worm into a dragonfly!",
        "I once tried to teach a troll ballet. Let's just say, we both ended up with sore toes.",
        "If you find a talking squirrel, please let me know. I've been looking for a good conversationalist!",
        "I've heard there were foxes, deer standing on two feet and squirrels that shot nuts in the forest, have you seen any? Maybe the witch killed them all..."
    };

    private string[] hoverPlayButton = new string[]
    {
        "May fortune favor you!",
        "Wishing you success and victory!",
        "Luck be on your side!",
        "Here's to a triumphant journey!",
        "May your skills shine bright!",
        "Go forth with courage and luck!",
        "Best of luck on your magical quest!",
        "May every spell cast bring you closer to triumph!",
        "Harness your magic and conquer!",
        "May the stars align in your favor!"
    };

    private string upgradeBaseSpell = "Oh yes! Upgrade your base spells to deal more damage and have a bigger chance of applying a status effect";
    private string ultimateSpellButton = "Oh ultimate spells! They are really strong and you can only get them on the far end of the forest. Remember they are a one-time use only!";
    private string healthUpgradeButton;
    private string defenseUpgrade;
    private string soulDropUpgrade;
    private string spellCooldownUpgrade;
    private string rerollText = "Reroll the special spells, but remember it only rerolls the ones you have acquired! Good luck!";

    private string baseSpell1Text;
    private string baseSpell2Text;
    private string baseSpell3Text;

    private string specialSpell1Text;
    private string specialSpell2Text;
    private string specialSpell3Text;


    private Typer typer;

    public void StartShopKeeperManager()
    {

        baseSpell1Text = shopBase.fireSpell.spellData.shopkeeperDescription;
        baseSpell2Text = shopBase.iceSpell.spellData.shopkeeperDescription;
        baseSpell3Text = shopBase.lightningSpell.spellData.shopkeeperDescription;
        if (shopSpecials.spell1 != null)
            specialSpell1Text = shopSpecials.spell1.spellData.shopkeeperDescription;
        if (shopSpecials.spell2 != null)
            specialSpell2Text = shopSpecials.spell2.spellData.shopkeeperDescription;
        if (shopSpecials.spell3 != null)
            specialSpell3Text = shopSpecials.spell3.spellData.shopkeeperDescription;


        healthUpgradeButton = $"So cheap! Increase your health by {pData.healthBonusIncrement} health points";
        defenseUpgrade = $"Enemies will deal {pData.runeIncrement * 100f}% less damage to you";
        soulDropUpgrade = $"Increase the soul drop from enemies and temples by {pData.templeSoulsDropRateIncrement * 100f}%";
        spellCooldownUpgrade = $"Reduce your special spells cooldowns by {pData.cooldownReductionIncrement * 100f}%";


        typer = GetComponent<Typer>();
        if (ShopManager.Instance.permData.tutorialDone)
        {
            StartCoroutine(FadeOut());
            IntroDialogue();
            ShopManager.Instance.ButtonsInteractability(true);
        }
        else
        {
            StartDialogue(newGameTexts);
            ShopManager.Instance.ButtonsInteractability(false);

        }
    }

    private void IntroDialogue()
    {
        typer.ShowText(introTexts[Random.Range(0, introTexts.Length)]);
    }

    public void ChatDialogue()
    {
        chatCounter++;
        if (chatCounter > limitToWitch)
        {
            typer.ShowText(witchStoryConfirmation);
            YesWitch.SetActive(true);
            NoWitch.SetActive(true);
            limitToWitch = 100000;
        }
        else
        {
            typer.ShowText(chatTexts[Random.Range(0, introTexts.Length)]);
        }

    }


    private void StartDialogue(string[] dialogue)
    {
        currentDialogueIndex = 0;
        currentDialogueArray = dialogue;
        ShowNextDialogue(dialogue);
    }


    private void ShowNextDialogue(string[] texts)
    {
        if (texts == newGameTexts && currentDialogueIndex == texts.Length - 1)
        {
            typer.ShowText(texts[currentDialogueIndex]);
            YesButton.SetActive(true);
            NoButton.SetActive(true);
        }
        else if (currentDialogueIndex < texts.Length)
        {
            typer.ShowText(texts[currentDialogueIndex]);

        }
        else
        {
            EndDialogue();

            if (currentTutorialIndex < 3 && fullTutorial)
            {
                currentTutorialIndex++;
                if (currentTutorialIndex == 1)
                {
                    SpecialSpellExplanation();
                }
                else if (currentTutorialIndex == 2)
                {
                    PermanentUpgradeExplanation();
                }
                else
                {
                    StartDialogue(endTutorialTexts);
                    ShopManager.Instance.ButtonsInteractability(false);

                    fullTutorial = false;

                }
            }
        }
    }

    public void ConfirmationButton()
    {
        fullTutorial = true;
        YesButton.SetActive(false);
        NoButton.SetActive(false);

        currentTutorialIndex = 0;
        ShopManager.Instance.permData.tutorialDone = true;
        BaseSpellExplanation();
    }

    public void DenyButton()
    {
        YesButton.SetActive(false);
        NoButton.SetActive(false);

        ShopManager.Instance.permData.tutorialDone = true;

        StartDialogue(endTutorialTexts);
    }

    public void WitchStoryConfirmationButton()
    {
        YesWitch.SetActive(false);
        NoWitch.SetActive(false);

        ShopManager.Instance.ButtonsInteractability(false);
        StartDialogue(witchStoryTexts);
    }

    public void WitchStoryDenyButton()
    {
        YesWitch.SetActive(false);
        NoWitch.SetActive(false);
        ShopManager.Instance.ButtonsInteractability(true);

        StartDialogue(endTutorialTexts);
    }


    public void BaseSpellExplanation()
    {
        highlightAreas[0].SetActive(true);
        StartDialogue(tutorialBaseSpellTexts);
        ShopManager.Instance.ButtonsInteractability(false);

    }

    public void SpecialSpellExplanation()
    {
        highlightAreas[1].SetActive(true);
        StartDialogue(tutorialSpecialSpellTexts);
        ShopManager.Instance.ButtonsInteractability(false);

    }

    public void PermanentUpgradeExplanation()
    {
        highlightAreas[2].SetActive(true);
        StartDialogue(tutorialPermanentUpgradesTexts);
        ShopManager.Instance.ButtonsInteractability(false);

    }


    public void PlayerInputReceived()
    {
        currentDialogueIndex++;
        ShowNextDialogue(currentDialogueArray); // Replace currentDialogueArray with appropriate array
    }

    void Update()
    {
        if (!typer.isTyping && Input.anyKeyDown && currentDialogueArray != null) // Check for input
        {
            PlayerInputReceived();
        }
    }



    void EndDialogue()
    {
        typer.ShowText("");
        ShopManager.Instance.ButtonsInteractability(true);
        if (blockShop.activeSelf)
            StartCoroutine(FadeOut());


        currentDialogueArray = null;
        foreach (var area in highlightAreas)
        {
            area.SetActive(false); // Deactivate all highlight areas
        }

    }

    private IEnumerator FadeOut()
    {
        float duration = 1.0f; // Adjust the duration to your preference
        float startAlpha = blockShop.GetComponent<CanvasGroup>().alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            blockShop.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, 0, time / duration);
            yield return null;
        }

        blockShop.GetComponent<CanvasGroup>().alpha = 0;
        blockShop.SetActive(false); // Finally, disable the GameObject
    }

    #region HoverFunctions
    public void BaseSpellUpgradeDialogue(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(upgradeBaseSpell);
        }
    }

    public void UltimateSpellUpgradeDialogue(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(ultimateSpellButton);
        }
    }

    public void HealthUpgradeDialogue(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(healthUpgradeButton);
        }
    }

    public void DefenseUpgradeDialogue(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(defenseUpgrade);
        }
    }

    public void SoulDropUpgradeDialogue(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(soulDropUpgrade);
        }
    }

    public void SpellCooldownUpgradeDialogue(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(spellCooldownUpgrade);
        }
    }

    public void PlayButton(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(hoverPlayButton[Random.Range(0, hoverPlayButton.Length)]);
        }
    }

    public void RerollButton(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(rerollText);
        }
    }

    public void BaseSpell1(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(baseSpell1Text);
        }
    }

    public void BaseSpell2(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(baseSpell2Text);
        }
    }

    public void BaseSpell3(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(baseSpell3Text);
        }
    }

    public void SpecialSpell1(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(specialSpell1Text);
        }
    }

    public void SpecialSpell2(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(specialSpell2Text);
        }
    }

    public void SpecialSpell3(bool buttonInteractable)
    {
        if (buttonInteractable)
        {
            typer.ShowText(specialSpell3Text);
        }
    }





    #endregion

}

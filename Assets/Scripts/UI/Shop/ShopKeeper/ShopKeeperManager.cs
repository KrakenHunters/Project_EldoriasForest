using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperManager : MonoBehaviour
{
    public GameObject[] highlightAreas; // Areas to highlight (Base Spells, Special Spell Books, etc.)
    private int currentDialogueIndex = 0;
    private string[] currentDialogueArray;

    private bool fullTutorial;

    private int currentTutorialIndex = 0;

    [SerializeField]
    private GameObject YesButton;
    [SerializeField]
    private GameObject NoButton;

    private string[] newGameTexts = new string[]
    {
        "Oh, hello there, magician!",
        "I'm Jolly, the shopkeeper of this village.",
        "Welcome to Eldoria, a magical village thriving on purified souls.",
        "We use a sacred ritual to extract and purify souls from forest animals.",
        "These purified souls are our primary source of energy, sustaining our magic and way of life.",
        "However, it's been difficult since the witch appeared.",
        "You know about the witch, don't you?",
        "She resides in the forest, corrupting and feeding on animal souls.",
        "Her dark magic has cursed our land, draining its life and disrupting our lives.",
        "Many warriors have tried to defeat her, but none have succeeded.",
        "But there's something different about you...",
        "Legends speak of a magician destined to break this curse.",
        "Perhaps you are the one!",
        "If you can bring me souls, and I'll help you grow stronger to defeat her.",
        "Would you like me to explain how the shop works?"
    };

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
        "While in the forest, find the purple portal whenever you need to return to the shop to upgrade yourself..",
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


    private Typer typer;

    void Awake()
    {
        typer = GetComponent<Typer>();
        if (ShopManager.Instance.permData.tutorialDone)
            IntroDialogue();
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
        typer.ShowText(chatTexts[Random.Range(0, introTexts.Length)]);
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
            ShopManager.Instance.ButtonsInteractability(false);

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
        ShopManager.Instance.ButtonsInteractability(true);

        StartDialogue(endTutorialTexts);
    }

    public void BaseSpellExplanation()
    {
        highlightAreas[0].SetActive(true);
        StartDialogue(tutorialBaseSpellTexts);
        
    }

    public void SpecialSpellExplanation()
    {
        highlightAreas[1].SetActive(true);
        StartDialogue(tutorialSpecialSpellTexts);
    }

    public void PermanentUpgradeExplanation()
    {
        highlightAreas[2].SetActive(true);
        StartDialogue(tutorialPermanentUpgradesTexts);
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
        if (currentDialogueArray == endTutorialTexts)
        {
            ShopManager.Instance.ButtonsInteractability(true);
        }

        currentDialogueArray = null;
        foreach (var area in highlightAreas)
        {
            area.SetActive(false); // Deactivate all highlight areas
        }

    }

}

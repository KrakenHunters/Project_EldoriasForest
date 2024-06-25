using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopKeeperManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject[] highlightAreas; // Areas to highlight (Base Spells, Special Spell Books, etc.)
    private int currentDialogueIndex = 0;
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
        "You can choose any of them, but you can't change a bse spell inside the forest, only here in the village.",
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
        "Good to see you! How goes the quest?",
        "Ah, there you are! Any luck in the forest?",
        "Back so soon? How was your journey?",
        "Hello again! Any new discoveries?",
        "Welcome, friend! How fares the battle with the witch?",
        "You’ve returned! News from the forest?",
        "Hey there! Gathered any souls on your travels?"
    };

    private string[] hintTexts = new string[]
    {
        "Fire spells can ignite enemies, causing damage over time.",
        "Ice spells can freeze enemies, weakening them temporarily.",
        "Lightning spells can stun enemies, immobilizing them briefly.",
        "If the screen darkens, your health might be low.",
        "Be careful, enemies get stronger as you venture deeper into the forest, but the rewards also improve.",
        "Ultimate spells can only be used once, so use them wisely.",
        "Pigs are slower but hit harder.",
        "The animals are fast, so keep moving to stay alive."
    };


    private Typer typer;

    void Start()
    {
        typer = GetComponent<Typer>();
        IntroDialogue();
    }

    public void IntroDialogue()
    {
        typer.ShowText(introTexts[Random.Range(0, introTexts.Length)]);
    }

    public void StartFullTutorial()
    {

    }

    public void NextDialogue(string[] texts)
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < texts.Length)
        {
            typer.ShowText(texts[currentDialogueIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    void HighlightArea(int index)
    {
        foreach (var area in highlightAreas)
        {
            area.SetActive(false); // Deactivate all highlight areas
        }
        if (index < highlightAreas.Length)
        {
            highlightAreas[index].SetActive(true); // Activate the current highlight area
        }
    }

    void EndDialogue()
    {
        typer.ShowText("");
        foreach (var area in highlightAreas)
        {
            area.SetActive(false); // Deactivate all highlight areas
        }
    }
}

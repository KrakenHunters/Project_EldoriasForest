using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    private int index;

    private string[] IntroTexts = new string[]
   {
        "In the mystical village of Eldoria, the enchanting souls of forest creatures breathe life into the land.",
        "But on one fateful day, a witch appeared, severing the connection between the animals and Eldoria.",
        "Driven by a dark hunger, the witch corrupted the souls of the forest creatures, gathering their essence and casting Eldoria into despair!",
        "Now the witch feeds on the forest, shaping it as she wishes and making life miserable for the people of Eldoria.",
        "Legend speaks of a magician capable of defeating the witch and bringing peace back to Eldoria... Maybe you?",
        "If not... we will just hire a knight or something."
   };

    [SerializeField]
    private Sprite[] images;

    [SerializeField]
    private Image sceneImage;

    private void Start()
    {
        index = 0;
        textComponent.text = IntroTexts[index];
        sceneImage.sprite = images[index];
    }
    public void Change()
    {
        index++;
        textComponent.text = IntroTexts[index];
        if (index <= IntroTexts.Length)
            sceneImage.sprite = images[index];

    }

    public void StartGame()
    {
        SceneManager.LoadScene("02_ForestScene");
    }

}
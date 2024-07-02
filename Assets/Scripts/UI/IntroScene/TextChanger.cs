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
        "In the mystical village Of EldOria, the enchanted sOuls Of fOrest creatures breathe life intO the land.",
        "But On One fateful day, a witch appeared, severing the cOnnectiOn between the animals and EldOria.",
        "Driven by a dark hunger, the witch cOrrupted the sOuls Of the fOrest creatures, gathering their essence and casting EldOria intO despair!",
        "NOw the witch feeds On the fOrest, shaping it as she wishes and making life miserable fOr the peOple Of EldOria.",
        "Legend speaks Of a magician capable Of defeating the witch and bringing peace back tO EldOria... Maybe yOu?",
        "If nOt... we will just hire a knight Or sOmething."
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
using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public void ChangeText(string newText)
    {
        textComponent.text = newText;
    }
}
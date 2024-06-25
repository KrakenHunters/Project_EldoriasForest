using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Typer : MonoBehaviour
{
    [SerializeField]
    private float typeSpeed = 0.05f;
    private float adjustTypeSpeed;
    [SerializeField]
    private TextMeshProUGUI textComponent;
    private string currentText = "";
    public bool isTyping = false;

    public void ShowText(string fullText)
    {
        currentText = fullText;
        adjustTypeSpeed = typeSpeed;

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = "";
        isTyping = true;
        foreach (char c in currentText.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(adjustTypeSpeed);
        }
        isTyping = false;
    }

    void Update()
    {
        if (isTyping && Input.anyKeyDown) // Check for mouse click
        {
            adjustTypeSpeed = 0.001f; // Increase typing speed
        }
    }
}

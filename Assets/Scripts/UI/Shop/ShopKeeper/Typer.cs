using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Typer : MonoBehaviour
{
    [SerializeField]
    private float typeSpeed = 0.05f;
    [SerializeField]
    private TextMeshProUGUI textComponent;
    private string currentText = "";
    private bool isTyping = false;

    public void ShowText(string fullText)
    {
        currentText = fullText;
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = "";
        isTyping = true;
        foreach (char c in currentText.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    void Update()
    {
        if (isTyping && Input.GetMouseButtonDown(0)) // Check for mouse click
        {
            typeSpeed = 0.001f; // Increase typing speed
        }
    }
}

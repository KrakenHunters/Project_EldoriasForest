using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public bool isTyping = true;

    private float timer;

    public void ShowText(string fullText)
    {
        currentText = fullText;
        adjustTypeSpeed = typeSpeed;
        isTyping = true;
        timer = 0f;
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = "";
        foreach (char c in currentText.ToCharArray())
        {
            textComponent.text += c;

            yield return new WaitForSeconds(adjustTypeSpeed);
        }
        isTyping = false;
    }

    void LateUpdate()
    {
        timer += Time.deltaTime;   
        if (isTyping && Input.anyKeyDown && timer >= 0.1f) // Check for mouse click
        {
            adjustTypeSpeed = 0.001f; // Increase typing speed
        }
    }
}

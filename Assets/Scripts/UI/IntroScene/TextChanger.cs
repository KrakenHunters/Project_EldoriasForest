using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void ChangeText(string newText)
    {
        textComponent.text = newText;
        
    }

    public void ChangeAudio(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
    }
}
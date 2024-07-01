using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI hintText;

    [SerializeField] private AudioClip DeathBGMusic;
    [SerializeField] private AudioClip[] DeathScreenBGMusic;
    [SerializeField] private MenuAudioEvent AudioEvent;

    private void Awake()
    {
        AudioEvent.StopAllAudio.Invoke();
        AudioEvent.PlayBGMusic.Invoke(DeathBGMusic);
        AudioEvent.ButtonClick.Invoke(DeathScreenBGMusic[Random.Range(0,DeathScreenBGMusic.Length)]);
        Time.timeScale = 0;
        if (GameManager.Instance.pData.tutorialDone)
        {
            hintText.enabled = true;
        }
        else
        {
            hintText.enabled = false;
        }
    }

    public void ReturnToBase()

    {
        Time.timeScale = 1f;
        SaveManager.Instance.ResetTemporaryData();
        if (GameManager.Instance.pData.tutorialDone)
        {
            SceneManager.LoadScene("01_Shop");
        }
        else
        {
            hintText.enabled = false;
            SceneManager.LoadScene("02_ForestScene");
        }
    }

    public void TeleporttoShop()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("01_Shop");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("00_MainMenu");
    }
}

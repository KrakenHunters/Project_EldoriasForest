using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI hintText;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    public void ReturnToBase()
    {
        Time.timeScale = 1f;
        SaveManager.Instance.ResetTemporaryData();
        if (GameManager.Instance.pData.tutorialDone)
        {
            hintText.enabled = true;
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

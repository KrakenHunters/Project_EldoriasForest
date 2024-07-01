using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{

    [SerializeField] private AudioClip winBGMusic;
    [SerializeField] private MenuAudioEvent AudioEvent;

    private void Awake()
    {
        AudioEvent.StopAllAudio.Invoke();
        AudioEvent.PlayBGMusic.Invoke(winBGMusic);
        Time.timeScale = 0;
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

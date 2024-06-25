using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    private bool _isPaused;

    private void Awake()
    {
        _isPaused = false;
        _pauseMenu.gameObject.SetActive(_isPaused);
    }

    public void OnTogglePauseMenu()
    {
        _isPaused = !_isPaused;
        _pauseMenu.gameObject.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1;
    }


    public void OnLoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("00_MainMenu");
    }

    public void OnReturnToShop()
    {

       Time.timeScale = 1f;
        SceneManager.LoadScene("01_Shop");
    }

    public void OnQuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}

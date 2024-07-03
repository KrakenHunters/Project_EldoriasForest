using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    public UnityEvent OnPause;
    public UnityEvent OnResume;
    private bool _isPaused;
    private bool wasPaused = false;
    private void Awake()
    {
        _isPaused = false;
        _pauseMenu.gameObject.SetActive(_isPaused);
    }

    public void OnTogglePauseMenu()
    {
        wasPaused = (Time.timeScale == 0f); 
        _isPaused = !_isPaused;
        _pauseMenu.gameObject.SetActive(_isPaused);

        if (_isPaused)
            Time.timeScale = 0;
        else if (!wasPaused)
            Time.timeScale = 1f;

        if (_isPaused)
            OnPause.Invoke();
        else
            OnResume.Invoke();
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

        SceneManager.LoadScene("00_MainMenu");
/*#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
*/    }

}

using UnityEngine;

public class PauseMenu : Menu
{
    private bool _isPaused;
     private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  //add and input manager to handle this later 
        {
            OnTogglePauseMenu();   
        }
    }
    public void OnTogglePauseMenu()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Time.timeScale = _isPaused ? 0 : 1;
       // DisableScreens();
       
    }

    public void OnLoadMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("00_MainMenu");
    }

}

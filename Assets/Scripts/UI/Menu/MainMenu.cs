using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField]
    private GameObject _popupMenu;

    [Header("Start Buttons")]
    [SerializeField]
    private Button _loadGame;
    [SerializeField]
    private Button _newGame;

    [SerializeField]
    private AudioClip menuClip;
    [SerializeField]
    private AudioClip buttonClip;

    [Header("Audio Souce")]
    [SerializeField, Tooltip("Audio Mixer form the Assets folder")]
    private AudioMixer _MasterAudioMixer;

    public MenuAudioEvent MenuEvent;
    
    private float _maxVolume = 1f;

    private void Start()
    {
        if (!SaveManager.Instance.HasSaveData())
            _loadGame.interactable = false;
        else
        SaveManager.Instance.LoadPermanentData();

        //Ui settings
        _startActive = true;
        DisableScreens();
        MenuEvent.PlayBGMusic.Invoke(menuClip);

    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           SaveManager.Instance.DeleteAllSaveData();
            Debug.Log("Delete Save Data");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
           SaveManager.Instance.SavePermanentData();
            Debug.Log("Save Game");
        }*/
    }
    public void OnTogglePopUpMenu() => _popupMenu.SetActive(!_popupMenu.activeSelf);

    protected override void DisableScreens()
    {
        base.DisableScreens();
        _popupMenu.SetActive(false);
    }

    public void OnLoadGame()
    {
        StartGame();
    }
    public void StartGame()
    {                                                                                      
        SaveManager.Instance.ResetTemporaryData();
        SceneManager.LoadScene("01_Shop");//change to the  level  name
    }
    public void OnNewGame()
    {
        if (_loadGame.interactable)
            OnTogglePopUpMenu();
        else
            StartNewGameData();
    }
    public void StartNewGameData()
    {
        SaveManager.Instance.ResetPermanentData();
        SaveManager.Instance.ResetTemporaryData();
        SceneManager.LoadScene("02_ForestScene");//change to the  level  name
    }

    public void OnButtonClick()
    {
        MenuEvent.ButtonClick.Invoke(buttonClip);
    }

    public void OnToggleMute()
    {
        OnButtonClick();
        _isMusted = !_isMusted;
        _MasterAudioMixer.SetFloat("Master", _isMusted ? Mathf.Log10(0.001f) * 20 : Mathf.Log10(_maxVolume) * 20);
    }
}

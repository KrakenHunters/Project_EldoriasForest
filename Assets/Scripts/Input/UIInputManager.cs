using UnityEngine;

public class UIInputManager : MonoBehaviour
{
    PlayerInput _action;
    PauseMenu _menu;

    void Awake()
    {
        _menu = GetComponent<PauseMenu>();
        _action = new PlayerInput();
    }
    private void OnEnable()
    {
        _action.UI.Pause.performed += (val) => _menu.OnTogglePauseMenu();
        _action.Enable();

    }

    private void OnDisable()
    {
        _action.UI.Pause.performed -= (val) => _menu.OnTogglePauseMenu();
        _action.Disable();
    }
}

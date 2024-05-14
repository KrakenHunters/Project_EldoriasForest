using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput _action;
    PlayerController _player;
    void Awake()
    {
        _player = GetComponent<PlayerController>();
        _action = new PlayerInput();
        Debug.Log("Awake iknput");
    }

    private void Update()
    {
        //Check if the player is using the keyboard or the gamepad
        /* var devices = InputSystem.devices;

         foreach (var device in devices)
         {
             // Check if the device is active
             if (device is Gamepad gamepad && gamepad.leftStick.ReadValue() != Vector2.zero)
             {
                 UIGameControlsManager.Instance.SetToGamePadUI();
             }
             else if (device is Keyboard keyboard && (keyboard.anyKey.isPressed || keyboard.anyKey.wasPressedThisFrame))
             {
                UIGameControlsManager.Instance.SetToKeyBoardUI();
             }
         }*/

    }
    private void OnEnable()
    {

        _action.Player.Move.performed += (val) => _player.HandleMove( val.ReadValue<Vector2>());
        _action.Player.BaseAttack.performed += (val) => _player.HandleBaseAttack();

        _action.Enable();
    }

    private void OnDisable()
    {
        _action.Player.Move.performed -= (val) => _player.HandleMove(val.ReadValue<Vector2>());
        _action.Player.BaseAttack.performed -= (val) => _player.HandleBaseAttack();

        _action.Disable();
    }

}

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput _action;
 void Awake()
    {
        _player = GetComponent<PlayerController>();
        _action = new PlayerInput();
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
     
            //action.Player.Move.performed += (val) => player.HandleMove();
           
       
        _action.Enable();
    }

    private void OnDisable()
    {
        action.Player.Move.performed -= (val) => player.HandleMove();
        _action.Disable();
    }

    private void WaitTimer()
    {
        _action.Disable();
        OnEnable();
    }

}

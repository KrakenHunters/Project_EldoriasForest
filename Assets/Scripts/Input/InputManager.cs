using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput _action;
    PlayerController _player;

    Vector2 _movement;
    public Vector2 Movement
    {
        get
        {
            return _movement;
        }
        private set
        {
            _movement = value;
        }
    }
    void Awake()
    {
        _player = GetComponent<PlayerController>();
        _action = new PlayerInput();
    }

    private void Update()
    {

    }
    private void OnEnable()
    {

        _action.Player.Move.performed += (val) => Movement = val.ReadValue<Vector2>();
        _action.Player.PointerMove.performed += (val) => _player.HandlePointerDirection(val.ReadValue<Vector2>());


        _action.Player.BaseAttack.performed += (val) => _player.HandleBaseAttack();
        _action.Player.BaseAttack.canceled += (val) => _player.HandleCancelBaseAttack();

        _action.Player.SpecialAttack.performed += (val) => _player.HandleSpecialAttack();
        _action.Player.UltimateAttack.performed += (val) => _player.HandleUltimateAttack();

        _action.Player.Interact.performed += (val) => _player.HandleInteract();
        _action.Player.Interact.canceled += (val) => _player.CancelInteract();

        _action.Enable();
    }

    private void OnDisable()
    {
        _action.Player.Move.performed -= (val) => Movement = val.ReadValue<Vector2>();

        _action.Player.PointerMove.performed -= (val) => _player.HandlePointerDirection(val.ReadValue<Vector2>());

        _action.Player.BaseAttack.performed -= (val) => _player.HandleBaseAttack();
        _action.Player.BaseAttack.canceled -= (val) => _player.HandleCancelBaseAttack();

        _action.Player.SpecialAttack.performed -= (val) => _player.HandleSpecialAttack();
        _action.Player.UltimateAttack.performed -= (val) => _player.HandleUltimateAttack();

        _action.Player.Interact.performed -= (val) => _player.HandleInteract();
        _action.Player.Interact.canceled -= (val) => _player.CancelInteract();



        _action.Disable();
    }

    public void EnableInput()
    {
        _action.Player.Move.performed += (val) => Movement = val.ReadValue<Vector2>();
        _action.Player.PointerMove.performed += (val) => _player.HandlePointerDirection(val.ReadValue<Vector2>());


        _action.Player.BaseAttack.performed += (val) => _player.HandleBaseAttack();
        _action.Player.BaseAttack.canceled += (val) => _player.HandleCancelBaseAttack();

        _action.Player.SpecialAttack.performed += (val) => _player.HandleSpecialAttack();
        _action.Player.UltimateAttack.performed += (val) => _player.HandleUltimateAttack();

        _action.Player.Interact.performed += (val) => _player.HandleInteract();
        _action.Player.Interact.canceled += (val) => _player.CancelInteract();
    }
    public void DisableInput()
    {
        _action.Player.Move.performed -= (val) => Movement = val.ReadValue<Vector2>();
        _action.Player.PointerMove.performed -= (val) => _player.HandlePointerDirection(val.ReadValue<Vector2>());


        _action.Player.BaseAttack.performed -= (val) => _player.HandleBaseAttack();
        _action.Player.BaseAttack.canceled -= (val) => _player.HandleCancelBaseAttack();

        _action.Player.SpecialAttack.performed -= (val) => _player.HandleSpecialAttack();
        _action.Player.UltimateAttack.performed -= (val) => _player.HandleUltimateAttack();

        _action.Player.Interact.performed -= (val) => _player.HandleInteract();
        _action.Player.Interact.canceled -= (val) => _player.CancelInteract();

    }

}

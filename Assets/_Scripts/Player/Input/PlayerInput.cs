using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private InputSystem_Actions _inputAction;
    private Vector2 _currentPos;

    public Vector2 JoystickValue => _currentPos;

    void OnEnable()
    {
        _inputAction = new InputSystem_Actions();
        _inputAction.Player.Enable();
    }

    private void Update()
    {
        _currentPos = _inputAction.Player.Move.ReadValue<Vector2>();
    }
}

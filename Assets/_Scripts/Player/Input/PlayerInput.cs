using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputSystem_Actions _inputAction;
    private Vector2 _currentPos;

    public Vector2 JoystickValue => _currentPos;

    private void OnEnable()
    {
        _inputAction = new InputSystem_Actions();
        _inputAction.Player.Enable();

        PlayerCollision.OnPlayerBuying += DisableInputMovement;
        GameplayUIManager.OnPlayerStopBuying += EnableInputMovement;
    }

    private void OnDisable()
    {
        _inputAction.Player.Disable();
        PlayerCollision.OnPlayerBuying -= DisableInputMovement;
        GameplayUIManager.OnPlayerStopBuying -= EnableInputMovement;
    }

    private void Update()
    {
        _currentPos = _inputAction.Player.Move.ReadValue<Vector2>();
    }

    private void DisableInputMovement()
    {
        _inputAction.Player.Move.Disable();
    }

    private void EnableInputMovement()
    {
        _inputAction.Player.Move.Enable();
    }
}

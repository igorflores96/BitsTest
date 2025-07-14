using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private float _movementSpeed = 3f;

    private void Update()
    {
        Move(_input.JoystickValue);
    }

    private void Move(Vector2 input)
    {
        if (input == Vector2.zero) return;
        Vector3 movement = new Vector3(input.x, 0.0f, input.y);
        Vector3 goTo = movement.normalized * _movementSpeed * Time.deltaTime;
        Vector3 directionToLook = transform.position + goTo;
        transform.LookAt(directionToLook);
        transform.position += goTo;
    }

}

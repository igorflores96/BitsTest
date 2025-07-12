using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private Transform _targetToFollow;

    void OnEnable()
    {
        if (_targetToFollow == null)
            Debug.LogWarning("Camera doesn't have target");
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(_targetToFollow.position.x + 3f, transform.position.y, _targetToFollow.position.z - 8f);
    }

}

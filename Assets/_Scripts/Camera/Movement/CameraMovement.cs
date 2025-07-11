using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private Transform _targetToFollow;
    [SerializeField] private float _zOffset = -10f;

    void OnEnable()
    {
        if (_targetToFollow == null)
            Debug.LogWarning("Camera doesn't have target");
    }

    private void LateUpdate()
    {
        transform.Translate(new Vector3(_targetToFollow.position.x, transform.position.y, _targetToFollow.position.z * _zOffset), Space.World);
    }
}

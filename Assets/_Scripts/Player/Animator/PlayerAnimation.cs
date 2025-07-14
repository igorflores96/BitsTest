using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerInput _playerInput;
    private float _punchLayerWeight = 0.0f;

    private readonly string _isIdleTrigger = "isIdle";
    private readonly string _isRunningTrigger = "isRunning";
    private readonly int _punchLayerIndex = 1;

    private void OnEnable()
    {
        PlayerPunch.OnPlayerPunch += SetPunchWeight;
    }

    private void OnDisable()
    {
        PlayerPunch.OnPlayerPunch -= SetPunchWeight;
    }


    private void Update()
    {
        _animator.SetLayerWeight(_punchLayerIndex, _punchLayerWeight);

        if (_playerInput.JoystickValue == Vector2.zero)
        {
            _animator.SetTrigger(_isIdleTrigger);
            return;
        }

        _animator.SetTrigger(_isRunningTrigger);
    }

    private void SetPunchWeight()
    {
        _punchLayerWeight = 1.0f;
    }

    public void CancelPunchWeight() //called on keyframe punch animation
    {
        _punchLayerWeight = 0.0f;
    }

}

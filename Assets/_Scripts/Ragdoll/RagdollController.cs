using System;
using System.Collections;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Collider _characterCollider;
    [SerializeField] private Transform _characterBody;
    [SerializeField] private Animator _characterAnimator;

    private Rigidbody[] _allRbs;
    private Transform _playerParent;
    bool _isRagdollState;

    public static event Action<Transform> OnPunched;

    private void OnEnable()
    {
        _playerParent = null;
        _allRbs = GetComponentsInChildren<Rigidbody>();

        InitialSetup();
    }

    private void InitialSetup()
    {
        _characterCollider.enabled = true;
        _isRagdollState = false;

        foreach (Rigidbody bone in _allRbs)
            bone.isKinematic = true;

        _characterAnimator.enabled = true;
    }

    private void UpdateRagdoll(bool isActive, bool shouldGoToPlayer)
    {
        _characterCollider.enabled = !isActive;
        _isRagdollState = isActive;

        foreach (Rigidbody bone in _allRbs)
            bone.isKinematic = !_isRagdollState;

        _characterAnimator.enabled = !_isRagdollState;

        GetPunched();

        if (shouldGoToPlayer)
            StartCoroutine(nameof(GoToPlayer));
        else if (isActive)
            StartCoroutine(nameof(GetUp));
    }

    private void GetPunched()
    {
        if (_playerParent == null) return; 

        Vector3 direction = transform.position - _playerParent.position;
        direction = direction.normalized;

        foreach (Rigidbody bone in _allRbs)
            bone.linearVelocity = direction * 4f + Vector3.up * 2f;
    }

    private IEnumerator GetUp()
    {
        yield return new WaitForSeconds(2f);
        _playerParent = null;
        InitialSetup();
    }

    private IEnumerator GoToPlayer()
    {
        yield return new WaitForSeconds(2f);

        foreach (Rigidbody bone in _allRbs)
            bone.isKinematic = true;

        _characterBody.localPosition = Vector3.zero;
        OnPunched(this.transform);

    }

    public void ActiveRagdoll(Transform player, bool shouldGoToPlayer)
    {
        _playerParent = player;
        UpdateRagdoll(true, shouldGoToPlayer);
    }
}

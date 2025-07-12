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

        UpdateRagdoll(false, false);
    }

    private void UpdateRagdoll(bool isActive, bool shouldGoToPlayer)
    {
        _characterCollider.enabled = !isActive;
        _isRagdollState = isActive;

        foreach (Rigidbody bone in _allRbs)
            bone.isKinematic = !_isRagdollState;

        _characterAnimator.enabled = !_isRagdollState;

        if (shouldGoToPlayer)
        {
            Vector3 direction = transform.position - _playerParent.position;
            direction = direction.normalized;

            foreach (Rigidbody bone in _allRbs)
                bone.linearVelocity = direction * 4f + Vector3.up * 2f;

            StartCoroutine(nameof(GoToPlayer));
        }
    }

    private IEnumerator GoToPlayer()
    {
        yield return new WaitForSeconds(2f);

        foreach (Rigidbody bone in _allRbs)
            bone.isKinematic = true;

        _characterBody.localPosition = Vector3.zero;
        OnPunched(this.transform);
    }

    public void ActiveRagdoll(Transform player)
    {
        _playerParent = player;
        UpdateRagdoll(true, true);
    }
}

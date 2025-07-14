using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private PlayerStatusManager _playerStatus;
    [SerializeField] private float _punchDistance = 2f;

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hit, _punchDistance))
        {
            if (hit.collider.gameObject.TryGetComponent(out RagdollController ragdoll))
                ragdoll.ActiveRagdoll(this.transform, shouldGoToPlayer: !_playerStatus.StackIsFull);
        }

    }
}

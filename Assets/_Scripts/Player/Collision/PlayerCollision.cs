using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private readonly string SellingTag = "SellingSpot";
    private bool _isSelling = false;
    private Vector3 _sellingSpotPos;

    public bool IsSelling => _isSelling;
    public Vector3 SellingSpotPosition => _sellingSpotPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(SellingTag))
        {
            _sellingSpotPos = other.transform.position;   
            StartCoroutine(nameof(AwaitToSell));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(SellingTag))
        {
            _sellingSpotPos = Vector3.zero;
            _isSelling = false;
        }
    }

    private IEnumerator AwaitToSell()
    {
        yield return new WaitForSeconds(2f);
        _isSelling = true;
    }
}

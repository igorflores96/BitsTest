using System;
using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private readonly string SellingTag = "SellingSpot";
    private Vector3 _sellingSpotPos;

    public static event Action<Vector3> OnPlayerSelling;

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
            _sellingSpotPos = Vector3.zero;
    }

    private IEnumerator AwaitToSell()
    {
        yield return new WaitForSeconds(2f);
        OnPlayerSelling(_sellingSpotPos);
    }
}

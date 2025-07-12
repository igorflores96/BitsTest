using System;
using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private readonly string SellingTag = "SellingSpot";
    private readonly string BuyingTag = "BuyingSpot";

    private Vector3 _sellingSpotPos;
    private Coroutine _sellingCrt;
    private Coroutine _buyingCrt;

    public static event Action<Vector3> OnPlayerSelling;
    public static event Action OnPlayerBuying;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(SellingTag))
        {
            _sellingSpotPos = other.transform.position;
            _sellingCrt = StartCoroutine(nameof(AwaitToSell));
        }

        if (other.transform.CompareTag(BuyingTag))
        {
            _buyingCrt = StartCoroutine(nameof(AwaitToBuy));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(SellingTag))
        {
            if (_sellingCrt != null) StopCoroutine(_sellingCrt);
            _sellingSpotPos = Vector3.zero;
        }

        if (other.transform.CompareTag(BuyingTag))
            if (_buyingCrt != null) StopCoroutine(_buyingCrt);
    }

    private IEnumerator AwaitToBuy()
    {
        yield return new WaitForSeconds(2f);
        OnPlayerBuying();
    }

    private IEnumerator AwaitToSell()
    {
        yield return new WaitForSeconds(2f);
        OnPlayerSelling(_sellingSpotPos);
    }
}

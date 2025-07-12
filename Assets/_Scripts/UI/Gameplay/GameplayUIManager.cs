using System;
using TMPro;
using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{

    [SerializeField] private GameObject _buyingCanvas;
    [SerializeField] private GameObject _stickCanvas;
    [SerializeField] private TextMeshProUGUI _moneyText;

    public static event Action OnPlayerStopBuying;

    private void OnEnable()
    {
        PlayerStatusManager.OnEnemySold += UpdateMoney;
        PlayerCollision.OnPlayerBuying += PlayerBuying;
    }

    private void OnDisable()
    {
        PlayerStatusManager.OnEnemySold -= UpdateMoney;
        PlayerCollision.OnPlayerBuying -= PlayerBuying;
    }

    private void PlayerBuying()
    {
        _stickCanvas.gameObject.SetActive(false);
        _buyingCanvas.gameObject.SetActive(true);
    }

    private void UpdateMoney(int currentAmount)
    {
        _moneyText.text = $"Money: {currentAmount}";
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{

    [SerializeField] private GameObject _buyingCanvas;
    [SerializeField] private GameObject _stickCanvas;
    [SerializeField] private Image _colorFeedback;
    [SerializeField] private SO_Colors _colorsToSell;
    [SerializeField] private TextMeshProUGUI _moneyTextGameplay;
    [SerializeField] private TextMeshProUGUI _moneyTextStore;
    [SerializeField] private Button _buyStack;
    [SerializeField] private Button _buyColor;
    [SerializeField] private Button _closeMenuBtn;
    [SerializeField] private Button _plusColorBtn;
    [SerializeField] private Button _minusColorBtn;

    public static event Action OnPlayerStopBuying;
    public static event Action OnPlayerBoughtStack;
    public static event Action<Color> OnPlayerBoughtColor;

    private int _colorIndex;
    private Color _colorChosen;

    private void Awake()
    {
        _colorIndex = 0;
        _colorFeedback.color = _colorsToSell.Colors[_colorIndex];
    }

    private void OnEnable()
    {
        PlayerStatusManager.OnEnemySold += UpdateMoney;
        PlayerCollision.OnPlayerBuying += PlayerBuying;
        _closeMenuBtn.onClick.AddListener(CloseBuyingMenu);
        _plusColorBtn.onClick.AddListener(PlusColor);
        _minusColorBtn.onClick.AddListener(MinusColor);
        _buyStack.onClick.AddListener(BuyStack);
        _buyColor.onClick.AddListener(BuyColor);

    }

    private void OnDisable()
    {
        PlayerStatusManager.OnEnemySold -= UpdateMoney;
        PlayerCollision.OnPlayerBuying -= PlayerBuying;
        _closeMenuBtn.onClick.RemoveListener(CloseBuyingMenu);
        _plusColorBtn.onClick.RemoveListener(PlusColor);
        _minusColorBtn.onClick.RemoveListener(MinusColor);
        _buyStack.onClick.RemoveListener(BuyStack);
        _buyColor.onClick.RemoveListener(BuyColor);

    }

    private void PlayerBuying()
    {
        _stickCanvas.gameObject.SetActive(false);
        _buyingCanvas.gameObject.SetActive(true);
    }

    private void UpdateMoney(int currentAmount)
    {
        _moneyTextGameplay.text = currentAmount.ToString();
        _moneyTextStore.text = currentAmount.ToString();
    }

    private void CloseBuyingMenu()
    {
        _buyingCanvas.gameObject.SetActive(false);
        OnPlayerStopBuying();
    }

    private void BuyStack()
    {
        OnPlayerBoughtStack();
    }

    private void BuyColor()
    {
        _colorChosen = _colorsToSell.Colors[_colorIndex];
        OnPlayerBoughtColor(_colorChosen);
    }

    private void PlusColor()
    {
        _colorIndex = (_colorIndex + 1) % _colorsToSell.Colors.Length;
        _colorFeedback.color = _colorsToSell.Colors[_colorIndex];
    }

    private void MinusColor()
    {
        _colorIndex = (_colorIndex - 1 + _colorsToSell.Colors.Length) % _colorsToSell.Colors.Length;
        _colorFeedback.color = _colorsToSell.Colors[_colorIndex];
    }
}

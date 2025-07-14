using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{

    [SerializeField] private SO_StoreValues _storeValues;
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
    public static event Action<int> OnPlayerBoughtStack;
    public static event Action<Color, int> OnPlayerBoughtColor;

    private int _colorIndex;
    private Color _colorChosen;

    private void Awake()
    {
        _colorIndex = 0;
        _colorFeedback.color = _colorsToSell.Colors[_colorIndex];

        _buyColor.interactable = false;
        _buyStack.interactable = false;
    }

    private void OnEnable()
    {
        PlayerStatusManager.OnPlayerUpdatedMoney += UpdateMoney;
        PlayerCollision.OnPlayerBuying += PlayerBuying;
        _closeMenuBtn.onClick.AddListener(CloseBuyingMenu);
        _plusColorBtn.onClick.AddListener(PlusColor);
        _minusColorBtn.onClick.AddListener(MinusColor);
        _buyStack.onClick.AddListener(BuyStack);
        _buyColor.onClick.AddListener(BuyColor);

    }

    private void OnDisable()
    {
        PlayerStatusManager.OnPlayerUpdatedMoney -= UpdateMoney;
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
        _buyingCanvas.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        LeanTween.scale(_buyingCanvas, Vector3.one, 0.1f).setEaseInBounce();
    }

    private void UpdateMoney(int currentAmount)
    {
        bool haveMoney = currentAmount > 0;
        _moneyTextGameplay.text = currentAmount.ToString();
        _moneyTextStore.text = currentAmount.ToString();


        CheckButtons(haveMoney);
    }

    private void CheckButtons(bool playerHaveMoney)
    {
        _buyColor.interactable = playerHaveMoney;
        _buyStack.interactable = playerHaveMoney;
    }

    private void CloseBuyingMenu()
    {
        _buyingCanvas.gameObject.SetActive(false);
        _stickCanvas.gameObject.SetActive(true);
        OnPlayerStopBuying();
    }

    private void BuyStack()
    {
        OnPlayerBoughtStack(_storeValues.AddStackSize);
        LeanTween.scale(_buyStack.gameObject, Vector3.one * 1.2f, 0.1f).setEaseOutSine().setOnComplete(() => LeanTween.scale(_buyStack.gameObject, Vector3.one, 0.1f).setEaseInSine());
    }

    private void BuyColor()
    {
        _colorChosen = _colorsToSell.Colors[_colorIndex];
        OnPlayerBoughtColor(_colorChosen, _storeValues.NewColor);
        LeanTween.scale(_buyColor.gameObject, Vector3.one * 1.2f, 0.1f).setEaseOutSine().setOnComplete(() => LeanTween.scale(_buyColor.gameObject, Vector3.one, 0.1f).setEaseInSine());
    }

    private void PlusColor()
    {
        _colorIndex = (_colorIndex + 1) % _colorsToSell.Colors.Length;
        _colorFeedback.color = _colorsToSell.Colors[_colorIndex];
        LeanTween.scale(_plusColorBtn.gameObject, Vector3.one * 1.2f, 0.1f).setEaseOutSine().setOnComplete(() => LeanTween.scale(_plusColorBtn.gameObject, Vector3.one, 0.1f).setEaseInSine());
    }

    private void MinusColor()
    {
        _colorIndex = (_colorIndex - 1 + _colorsToSell.Colors.Length) % _colorsToSell.Colors.Length;
        _colorFeedback.color = _colorsToSell.Colors[_colorIndex];
        LeanTween.scale(_minusColorBtn.gameObject, Vector3.one * 1.2f, 0.1f).setEaseOutSine().setOnComplete(() => LeanTween.scale(_minusColorBtn.gameObject, Vector3.one, 0.1f).setEaseInSine());

    }
}

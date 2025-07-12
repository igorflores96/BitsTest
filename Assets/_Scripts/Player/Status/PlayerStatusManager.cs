using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerCollision _collision;
    [SerializeField] private SO_PlayerStatus _initialStatus;
    [SerializeField] private Material _playerMaterial;

    private Stack<Transform> _playerStack;
    private PlayerStatus _currentStatus;

    public static event Action<int> OnEnemySold;

    private void Awake()
    {
        _currentStatus = new PlayerStatus(_initialStatus);
    }

    private void OnEnable()
    {
        _playerStack = new Stack<Transform>();
        PlayerCollision.OnPlayerSelling += Sell;
        RagdollController.OnPunched += AddToStack;
        GameplayUIManager.OnPlayerBoughtColor += ChangeColor;
        GameplayUIManager.OnPlayerBoughtStack += AddStackSize;
    }

    private void OnDisable()
    {
        PlayerCollision.OnPlayerSelling -= Sell;
        RagdollController.OnPunched -= AddToStack;
        GameplayUIManager.OnPlayerBoughtColor -= ChangeColor;
        GameplayUIManager.OnPlayerBoughtStack -= AddStackSize;

    }

    private void Update()
    {
        //Inertia();
    }

    private void Sell(Vector3 spotPosition)
    {
        if (_playerStack.Count == 0) return;

        Transform[] array = _playerStack.ToArray();
        _playerStack.Clear();

        for (int i = 0; i < array.Length; i++)
        {
            GameObject objToSell = array[i].gameObject;
            array[i].SetParent(null);

            LeanTween.delayedCall(i * 0.1f, () =>
            {
                LeanTween.move(objToSell, spotPosition, 0.1f).setEase(LeanTweenType.linear).setOnComplete(() =>
                {
                    _currentStatus.CurrentMoney++;
                    LeanTween.scale(objToSell, Vector3.zero, 0.1f).setEase(LeanTweenType.linear).setOnComplete(() => OnEnemySold(_currentStatus.CurrentMoney));
                });
            });
        }
    }

    private void Inertia()
    {
        if (_playerStack.Count == 0)
            return;

        Transform[] array = _playerStack.ToArray();
        Transform first = array[array.Length - 1];
        first.localPosition = Vector3.Lerp(first.localPosition, first.localPosition + new Vector3(_input.JoystickValue.x, 0.0f, _input.JoystickValue.y), Time.deltaTime * 5f);
    }

    public void AddToStack(Transform transform)
    {
        if (_playerStack.Count >= _currentStatus.MaxStack) return;
        
        _playerStack.Push(transform);
        transform.SetParent(this.transform);

        Transform[] array = _playerStack.ToArray();

        for (int i = array.Length - 1; i >= 0; i--)
        {
            array[i].localPosition = new Vector3(0.0f, (i + 3.0f) * 0.5f, -1.0f);
            array[i].localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }

    private void AddStackSize()
    {
        _currentStatus.MaxStack++;
    }

    private void ChangeColor(Color newColor)
    {
        _playerMaterial.color = newColor;
    }
}


public class PlayerStatus
{
    public int CurrentMoney;
    public int MaxStack;

    public PlayerStatus(SO_PlayerStatus initialStatus)
    {
        MaxStack = initialStatus.InitialMaxStack;
        CurrentMoney = initialStatus.InitialMoney;
    }
}

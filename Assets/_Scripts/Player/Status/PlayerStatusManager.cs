using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerCollision _collision;
    [SerializeField] private SO_PlayerStatus _initialStatus;
    [SerializeField] private Material _playerMaterial;

    private Stack<Transform> _playerStack;
    private PlayerStatus _currentStatus;

    public bool StackIsFull => _currentStatus.MaxStack == _playerStack.Count; 

    public static event Action<int> OnPlayerUpdatedMoney;

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
        Inertia();
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
                    LeanTween.scale(objToSell, Vector3.zero, 0.1f).setEase(LeanTweenType.linear).setOnComplete(() => OnPlayerUpdatedMoney(_currentStatus.CurrentMoney));
                });
            });
        }
    }

    private void Inertia()
    {
        if (_playerStack.Count == 0)
            return;

        Transform[] array = _playerStack.ToArray();
        Vector2 input = _input.JoystickValue;
        float targetX = 0.0f;
        float targetZ = 0.0f;

        for (int i = array.Length - 1; i >= 0; i--)
        {
            Vector3 localPos = array[i].localPosition;

            if (input.y == 0f && input.x != 0f) //if just use X axis
            {
                targetZ = input.x > 0f ? localPos.z - input.x * i : localPos.z + input.x * i;
                targetX = 0.0f; //X is already zero, here just to clarity
            }
            else if (input.y != 0f && input.x == 0f) // if just use Y axis
            {
                targetZ = input.y > 0f ? localPos.z - input.y * i : localPos.z + input.y * i;
                targetX = 0.0f;
            }
            else if (input == Vector2.zero)
            {
                targetZ = -1.0f;
                targetX = 0.0f;
            }
            else
            {
                targetZ = input.y > 0f ? localPos.z - input.y * i : localPos.z + input.y * i;
                targetX = input.y > 0f ? localPos.x - input.x * i : localPos.x + input.x * i; //change it depends of Y axis. 
            }

            targetX = Mathf.Clamp(targetX, -0.5f, 0.5f);
            targetZ = Mathf.Clamp(targetZ, -1.5f, 0.5f);
            array[i].localPosition = Vector3.Lerp(localPos, new Vector3(targetX, localPos.y, targetZ), Time.deltaTime * 1.5f);
        }
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

    private void AddStackSize(int newStackValue)
    {
        ChangeMoney(-newStackValue);
        _currentStatus.MaxStack++;
    }

    private void ChangeColor(Color newColor, int newColorValue)
    {
        ChangeMoney(-newColorValue);
        _playerMaterial.color = newColor;
    }

    private void ChangeMoney(int quantity)
    {
        _currentStatus.CurrentMoney += quantity;

        if (_currentStatus.CurrentMoney < 0)
            _currentStatus.CurrentMoney = 0;

        OnPlayerUpdatedMoney(_currentStatus.CurrentMoney);
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

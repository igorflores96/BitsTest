using System.Collections.Generic;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerCollision _collision;
    private Stack<Transform> _playerStack;

    void OnEnable()
    {
        _playerStack = new Stack<Transform>();
    }

    private void Update()
    {
        //Inertia();
        TryToSell();
    }

    private void TryToSell()
    {
        if (!_collision.IsSelling || _playerStack.Count == 0) return;

        Transform[] array = _playerStack.ToArray();

        for (int i = 0; i < array.Length; i++)
        {
            array[i].SetParent(null);
            array[i].transform.position = _collision.SellingSpotPosition;
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
        _playerStack.Push(transform);
        transform.SetParent(this.transform);

        Transform[] array = _playerStack.ToArray();

        for (int i = array.Length - 1; i >= 0; i--)
        {
            array[i].localPosition = new Vector3(0.0f, (i + 3.0f) * 0.5f, -1.0f);
            array[i].localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }
}

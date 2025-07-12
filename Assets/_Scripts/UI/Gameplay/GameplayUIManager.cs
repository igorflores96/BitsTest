using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{

    private void OnEnable()
    {
        PlayerStack.OnEnemySold += Test;
    }

    private void OnDisable()
    {
        PlayerStack.OnEnemySold -= Test;
    }

    private void Test()
    {
        Debug.Log("funcionando");
    }
}

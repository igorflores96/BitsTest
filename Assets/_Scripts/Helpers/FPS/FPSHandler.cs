using TMPro;
using UnityEngine;

public class FPSHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsText;

    void Update()
    {
        _fpsText.text = $"FPS: {1f / Time.deltaTime:F2}";
    }
}

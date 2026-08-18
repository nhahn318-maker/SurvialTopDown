using TMPro;
using UnityEngine;

public class WaveStartFeedback : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private Transform feedbackPoint;

    [Header("Floating Text")]
    [SerializeField] private GameObject waveStartPopupPrefab;
    [SerializeField, Min(0.01f)] private float popupLifetime;
    [SerializeField] private string waveTextFormat = "WAVE {0}";

    private void Awake()
    {
        if (waveManager == null || waveStartPopupPrefab == null)
        {
            Debug.LogError(
                "WaveStartFeedback requires all references.",
                this);

            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (waveManager != null)
        {
            waveManager.WaveStarted += PlayFeedback;
        }
    }

    private void OnDisable()
    {
        if (waveManager != null)
        {
            waveManager.WaveStarted -= PlayFeedback;
        }
    }

    private void PlayFeedback(int waveNumber)
    {
        Vector3 position = feedbackPoint != null
            ? feedbackPoint.position
            : transform.position;

        GameObject popup = Instantiate(
            waveStartPopupPrefab,
            position,
            Quaternion.identity);

        TMP_Text popupText = popup.GetComponentInChildren<TMP_Text>();

        if (popupText != null)
        {
            popupText.text = string.Format(waveTextFormat, waveNumber);
        }

        Destroy(popup, popupLifetime);
    }
}

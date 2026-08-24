using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopupPresenter : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Transform popupPoint;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField, Min(0.01f)] private float popupLifetime;

    [Header("Damage Merge")]
    [SerializeField, Min(0f)] private float popupMergeWindowSeconds = 0.1f;

    private Coroutine pendingPopupCoroutine;
    private float pendingDamage;

    private void Awake()
    {
        if (health == null || damagePopupPrefab == null)
        {
            Debug.LogError(
                "DamagePopupPresenter requires all references.",
                this);

            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.Damaged += ShowDamage;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.Damaged -= ShowDamage;
        }

        if (pendingPopupCoroutine != null)
        {
            StopCoroutine(pendingPopupCoroutine);
            pendingPopupCoroutine = null;

            ShowPopup(pendingDamage);
        }

        pendingDamage = 0f;
    }

    private void ShowDamage(float damage, bool playHitAnimation)
    {
        if (!playHitAnimation)
        {
            return;
        }

        pendingDamage += damage;

        if (pendingPopupCoroutine == null)
        {
            pendingPopupCoroutine = StartCoroutine(ShowMergedDamage());
        }
    }

    private IEnumerator ShowMergedDamage()
    {
        if (popupMergeWindowSeconds > 0f)
        {
            yield return new WaitForSeconds(popupMergeWindowSeconds);
        }

        float totalDamage = pendingDamage;
        pendingDamage = 0f;
        pendingPopupCoroutine = null;

        ShowPopup(totalDamage);
    }

    private void ShowPopup(float damage)
    {
        Vector3 position = popupPoint != null
            ? popupPoint.position
            : transform.position;

        GameObject popup = Instantiate(
            damagePopupPrefab,
            position,
            Quaternion.identity);

        TMP_Text popupText = popup.GetComponentInChildren<TMP_Text>();

        if (popupText != null)
        {
            popupText.text = damage.ToString("0.#");
        }

        Destroy(popup, popupLifetime);
    }
}

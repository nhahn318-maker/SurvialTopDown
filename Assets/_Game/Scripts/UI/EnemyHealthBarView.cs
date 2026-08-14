using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarView : MonoBehaviour {
    [SerializeField] private Health health;
    [SerializeField] private Image fillImage;

    private Camera mainCamera;

    private void Start()
    {
        if (health == null || fillImage == null)
        {
            Debug.LogError(
                "EnemyHealthBarView requires all references.",
                this);

            return;
        }

        mainCamera = Camera.main;

        health.HealthChanged += Refresh;

        Refresh(health.CurrentHealth, health.MaxHealth);
    }

    private void LateUpdate()
    {
        if (mainCamera == null)
            return;

        transform.rotation = Quaternion.LookRotation(
            mainCamera.transform.forward,
            mainCamera.transform.up);
    }

    private void OnDestroy()
    {
        if (health != null)
            health.HealthChanged -= Refresh;
    }

    private void Refresh(float currentHealth, float maxHealth)
    {
        fillImage.fillAmount = maxHealth > 0f
            ? currentHealth / maxHealth
            : 0f;
    }
}
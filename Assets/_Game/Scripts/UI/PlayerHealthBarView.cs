using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarView : MonoBehaviour {
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text healthText;

    private Health health;

    private void Start()
    {
        if (playerHealth == null ||
            fillImage == null ||
            healthText == null)
        {
            Debug.LogError(
                "PlayerHealthBarView requires all references.",
                this);

            return;
        }

        health = playerHealth.Health;

        if (health == null)
        {
            Debug.LogError(
                "PlayerHealthBarView could not find Health.",
                this);

            return;
        }

        health.HealthChanged += Refresh;

        Refresh(health.CurrentHealth, health.MaxHealth);
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

        healthText.text =
            $"{Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(maxHealth)}";
    }
}
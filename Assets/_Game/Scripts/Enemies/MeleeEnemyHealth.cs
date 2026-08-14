using UnityEngine;

[RequireComponent(typeof(Health))]
public class MeleeEnemyHealth : MonoBehaviour {
    [SerializeField] private MeleeEnemyStats enemyStats;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

        if (enemyStats == null)
        {
            Debug.LogError("MeleeEnemyHealth requires MeleeEnemyStats.", this);
            enabled = false;
            return;
        }

        health.Died += HandleDeath;
    }

    private void OnEnable()
    {
        if (enemyStats != null)
        {
            health.Initialize(
                enemyStats.MaxHealth,
                enemyStats.Armor);
        }
    }

    private void OnDestroy()
    {
        if (health != null)
            health.Died -= HandleDeath;
    }

    private void HandleDeath()
    {
        gameObject.SetActive(false);
    }
}
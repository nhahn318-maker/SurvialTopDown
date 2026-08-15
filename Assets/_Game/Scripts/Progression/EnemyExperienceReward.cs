using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyExperienceReward : MonoBehaviour {
    private Health health;
    private PlayerProgression playerProgression;
    private bool hasGrantedReward;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        hasGrantedReward = false;

        playerProgression = FindObjectOfType<PlayerProgression>();

        if (playerProgression == null)
        {
            Debug.LogError(
                "EnemyExperienceReward requires PlayerProgression.",
                this);

            return;
        }

        health.Died += HandleDeath;
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.Died -= HandleDeath;
        }
    }

    private void HandleDeath()
    {
        if (hasGrantedReward)
        {
            return;
        }

        hasGrantedReward = true;
        playerProgression.GainEnemyExperience();
    }
}
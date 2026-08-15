using System;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerProgression : MonoBehaviour {
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerProgressionStats progressionStats;

    private PlayerHealth playerHealth;
    private Health health;

    public int CurrentLevel { get; private set; } = 1;
    public int CurrentExperience { get; private set; }
    public float DamageMultiplier { get; private set; }

    public event Action<int, int> ExperienceChanged;
    public event Action<int> LevelChanged;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if (playerStats == null || progressionStats == null)
        {
            Debug.LogError(
                "PlayerProgression requires PlayerStats and PlayerProgressionStats.",
                this);

            enabled = false;
        }
    }

    private void Start()
    {
        health = playerHealth.Health;
        DamageMultiplier = playerStats.DamageMultiplier;

        ExperienceChanged?.Invoke(
            CurrentExperience,
            progressionStats.ExperiencePerLevel);

        LevelChanged?.Invoke(CurrentLevel);
    }

    public void GainEnemyExperience()
    {
        GainExperience(progressionStats.ExperiencePerEnemy);
    }

    public void GainExperience(int amount)
    {


        if (amount <= 0 || health == null)
        {
            return;
        }

        CurrentExperience += amount;

        while (CurrentExperience >= progressionStats.ExperiencePerLevel)
        {
            CurrentExperience -= progressionStats.ExperiencePerLevel;
            LevelUp();
        }

        ExperienceChanged?.Invoke(
            CurrentExperience,
            progressionStats.ExperiencePerLevel);
    }

    private void LevelUp()
    {
        CurrentLevel++;

        health.AddMaxHealth(
            progressionStats.MaxHealthPerLevel);

        health.RestoreHealth(
            progressionStats.CurrentHealthPerLevel);

        health.AddArmor(
            progressionStats.ArmorPerLevel);

        DamageMultiplier +=
            progressionStats.DamageMultiplierPerLevel;

        LevelChanged?.Invoke(CurrentLevel);
    }
}
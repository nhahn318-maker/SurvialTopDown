using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerProgressionStats",
    menuName = "Survival Top-down/Progression/Player Progression Stats")]
public class PlayerProgressionStats : ScriptableObject {
    [SerializeField, Min(0)] private int experiencePerEnemy;
    [SerializeField, Min(1)] private int experiencePerLevel;
    [SerializeField, Min(0f)] private float maxHealthPerLevel;
    [SerializeField, Min(0f)] private float currentHealthPerLevel;
    [SerializeField, Min(0f)] private float armorPerLevel;
    [SerializeField, Min(0f)] private float damageMultiplierPerLevel;

    public int ExperiencePerEnemy => experiencePerEnemy;
    public int ExperiencePerLevel => experiencePerLevel;
    public float MaxHealthPerLevel => maxHealthPerLevel;
    public float CurrentHealthPerLevel => currentHealthPerLevel;
    public float ArmorPerLevel => armorPerLevel;
    public float DamageMultiplierPerLevel => damageMultiplierPerLevel;
}
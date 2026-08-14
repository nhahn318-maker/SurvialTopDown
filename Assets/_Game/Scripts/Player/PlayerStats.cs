using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerStats",
    menuName = "Survival Top-down/Player Stats")]
public class PlayerStats : ScriptableObject {
    [Header("Base Stats")]
    [SerializeField, Min(1f)] private float maxHealth;
    [SerializeField, Min(0f)] private float moveSpeed;
    [SerializeField, Min(0f)] private float turnSpeedDegreesPerSecond;
    [SerializeField, Min(0f)] private float armor;
    [SerializeField, Min(0f)] private float damageMultiplier;

    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float TurnSpeedDegreesPerSecond => turnSpeedDegreesPerSecond;
    public float Armor => armor;
    public float DamageMultiplier => damageMultiplier;
}
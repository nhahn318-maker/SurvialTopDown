using UnityEngine;

[CreateAssetMenu(
    fileName = "MeleeEnemyStats",
    menuName = "Survival Top-down/Enemies/Melee Enemy Stats")]
public class MeleeEnemyStats : ScriptableObject {
    [SerializeField, Min(1f)] private float maxHealth;
    [SerializeField, Min(0f)] private float moveSpeed;
    [SerializeField, Min(0f)] private float attackBaseDamage;
    [SerializeField, Range(0f, 360f)] private float attackConeAngle;
    [SerializeField, Min(0f)] private float attackRange;
    [SerializeField, Min(0f)] private float recoverySeconds;
    [SerializeField, Min(0f)] private float armor;

    [Header("Facing")]
    [SerializeField, Min(0f)] private float turnSpeedDegreesPerSecond;

    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float AttackBaseDamage => attackBaseDamage;
    public float AttackConeAngle => attackConeAngle;
    public float AttackRange => attackRange;
    public float RecoverySeconds => recoverySeconds;
    public float Armor => armor;
    public float TurnSpeedDegreesPerSecond => turnSpeedDegreesPerSecond;
}

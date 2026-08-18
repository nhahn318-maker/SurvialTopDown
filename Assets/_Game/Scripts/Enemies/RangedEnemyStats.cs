using UnityEngine;

[CreateAssetMenu(
    fileName = "RangedEnemyStats",
    menuName = "Survival Top-down/Enemies/Ranged Enemy Stats")]
public class RangedEnemyStats : ScriptableObject {
    [Header("Base")]
    [SerializeField, Min(1f)] private float maxHealth;
    [SerializeField, Min(0f)] private float moveSpeed;
    [SerializeField, Min(0f)] private float armor;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float preferredDistance;
    [SerializeField, Min(0f)] private float chaseReengageDistance;

    [Header("Poison Projectile")]
    [SerializeField, Min(0f)] private float projectileSpeed;
    [SerializeField, Min(0f)] private float projectileRange;
    [SerializeField, Min(0f)] private float poisonDamagePerTick;
    [SerializeField, Min(1)] private int poisonTickCount;
    [SerializeField, Min(0f)] private float poisonDuration;
    [SerializeField, Min(0f)] private float attackCooldown;

    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float Armor => armor;
    public float PreferredDistance => preferredDistance;
    public float ChaseReengageDistance => chaseReengageDistance;
    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileRange => projectileRange;
    public float PoisonDamagePerTick => poisonDamagePerTick;
    public int PoisonTickCount => poisonTickCount;
    public float PoisonDuration => poisonDuration;
    public float AttackCooldown => attackCooldown;
}

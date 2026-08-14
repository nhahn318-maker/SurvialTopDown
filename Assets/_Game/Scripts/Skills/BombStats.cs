using UnityEngine;

[CreateAssetMenu(
    fileName = "BombStats",
    menuName = "Survival Top-down/Skills/Bomb Stats")]
public class BombStats : ScriptableObject {
    [SerializeField, Min(0f)] private float baseDamage;
    [SerializeField, Min(0f)] private float detonationDelay;
    [SerializeField, Min(0f)] private float explosionRadius;
    [SerializeField, Min(0f)] private float cooldownSeconds;

    public float BaseDamage => baseDamage;
    public float DetonationDelay => detonationDelay;
    public float ExplosionRadius => explosionRadius;
    public float CooldownSeconds => cooldownSeconds;
}
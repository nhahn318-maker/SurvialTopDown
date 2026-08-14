using UnityEngine;

[CreateAssetMenu(
    fileName = "DashStats",
    menuName = "Survival Top-down/Skills/Dash Stats")]
public class DashStats : ScriptableObject {
    [SerializeField, Min(0f)] private float dashDistance;
    [SerializeField, Min(0f)] private float dashDuration;
    [SerializeField, Min(0f)] private float explosionBaseDamage;
    [SerializeField, Min(0f)] private float explosionRadius;
    [SerializeField, Min(0f)] private float cooldownSeconds;

    public float DashDistance => dashDistance;
    public float DashDuration => dashDuration;
    public float ExplosionBaseDamage => explosionBaseDamage;
    public float ExplosionRadius => explosionRadius;
    public float CooldownSeconds => cooldownSeconds;
}
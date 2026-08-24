using UnityEngine;

[CreateAssetMenu(
    fileName = "ProjectileStats",
    menuName = "Survival Top-down/Skills/Projectile Stats")]
public class ProjectileStats : ScriptableObject {
    [SerializeField, Min(0f)] private float moveSpeed;
    [SerializeField, Min(0f)] private float maxTravelDistance;

    public float MoveSpeed => moveSpeed;
    public float MaxTravelDistance => maxTravelDistance;
}

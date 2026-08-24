using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerProjectileLauncher : MonoBehaviour {
    [SerializeField] private PlayerBasicAttack basicAttack;
    [SerializeField] private PlayerProgression playerProgression;
    [SerializeField] private ProjectilePool projectilePool;
    [SerializeField] private VfxPool impactVfxPool;
    [SerializeField] private Transform launchPoint;

    private void Awake()
    {
        if (basicAttack == null ||
            playerProgression == null ||
            projectilePool == null ||
            impactVfxPool == null ||
            launchPoint == null)
        {
            Debug.LogError(
                "PlayerProjectileLauncher requires all references.",
                this);

            enabled = false;
        }
    }

    private void OnEnable()
    {
        basicAttack.ProjectileRequested += LaunchProjectile;
    }

    private void OnDisable()
    {
        basicAttack.ProjectileRequested -= LaunchProjectile;
    }

    private void LaunchProjectile(float spreadAngle)
    {
        Vector3 direction = GetLaunchDirection(spreadAngle);
        float damage = CalculateProjectileDamage();

        LaunchFromPool(direction, damage);
    }

    private Vector3 GetLaunchDirection(float spreadAngle)
    {
        return Quaternion.AngleAxis(spreadAngle, Vector3.up) *
            transform.forward;
    }

    private float CalculateProjectileDamage()
    {
        return DamageCalculator.CalculateDamageDealt(
            basicAttack.ProjectileBaseDamage,
            playerProgression.DamageMultiplier);
    }

    private void LaunchFromPool(Vector3 direction, float damage)
    {
        GameObject projectileObject = projectilePool.Get(
            launchPoint.position,
            Quaternion.LookRotation(direction));

        PlayerProjectile projectile =
            projectileObject.GetComponent<PlayerProjectile>();

        projectile.Launch(
            projectilePool,
            direction,
            damage,
            impactVfxPool);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (projectilePool == null || launchPoint == null)
            return;

        ProjectileStats projectileStats = projectilePool.ProjectileStats;

        if (projectileStats == null)
            return;

        float[] spreadAngles = basicAttack.SpreadAngles;

        if (spreadAngles == null || spreadAngles.Length == 0)
            return;

        float minimumAngle = spreadAngles[0];
        float maximumAngle = spreadAngles[0];

        foreach (float angle in spreadAngles)
        {
            minimumAngle = Mathf.Min(minimumAngle, angle);
            maximumAngle = Mathf.Max(maximumAngle, angle);
        }

        Vector3 origin = transform.position + Vector3.up * 0.03f;
        Vector3 startDirection =
            Quaternion.AngleAxis(minimumAngle, Vector3.up) * transform.forward;

        Handles.color = new Color(1f, 0f, 0f, 0.16f);
        Handles.DrawSolidArc(
            origin,
            Vector3.up,
            startDirection,
            maximumAngle - minimumAngle,
            projectileStats.MaxTravelDistance);

        Handles.color = Color.red;
        Handles.DrawWireArc(
            origin,
            Vector3.up,
            startDirection,
            maximumAngle - minimumAngle,
            projectileStats.MaxTravelDistance);

        foreach (float angle in spreadAngles)
        {
            Vector3 direction =
                Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;

            Handles.DrawLine(
                origin,
                origin + direction * projectileStats.MaxTravelDistance);
        }
    }
#endif
}

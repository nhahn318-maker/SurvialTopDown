using UnityEngine;

public class PlayerProjectileLauncher : MonoBehaviour {
    [SerializeField] private PlayerBasicAttack basicAttack;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private ProjectilePool projectilePool;
    [SerializeField] private Transform launchPoint;

    private void Awake()
    {
        if (basicAttack == null ||
            playerStats == null ||
            projectilePool == null ||
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
        Vector3 direction =
            Quaternion.AngleAxis(spreadAngle, Vector3.up) *
            transform.forward;

        float finalDamage = DamageCalculator.CalculateDamageDealt(
            basicAttack.ProjectileBaseDamage,
            playerStats.DamageMultiplier);

        GameObject projectileObject = projectilePool.Get(
            launchPoint.position,
            Quaternion.LookRotation(direction));

        PlayerProjectile projectile =
            projectileObject.GetComponent<PlayerProjectile>();

        projectile.Launch(
            projectilePool,
            direction,
            finalDamage);
    }
}
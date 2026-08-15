using UnityEngine;

public class PlayerProjectileLauncher : MonoBehaviour {
    [SerializeField] private PlayerBasicAttack basicAttack;
    [SerializeField] private PlayerProgression playerProgression;
    [SerializeField] private ProjectilePool projectilePool;
    [SerializeField] private Transform launchPoint;

    private void Awake()
    {
        if (basicAttack == null ||
            playerProgression == null ||
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
            playerProgression.DamageMultiplier);

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

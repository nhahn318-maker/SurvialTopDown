using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private ProjectileStats projectileStats;
    [SerializeField] private LayerMask hitLayers;

    public ProjectileStats Stats => projectileStats;

    private Rigidbody projectileRigidbody;
    private ProjectilePool ownerPool;
    private VfxPool impactVfxPool;
    private Vector3 direction;
    private float travelledDistance;
    private float damage;
    private bool isLaunched;

    private void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isLaunched)
            return;

        MoveProjectile();
        ReleaseIfOutOfRange();
    }

    private void MoveProjectile()
    {
        float travelStep = GetTravelStep();

        projectileRigidbody.MovePosition(
            projectileRigidbody.position + direction * travelStep);

        travelledDistance += travelStep;
    }

    private float GetTravelStep()
    {
        return projectileStats.MoveSpeed * Time.fixedDeltaTime;
    }

    private void ReleaseIfOutOfRange()
    {
        if (travelledDistance >= projectileStats.MaxTravelDistance)
        {
            ReleaseToPool();
        }
    }

    public void Launch(
        ProjectilePool pool,
        Vector3 launchDirection,
        float projectileDamage,
        VfxPool impactPool)
    {
        ownerPool = pool;
        impactVfxPool = impactPool;
        direction = launchDirection.normalized;
        damage = projectileDamage;
        travelledDistance = 0f;
        isLaunched = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLaunched || !CanHit(other))
            return;

        if (!TryGetDamageable(other, out IDamageable damageable))
            return;

        HandleHit(other, damageable);
    }

    private bool CanHit(Collider other)
    {
        int otherLayerMask = 1 << other.gameObject.layer;

        return (hitLayers.value & otherLayerMask) != 0;
    }

    private static bool TryGetDamageable(
        Collider other,
        out IDamageable damageable)
    {
        damageable = other.GetComponentInParent<IDamageable>();
        return damageable != null;
    }

    private void HandleHit(Collider other, IDamageable damageable)
    {
        damageable.TakeDamage(damage);
        impactVfxPool?.Play(
            other.ClosestPoint(transform.position),
            Quaternion.identity);

        ReleaseToPool();
    }

    private void ReleaseToPool()
    {
        if (!isLaunched)
            return;

        isLaunched = false;
        ownerPool.Release(gameObject);
    }
}

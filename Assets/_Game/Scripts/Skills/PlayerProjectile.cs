using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerProjectile : MonoBehaviour {
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

        float travelStep =
            projectileStats.MoveSpeed * Time.fixedDeltaTime;

        projectileRigidbody.MovePosition(
            projectileRigidbody.position + direction * travelStep);

        travelledDistance += travelStep;

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

        if (!isLaunched)
            return;

        int otherLayerMask = 1 << other.gameObject.layer;

        if ((hitLayers.value & otherLayerMask) == 0)
            return;

        IDamageable damageable =
            other.GetComponentInParent<IDamageable>();

        if (damageable == null)
            return;

        damageable.TakeDamage(damage);
        impactVfxPool?.Play(other.ClosestPoint(transform.position), Quaternion.identity);
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

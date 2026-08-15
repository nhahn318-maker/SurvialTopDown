using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RangedEnemyProjectile : MonoBehaviour {
    [SerializeField] private LayerMask targetLayers;

    private Rigidbody projectileRigidbody;
    private RangedProjectilePool ownerPool;
    private Vector3 moveDirection;
    private float projectileSpeed;
    private float projectileRange;
    private float poisonDamagePerTick;
    private int poisonTickCount;
    private float poisonDuration;
    private float travelledDistance;
    private bool isLaunched;

    private void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isLaunched)
        {
            return;
        }

        float moveDistance = projectileSpeed * Time.fixedDeltaTime;

        projectileRigidbody.MovePosition(
            projectileRigidbody.position + moveDirection * moveDistance);

        travelledDistance += moveDistance;

        if (travelledDistance >= projectileRange)
        {
            ReturnToPool();
        }
    }

    public void Launch(
        RangedProjectilePool pool,
        Vector3 direction,
        float speed,
        float range,
        float damagePerTick,
        int tickCount,
        float duration)
    {
        ownerPool = pool;
        moveDirection = direction.normalized;
        projectileSpeed = speed;
        projectileRange = range;
        poisonDamagePerTick = damagePerTick;
        poisonTickCount = tickCount;
        poisonDuration = duration;
        travelledDistance = 0f;
        isLaunched = true;

        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLaunched)
        {
            return;
        }

        int otherLayerMask = 1 << other.gameObject.layer;

        if ((targetLayers.value & otherLayerMask) == 0)
        {
            return;
        }

        PoisonEffect poisonEffect =
            other.GetComponentInParent<PoisonEffect>();

        if (poisonEffect == null)
        {
            return;
        }

        poisonEffect.Apply(
            poisonDamagePerTick,
            poisonTickCount,
            poisonDuration);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (!isLaunched)
        {
            return;
        }

        isLaunched = false;
        ownerPool.Release(gameObject);
    }
}
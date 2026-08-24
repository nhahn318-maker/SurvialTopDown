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
            return;

        MoveProjectile();
        ReturnIfOutOfRange();
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
        InitializeLaunchData(
            pool,
            direction,
            speed,
            range,
            damagePerTick,
            tickCount,
            duration);

        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    private void InitializeLaunchData(
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
    }

    private void MoveProjectile()
    {
        float moveDistance = GetMoveDistance();

        projectileRigidbody.MovePosition(
            projectileRigidbody.position + moveDirection * moveDistance);

        travelledDistance += moveDistance;
    }

    private float GetMoveDistance()
    {
        return projectileSpeed * Time.fixedDeltaTime;
    }

    private void ReturnIfOutOfRange()
    {
        if (travelledDistance >= projectileRange)
            ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLaunched || !CanHit(other))
            return;

        if (TryApplyPoison(other))
            ReturnToPool();
    }

    private bool CanHit(Collider other)
    {
        int otherLayerMask = 1 << other.gameObject.layer;
        return (targetLayers.value & otherLayerMask) != 0;
    }

    private bool TryApplyPoison(Collider other)
    {
        PoisonEffect poisonEffect =
            other.GetComponentInParent<PoisonEffect>();

        if (poisonEffect == null)
            return false;

        poisonEffect.Apply(
            poisonDamagePerTick,
            poisonTickCount,
            poisonDuration);

        return true;
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

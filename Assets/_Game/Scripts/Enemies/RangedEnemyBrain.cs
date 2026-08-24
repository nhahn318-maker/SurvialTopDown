using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CharacterController))]
public class RangedEnemyBrain : MonoBehaviour {
    [SerializeField] private RangedEnemyStats enemyStats;
    [SerializeField] private Transform target;
    [SerializeField] private Transform projectileLaunchPoint;
    [SerializeField] private RangedProjectilePool projectilePool;

    private CharacterController characterController;
    private EnemyAnimationController animationController;
    private float nextAttackTime;
    private float recoveryEndTime;
    private bool isInAttackPosition;
    private bool isRecovering;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController =
            GetComponentInChildren<EnemyAnimationController>(true);

        if (enemyStats == null || projectileLaunchPoint == null)
        {
            Debug.LogError(
                "RangedEnemyBrain requires stats and launch point.",
                this);

            enabled = false;
        }
    }

    private void OnEnable()
    {
        nextAttackTime = 0f;
        recoveryEndTime = 0f;
        isInAttackPosition = false;
        isRecovering = false;

        if (projectilePool == null)
        {
            projectilePool = FindObjectOfType<RangedProjectilePool>();

            if (projectilePool == null)
            {
                Debug.LogError(
                    "RangedEnemyBrain requires RangedProjectilePool.",
                    this);
                return;
            }
        }

        if (target == null)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

            if (playerHealth != null)
            {
                target = playerHealth.transform;
            }
        }
    }

    private void Update()
    {
        if (target == null ||
            enemyStats == null ||
            projectilePool == null)
        {
            animationController?.SetMovementSpeed(0f);
            return;
        }

        if (isRecovering)
        {
            animationController?.SetMovementSpeed(0f);

            if (Time.time >= recoveryEndTime)
                isRecovering = false;

            return;
        }

        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0f;

        float targetDistance = directionToTarget.magnitude;

        UpdateAttackPosition(targetDistance);

        if (!isInAttackPosition)
        {
            Chase(directionToTarget);
            return;
        }

        animationController?.SetMovementSpeed(0f);
        TryAttack(directionToTarget);
    }

    private void UpdateAttackPosition(float targetDistance)
    {
        if (!isInAttackPosition &&
            targetDistance <= enemyStats.PreferredDistance)
        {
            isInAttackPosition = true;
            return;
        }

        if (isInAttackPosition &&
            targetDistance > enemyStats.ChaseReengageDistance)
        {
            isInAttackPosition = false;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Chase(Vector3 directionToTarget)
    {
        if (directionToTarget.sqrMagnitude <= Mathf.Epsilon)
        {
            return;
        }

        Vector3 moveDirection = directionToTarget.normalized;

        RotateTowards(moveDirection);

        characterController.Move(
            moveDirection *
            enemyStats.MoveSpeed *
            Time.deltaTime);

        animationController?.SetMovementSpeed(enemyStats.MoveSpeed);
    }

    private void TryAttack(Vector3 directionToTarget)
    {
        if (Time.time < nextAttackTime ||
            directionToTarget.sqrMagnitude <= Mathf.Epsilon)
        {
            return;
        }

        float angleToTarget = Vector3.Angle(
            transform.forward,
            directionToTarget);

        if (angleToTarget > enemyStats.AttackAimAngle)
        {
            RotateTowards(directionToTarget);
            return;
        }

        Vector3 attackDirection = transform.forward;

        GameObject projectileObject = projectilePool.Get(
            projectileLaunchPoint.position,
            Quaternion.LookRotation(attackDirection));

        RangedEnemyProjectile projectile =
            projectileObject.GetComponent<RangedEnemyProjectile>();

        projectile.Launch(
            projectilePool,
            attackDirection,
            enemyStats.ProjectileSpeed,
            enemyStats.ProjectileRange,
            enemyStats.PoisonDamagePerTick,
            enemyStats.PoisonTickCount,
            enemyStats.PoisonDuration);

        animationController?.TriggerShoot();
        nextAttackTime = Time.time + enemyStats.AttackCooldown;
        recoveryEndTime = nextAttackTime;
        isRecovering = true;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude <= Mathf.Epsilon)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            enemyStats.TurnSpeedDegreesPerSecond * Time.deltaTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (enemyStats == null)
            return;

        Handles.color = new Color(1f, 0f, 0f, 0.18f);

        Handles.DrawSolidDisc(
            transform.position,
            Vector3.up,
            enemyStats.PreferredDistance);

        Handles.color = Color.red;

        Handles.DrawWireDisc(
            transform.position,
            Vector3.up,
            enemyStats.PreferredDistance);
    }
#endif
}

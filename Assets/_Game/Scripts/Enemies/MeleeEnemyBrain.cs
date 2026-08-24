using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CharacterController))]
public class MeleeEnemyBrain : MonoBehaviour {
    [SerializeField] private MeleeEnemyStats enemyStats;
    [SerializeField] private Transform target;
    [SerializeField] private EnemyAnimationController animationController;

    private CharacterController characterController;
    private float recoveryEndTime;
    private bool isRecovering;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (enemyStats == null)
        {
            Debug.LogError("MeleeEnemyBrain requires MeleeEnemyStats.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        recoveryEndTime = 0f;
        isRecovering = false;

        if (target == null)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

            if (playerHealth != null)
                target = playerHealth.transform;
        }
    }

    private void Update()
    {
        if (target == null || enemyStats == null)
        {
            StopMovementAnimation();
            return;
        }

        if (HandleRecovery())
            return;

        Vector3 directionToTarget = GetDirectionToTarget();
        float targetDistance = directionToTarget.magnitude;

        if (targetDistance > enemyStats.AttackRange)
        {
            Chase(directionToTarget);
            return;
        }

        StopMovementAnimation();
        TryAttack(directionToTarget);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private bool HandleRecovery()
    {
        if (!isRecovering)
            return false;

        StopMovementAnimation();

        if (Time.time >= recoveryEndTime)
            isRecovering = false;

        return true;
    }

    private Vector3 GetDirectionToTarget()
    {
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0f;
        return directionToTarget;
    }

    private void Chase(Vector3 directionToTarget)
    {
        if (directionToTarget.sqrMagnitude <= Mathf.Epsilon)
            return;

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
        if (directionToTarget.sqrMagnitude <= Mathf.Epsilon)
            return;

        if (!IsTargetInAttackCone(directionToTarget))
        {
            RotateTowards(directionToTarget);
            return;
        }

        IDamageable damageable =
            target.GetComponent<IDamageable>();

        if (damageable == null)
            return;

        animationController?.TriggerAttack();
        damageable.TakeDamage(enemyStats.AttackBaseDamage);

        isRecovering = true;
        recoveryEndTime = Time.time + enemyStats.RecoverySeconds;
    }

    private bool IsTargetInAttackCone(Vector3 directionToTarget)
    {
        float angleToTarget = Vector3.Angle(
            transform.forward,
            directionToTarget);

        float halfConeAngle = enemyStats.AttackConeAngle * 0.5f;
        return angleToTarget <= halfConeAngle;
    }

    private void StopMovementAnimation()
    {
        animationController?.SetMovementSpeed(0f);
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return;

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

        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float halfConeAngle = enemyStats.AttackConeAngle * 0.5f;

        Vector3 leftDirection =
            Quaternion.Euler(0f, -halfConeAngle, 0f) * transform.forward;

        Vector3 rightDirection =
            Quaternion.Euler(0f, halfConeAngle, 0f) * transform.forward;

        Handles.color = new Color(1f, 0f, 0f, 0.18f);

        Handles.DrawSolidArc(
            origin,
            Vector3.up,
            leftDirection,
            enemyStats.AttackConeAngle,
            enemyStats.AttackRange);

        Handles.color = Color.red;

        Handles.DrawWireArc(
            origin,
            Vector3.up,
            leftDirection,
            enemyStats.AttackConeAngle,
            enemyStats.AttackRange);

        Handles.DrawLine(
            origin,
            origin + leftDirection * enemyStats.AttackRange);

        Handles.DrawLine(
            origin,
            origin + rightDirection * enemyStats.AttackRange);
    }
#endif
}

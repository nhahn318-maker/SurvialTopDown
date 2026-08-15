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

        if (targetDistance > enemyStats.AttackRange)
        {
            Chase(directionToTarget);
            return;
        }

        animationController?.SetMovementSpeed(0f);
        TryAttack(directionToTarget);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Chase(Vector3 directionToTarget)
    {
        if (directionToTarget.sqrMagnitude <= Mathf.Epsilon)
            return;

        Vector3 moveDirection = directionToTarget.normalized;

        transform.rotation = Quaternion.LookRotation(moveDirection);

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

        Vector3 attackDirection = directionToTarget.normalized;

        transform.rotation = Quaternion.LookRotation(attackDirection);

        float angleToTarget = Vector3.Angle(
            transform.forward,
            attackDirection);

        float halfConeAngle = enemyStats.AttackConeAngle * 0.5f;

        if (angleToTarget > halfConeAngle)
            return;

        IDamageable damageable =
            target.GetComponent<IDamageable>();

        if (damageable == null)
            return;

        animationController?.TriggerAttack();
        damageable.TakeDamage(enemyStats.AttackBaseDamage);

        isRecovering = true;
        recoveryEndTime = Time.time + enemyStats.RecoverySeconds;
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

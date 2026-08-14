using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MeleeEnemyBrain : MonoBehaviour {
    [SerializeField] private MeleeEnemyStats enemyStats;
    [SerializeField] private Transform target;

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
            return;

        if (isRecovering)
        {
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

        damageable.TakeDamage(enemyStats.AttackBaseDamage);

        isRecovering = true;
        recoveryEndTime = Time.time + enemyStats.RecoverySeconds;
    }
}

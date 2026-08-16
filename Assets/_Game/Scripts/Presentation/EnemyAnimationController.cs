using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private Health health;

    private void Awake()
    {
        health = GetComponentInParent<Health>();

        if (animator == null || health == null)
        {
            Debug.LogError("EnemyAnimationController requires an Animator and parent Health.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (health != null)
            health.Damaged += HandleDamaged;
    }

    private void OnDisable()
    {
        if (health != null)
            health.Damaged -= HandleDamaged;
    }

    public void SetMovementSpeed(float speed)
    {
        if (animator != null)
            animator.SetFloat(SpeedHash, speed);
    }

    public void TriggerAttack()
    {
        if (animator != null)
            animator.SetTrigger(AttackHash);
    }

    public void TriggerShoot()
    {
        if (animator != null)
            animator.SetTrigger(ShootHash);
    }

    public void TriggerDeath()
    {
        if (animator != null)
            animator.SetTrigger(DieHash);
    }

    private void HandleDamaged(float damage, bool playHitAnimation)
    {
        if (playHitAnimation)
            animator.SetTrigger(HitHash);
    }
}

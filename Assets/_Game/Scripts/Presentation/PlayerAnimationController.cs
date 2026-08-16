using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerBasicAttack basicAttack;
    [SerializeField] private PlayerDashSkill dashSkill;
    [SerializeField] private PlayerBombSkill bombSkill;

    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int ShootHash =
        Animator.StringToHash("Shoot");

    private static readonly int DashHash =
        Animator.StringToHash("Dash");

    private static readonly int BombHash =
        Animator.StringToHash("Bomb");

    private static readonly int HitHash =
        Animator.StringToHash("Hit");

    private static readonly int DieHash =
        Animator.StringToHash("Die");

    private Health health;
    private bool isDead;

    private void Awake()
    {
        health = GetComponent<Health>();

        if (animator == null ||
            playerMovement == null ||
            basicAttack == null ||
            dashSkill == null ||
            bombSkill == null ||
            health == null)
        {
            Debug.LogError(
                "PlayerAnimationController requires all references.",
                this);

            enabled = false;
        }
    }

    private void OnEnable()
    {
        isDead = health != null && health.IsDead;

        if (basicAttack != null)
        {
            basicAttack.Fired += TriggerShoot;
        }

        if (dashSkill != null)
        {
            dashSkill.DashStarted += TriggerDash;
        }

        if (bombSkill != null)
        {
            bombSkill.BombPlaced += TriggerBomb;
        }

        if (health != null)
        {
            health.Damaged += TriggerHit;
            health.Died += TriggerDeath;
        }
    }

    private void OnDisable()
    {
        if (basicAttack != null)
        {
            basicAttack.Fired -= TriggerShoot;
        }

        if (dashSkill != null)
        {
            dashSkill.DashStarted -= TriggerDash;
        }

        if (bombSkill != null)
        {
            bombSkill.BombPlaced -= TriggerBomb;
        }

        if (health != null)
        {
            health.Damaged -= TriggerHit;
            health.Died -= TriggerDeath;
        }
    }

    private void Update()
    {
        if (isDead)
            return;

        animator.SetFloat(
            SpeedHash,
            playerMovement.MovementInput.magnitude);
    }

    public void TriggerShoot()
    {
        if (isDead)
            return;

        animator.SetTrigger(ShootHash);
    }

    public void TriggerDash()
    {
        if (isDead)
            return;

        animator.SetTrigger(DashHash);
    }

    public void TriggerBomb()
    {
        if (isDead)
            return;

        animator.SetTrigger(BombHash);
    }

    private void TriggerHit(float damage, bool playHitAnimation)
    {
        if (!isDead && playHitAnimation)
            animator.SetTrigger(HitHash);
    }

    private void TriggerDeath()
    {
        isDead = true;
        animator.SetTrigger(DieHash);
    }
}

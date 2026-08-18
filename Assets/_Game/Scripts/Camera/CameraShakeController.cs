using UnityEngine;

[DefaultExecutionOrder(100)]
public class CameraShakeController : MonoBehaviour
{
    [SerializeField] private CameraShakeSettings shakeSettings;
    [SerializeField] private PlayerBasicAttack basicAttack;
    [SerializeField] private PlayerDashSkill dashSkill;
    [SerializeField] private PlayerHealth playerHealth;

    private Health health;
    private float shakeEndTime;
    private float shakeDuration;
    private float shakeMagnitude;

    private void Awake()
    {
        health = playerHealth != null
            ? playerHealth.GetComponent<Health>()
            : null;

        if (shakeSettings == null ||
            basicAttack == null ||
            dashSkill == null ||
            health == null)
        {
            Debug.LogError(
                "CameraShakeController requires all references.",
                this);

            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (basicAttack != null)
            basicAttack.Fired += HandleShot;

        if (dashSkill != null)
            dashSkill.DashExploded += HandleDashExplosion;

        if (health != null)
            health.Damaged += HandlePlayerDamaged;
    }

    private void OnDisable()
    {
        if (basicAttack != null)
            basicAttack.Fired -= HandleShot;

        if (dashSkill != null)
            dashSkill.DashExploded -= HandleDashExplosion;

        if (health != null)
            health.Damaged -= HandlePlayerDamaged;
    }

    private void LateUpdate()
    {
        float remainingTime = shakeEndTime - Time.time;

        if (remainingTime <= 0f || shakeDuration <= 0f)
            return;

        float strength = shakeMagnitude * (remainingTime / shakeDuration);
        Vector2 offset = Random.insideUnitCircle * strength;

        transform.position +=
            transform.right * offset.x +
            transform.up * offset.y;
    }

    private void HandleShot()
    {
        Shake(shakeSettings.Shot);
    }

    private void HandleDashExplosion()
    {
        Shake(shakeSettings.DashExplosion);
    }

    private void HandlePlayerDamaged(
        float damage,
        bool playHitAnimation)
    {
        if (playHitAnimation)
            Shake(shakeSettings.PlayerDamage);
    }

    private void Shake(CameraShakeProfile profile)
    {
        if (profile.Duration <= 0f || profile.Magnitude <= 0f)
            return;

        shakeDuration = profile.Duration;
        shakeMagnitude = profile.Magnitude;
        shakeEndTime = Time.time + shakeDuration;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerDashSkill : MonoBehaviour {
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerProgression playerProgression;
    [SerializeField] private DashStats dashStats;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField, Min(1)] private int maxHitColliders;

    public float RemainingCooldown =>
        Mathf.Max(0f, nextAvailableTime - Time.time);

    private CharacterController characterController;
    private Collider[] overlapBuffer;
    private readonly HashSet<IDamageable> damagedTargets = new();

    private float nextAvailableTime;
    private Coroutine dashCoroutine;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        overlapBuffer = new Collider[maxHitColliders];
    }

    private void OnDisable()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
            playerMovement.SetInputEnabled(true);
        }
    }

    public void TryDash()
    {
        if (dashCoroutine != null ||
            Time.time < nextAvailableTime ||
            dashStats.DashDuration <= 0f)
        {
            return;
        }

        nextAvailableTime = Time.time + dashStats.CooldownSeconds;
        dashCoroutine = StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        playerMovement.SetInputEnabled(false);

        Vector3 dashDirection = transform.forward;
        float elapsedTime = 0f;

        while (elapsedTime < dashStats.DashDuration)
        {
            float frameDuration = Mathf.Min(
                Time.deltaTime,
                dashStats.DashDuration - elapsedTime);

            float dashStep =
                dashStats.DashDistance *
                (frameDuration / dashStats.DashDuration);

            characterController.Move(dashDirection * dashStep);

            elapsedTime += frameDuration;
            yield return null;
        }

        playerMovement.SetInputEnabled(true);
        Explode();

        dashCoroutine = null;
    }

    private void Explode()
    {
        float finalDamage = DamageCalculator.CalculateDamageDealt(
            dashStats.ExplosionBaseDamage,
            playerProgression.DamageMultiplier);

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            dashStats.ExplosionRadius,
            overlapBuffer,
            enemyLayers,
            QueryTriggerInteraction.Collide);

        damagedTargets.Clear();

        for (int index = 0; index < hitCount; index++)
        {
            IDamageable damageable =
                overlapBuffer[index].GetComponentInParent<IDamageable>();

            if (damageable != null && damagedTargets.Add(damageable))
            {
                damageable.TakeDamage(finalDamage);
            }
        }
    }
}

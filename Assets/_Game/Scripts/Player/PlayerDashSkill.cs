using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CharacterController))]
public class PlayerDashSkill : MonoBehaviour {
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerProgression playerProgression;
    [SerializeField] private DashStats dashStats;
    [SerializeField] private VfxPool dashExplosionVfxPool;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField, Min(1)] private int maxHitColliders;

    public float RemainingCooldown =>
        Mathf.Max(0f, nextAvailableTime - Time.time);

    public event Action DashStarted;
    public event Action DashExploded;

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
        if (!CanDash())
            return;

        StartDash();
    }

    private bool CanDash()
    {
        return dashCoroutine == null &&
            Time.time >= nextAvailableTime &&
            dashStats.DashDuration > 0f;
    }

    private void StartDash()
    {
        nextAvailableTime = Time.time + dashStats.CooldownSeconds;
        DashStarted?.Invoke();
        dashCoroutine = StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        BeginDash();

        Vector3 dashDirection = transform.forward;
        float elapsedTime = 0f;

        while (elapsedTime < dashStats.DashDuration)
        {
            elapsedTime += MoveDash(dashDirection, elapsedTime);
            yield return null;
        }

        FinishDash();
    }

    private void BeginDash()
    {
        playerMovement.SetInputEnabled(false);
    }

    private float MoveDash(Vector3 dashDirection, float elapsedTime)
    {
        float frameDuration = Mathf.Min(
            Time.deltaTime,
            dashStats.DashDuration - elapsedTime);

        float dashStep = dashStats.DashDistance *
            (frameDuration / dashStats.DashDuration);

        characterController.Move(dashDirection * dashStep);
        return frameDuration;
    }

    private void FinishDash()
    {
        playerMovement.SetInputEnabled(true);
        Explode();
        dashCoroutine = null;
    }

    private void Explode()
    {
        float finalDamage = CalculateExplosionDamage();
        DamageEnemiesInRadius(finalDamage);
        PlayExplosionFeedback();
    }

    private float CalculateExplosionDamage()
    {
        return DamageCalculator.CalculateDamageDealt(
            dashStats.ExplosionBaseDamage,
            playerProgression.DamageMultiplier);
    }

    private void DamageEnemiesInRadius(float damage)
    {
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
                damageable.TakeDamage(damage);
            }
        }
    }

    private void PlayExplosionFeedback()
    {
        if (dashExplosionVfxPool != null)
            dashExplosionVfxPool.Play(transform.position, Quaternion.identity);

        DashExploded?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (dashStats == null)
            return;

        Vector3 origin = transform.position +
            transform.forward * dashStats.DashDistance +
            Vector3.up * 0.03f;

        Handles.color = new Color(0f, 0.7f, 1f, 0.18f);
        Handles.DrawSolidDisc(
            origin,
            Vector3.up,
            dashStats.ExplosionRadius);

        Handles.color = new Color(0f, 0.8f, 1f, 1f);
        Handles.DrawWireDisc(
            origin,
            Vector3.up,
            dashStats.ExplosionRadius);
    }
#endif
}

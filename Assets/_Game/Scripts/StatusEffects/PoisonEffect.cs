using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PoisonEffect : MonoBehaviour {
    [SerializeField] private VfxPool poisonVfxPool;
    [SerializeField] private Transform poisonVfxPoint;

    public event Action PoisonDamageApplied;

    private Health health;
    private Coroutine poisonRoutine;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void Apply(float damagePerTick, int tickCount, float duration)
    {
        if (poisonRoutine != null)
        {
            StopCoroutine(poisonRoutine);
        }

        poisonRoutine = StartCoroutine(
            ApplyRoutine(damagePerTick, tickCount, duration));
    }

    private IEnumerator ApplyRoutine(
        float damagePerTick,
        int tickCount,
        float duration)
    {
        float tickInterval = duration / (tickCount - 1);

        for (int tickIndex = 0; tickIndex < tickCount; tickIndex++)
        {
            float healthBeforeTick = health.CurrentHealth;
            health.TakeDamage(damagePerTick, false);

            if (health.CurrentHealth < healthBeforeTick)
            {
                Vector3 vfxPosition = poisonVfxPoint != null
                    ? poisonVfxPoint.position
                    : transform.position;

                poisonVfxPool?.Play(vfxPosition, Quaternion.identity);
                PoisonDamageApplied?.Invoke();
            }

            if (tickIndex < tickCount - 1)
            {
                yield return new WaitForSeconds(tickInterval);
            }
        }

        poisonRoutine = null;
    }

    private void OnDisable()
    {
        if (poisonRoutine != null)
        {
            StopCoroutine(poisonRoutine);
            poisonRoutine = null;
        }
    }
}

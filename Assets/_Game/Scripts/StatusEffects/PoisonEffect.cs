using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PoisonEffect : MonoBehaviour {
    [SerializeField] private VfxPool poisonVfxPool;
    [SerializeField] private Transform poisonVfxPoint;

    public event Action PoisonApplied;
    public event Action PoisonDamageApplied;

    private Health health;
    private Coroutine poisonRoutine;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void Apply(float damagePerTick, int tickCount, float duration)
    {
        PoisonApplied?.Invoke();

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
        float tickInterval = CalculateTickInterval(duration, tickCount);

        for (int tickIndex = 0; tickIndex < tickCount; tickIndex++)
        {
            ApplyPoisonTick(damagePerTick);

            if (tickIndex < tickCount - 1)
            {
                yield return new WaitForSeconds(tickInterval);
            }
        }

        poisonRoutine = null;
    }

    private float CalculateTickInterval(float duration, int tickCount)
    {
        return duration / (tickCount - 1);
    }

    private void ApplyPoisonTick(float damagePerTick)
    {
        float healthBeforeTick = health.CurrentHealth;
        health.TakeDamage(damagePerTick, false);

        if (health.CurrentHealth >= healthBeforeTick)
            return;

        poisonVfxPool?.Play(GetPoisonVfxPosition(), Quaternion.identity);
        PoisonDamageApplied?.Invoke();
    }

    private Vector3 GetPoisonVfxPosition()
    {
        return poisonVfxPoint != null
            ? poisonVfxPoint.position
            : transform.position;
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

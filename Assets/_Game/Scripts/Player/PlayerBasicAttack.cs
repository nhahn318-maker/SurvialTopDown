using System;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour {
    [SerializeField] private BasicAttackStats basicAttackStats;

    public int CurrentCharges { get; private set; }
    public int MaxCharges => basicAttackStats.MaxCharges;

    public float ProjectileBaseDamage =>
    basicAttackStats.ProjectileBaseDamage;

    public event Action<int, int> ChargesChanged;
    public event Action<float> ProjectileRequested;

    private float nextFireTime;
    private float nextChargeRecoveryTime;

    private void Awake()
    {
        if (basicAttackStats == null)
        {
            Debug.LogError("PlayerBasicAttack requires BasicAttackStats.", this);
            enabled = false;
            return;
        }

        CurrentCharges = basicAttackStats.MaxCharges;
        ChargesChanged?.Invoke(CurrentCharges, MaxCharges);
    }

    private void Update()
    {
        if (CurrentCharges >= MaxCharges)
            return;

        if (Time.time < nextChargeRecoveryTime)
            return;

        CurrentCharges++;
        ChargesChanged?.Invoke(CurrentCharges, MaxCharges);

        if (CurrentCharges < MaxCharges)
        {
            nextChargeRecoveryTime +=
                basicAttackStats.ChargeRecoverySeconds;
        }
    }

    public void TryFire()
    {
        if (CurrentCharges <= 0 || Time.time < nextFireTime)
            return;

        bool wasAtFullCharges = CurrentCharges == MaxCharges;

        CurrentCharges--;
        nextFireTime = Time.time + basicAttackStats.MinimumFireInterval;

        if (wasAtFullCharges)
        {
            nextChargeRecoveryTime =
                Time.time + basicAttackStats.ChargeRecoverySeconds;
        }

        ChargesChanged?.Invoke(CurrentCharges, MaxCharges);

        foreach (float angle in basicAttackStats.SpreadAngles)
        {
            ProjectileRequested?.Invoke(angle);
        }
    }
}

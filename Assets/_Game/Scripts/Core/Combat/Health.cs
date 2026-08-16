using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float Armor { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<float, float> HealthChanged;
    public event Action<float, bool> Damaged;
    public event Action Died;

    public void Initialize(float maxHealth, float armor)
    {
        MaxHealth = Mathf.Max(0f, maxHealth);
        CurrentHealth = MaxHealth;
        Armor = Mathf.Max(0f, armor);
        IsDead = CurrentHealth <= 0f;

        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(float baseDamage)
    {
        TakeDamage(baseDamage, true);
    }

    public void TakeDamage(float baseDamage, bool playHitAnimation)
    {
        if (IsDead || baseDamage <= 0f)
            return;

        float finalDamage = DamageCalculator.CalculateDamageTaken(baseDamage, Armor);

        if (finalDamage <= 0f)
            return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - finalDamage);

        Damaged?.Invoke(finalDamage, playHitAnimation);
        HealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0f)
        {
            IsDead = true;
            Died?.Invoke();
        }
    }

    public void AddMaxHealth(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        MaxHealth += amount;

        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void RestoreHealth(float amount)
    {
        if (IsDead || amount <= 0f)
        {
            return;
        }

        CurrentHealth = Mathf.Min(
            MaxHealth,
            CurrentHealth + amount);

        HealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void AddArmor(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        Armor += amount;
    }
}

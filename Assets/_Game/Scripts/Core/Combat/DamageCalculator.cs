using UnityEngine;

public static class DamageCalculator {
    public static float CalculateDamageTaken(float baseDamage, float armor)
    {
        return Mathf.Max(0f, baseDamage - armor);
    }

    public static float CalculateDamageDealt(
        float baseDamage,
        float damageMultiplier)
    {
        return baseDamage * (1f + damageMultiplier);
    }
}
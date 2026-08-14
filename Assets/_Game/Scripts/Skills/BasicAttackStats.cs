using UnityEngine;

[CreateAssetMenu(
    fileName = "BasicAttackStats",
    menuName = "Survival Top-down/Skills/Basic Attack Stats")]
public class BasicAttackStats : ScriptableObject {
    [SerializeField, Min(0f)] private float projectileBaseDamage;
    [SerializeField, Min(1)] private int maxCharges;
    [SerializeField, Min(0f)] private float chargeRecoverySeconds;
    [SerializeField, Min(0f)] private float minimumFireInterval;
    [SerializeField] private float[] spreadAngles;

    public float ProjectileBaseDamage => projectileBaseDamage;
    public int MaxCharges => maxCharges;
    public float ChargeRecoverySeconds => chargeRecoverySeconds;
    public float MinimumFireInterval => minimumFireInterval;
    public float[] SpreadAngles => spreadAngles;
}
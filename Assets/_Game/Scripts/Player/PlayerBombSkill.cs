using System;
using UnityEngine;

public class PlayerBombSkill : MonoBehaviour {
    [SerializeField] private PlayerProgression playerProgression;
    [SerializeField] private BombStats bombStats;
    [SerializeField] private BombPool bombPool;
    [SerializeField] private VfxPool explosionVfxPool;
    [SerializeField] private Transform bombPlacementPoint;

    public float RemainingCooldown =>
        Mathf.Max(0f, nextAvailableTime - Time.time);

    public event Action BombPlaced;

    private float nextAvailableTime;

    public void TryPlaceBomb()
    {
        if (Time.time < nextAvailableTime)
            return;

        float finalDamage = DamageCalculator.CalculateDamageDealt(
            bombStats.BaseDamage,
            playerProgression.DamageMultiplier);

        GameObject bombObject = bombPool.Get(
            bombPlacementPoint.position,
            Quaternion.identity);

        Bomb bomb = bombObject.GetComponent<Bomb>();
        bomb.Activate(bombPool, explosionVfxPool, finalDamage);
        BombPlaced?.Invoke();

        nextAvailableTime = Time.time + bombStats.CooldownSeconds;
    }
}

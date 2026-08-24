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
    public event Action BombExploded;

    private float nextAvailableTime;

    private void Awake()
    {
        if (playerProgression == null ||
            bombStats == null ||
            bombPool == null ||
            explosionVfxPool == null ||
            bombPlacementPoint == null)
        {
            Debug.LogError(
                "PlayerBombSkill requires all references.",
                this);

            enabled = false;
        }
    }

    public void TryPlaceBomb()
    {
        if (!CanPlaceBomb())
            return;

        PlaceBomb();
        StartCooldown();
    }

    private bool CanPlaceBomb()
    {
        return Time.time >= nextAvailableTime;
    }

    private void PlaceBomb()
    {
        float damage = CalculateBombDamage();

        GameObject bombObject = bombPool.Get(
            bombPlacementPoint.position,
            Quaternion.identity);

        Bomb bomb = bombObject.GetComponent<Bomb>();
        bomb.Activate(
            bombPool,
            explosionVfxPool,
            damage,
            NotifyBombExploded);

        BombPlaced?.Invoke();
    }

    private float CalculateBombDamage()
    {
        return DamageCalculator.CalculateDamageDealt(
            bombStats.BaseDamage,
            playerProgression.DamageMultiplier);
    }

    private void StartCooldown()
    {
        nextAvailableTime = Time.time + bombStats.CooldownSeconds;
    }

    private void NotifyBombExploded()
    {
        BombExploded?.Invoke();
    }
}

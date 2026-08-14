using UnityEngine;

public class PlayerBombSkill : MonoBehaviour {
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private BombStats bombStats;
    [SerializeField] private BombPool bombPool;
    [SerializeField] private Transform bombPlacementPoint;

    public float RemainingCooldown =>
        Mathf.Max(0f, nextAvailableTime - Time.time);

    private float nextAvailableTime;

    public void TryPlaceBomb()
    {
        if (Time.time < nextAvailableTime)
            return;

        float finalDamage = DamageCalculator.CalculateDamageDealt(
            bombStats.BaseDamage,
            playerStats.DamageMultiplier);

        GameObject bombObject = bombPool.Get(
            bombPlacementPoint.position,
            Quaternion.identity);

        Bomb bomb = bombObject.GetComponent<Bomb>();
        bomb.Activate(bombPool, finalDamage);

        nextAvailableTime = Time.time + bombStats.CooldownSeconds;
    }
}
using UnityEngine;

[CreateAssetMenu(
    fileName = "WaveConfig",
    menuName = "Survival Top-down/Progression/Wave Config")]
public class WaveConfig : ScriptableObject {
    [SerializeField, Min(0)] private int minMeleeCount;
    [SerializeField, Min(0)] private int maxMeleeCount;
    [SerializeField, Min(0)] private int minRangedCount;
    [SerializeField, Min(0)] private int maxRangedCount;

    [Header("Spawn Timing")]
    [SerializeField, Min(0f)] private float spawnIntervalSeconds;

    public int MinMeleeCount => minMeleeCount;
    public int MaxMeleeCount => maxMeleeCount;
    public int MinRangedCount => minRangedCount;
    public int MaxRangedCount => maxRangedCount;
    public float SpawnIntervalSeconds => spawnIntervalSeconds;
}

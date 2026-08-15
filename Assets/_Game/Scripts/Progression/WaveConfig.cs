using UnityEngine;

[CreateAssetMenu(
    fileName = "WaveConfig",
    menuName = "Survival Top-down/Progression/Wave Config")]
public class WaveConfig : ScriptableObject {
    [SerializeField, Min(0)] private int minMeleeCount;
    [SerializeField, Min(0)] private int maxMeleeCount;
    [SerializeField, Min(0)] private int minRangedCount;
    [SerializeField, Min(0)] private int maxRangedCount;

    public int MinMeleeCount => minMeleeCount;
    public int MaxMeleeCount => maxMeleeCount;
    public int MinRangedCount => minRangedCount;
    public int MaxRangedCount => maxRangedCount;
}
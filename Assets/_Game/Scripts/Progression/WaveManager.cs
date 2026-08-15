using UnityEngine;

public class WaveManager : MonoBehaviour {
    [SerializeField] private WaveConfig waveConfig;
    [SerializeField] private EnemyPool meleeEnemyPool;
    [SerializeField] private EnemyPool rangedEnemyPool;
    [SerializeField] private Transform[] spawnPoints;

    private int currentWave;
    private bool hasActiveWave;

    public int CurrentWave => currentWave;

    private void Awake()
    {
        if (waveConfig == null ||
            meleeEnemyPool == null ||
            rangedEnemyPool == null ||
            spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogError("WaveManager requires all references.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        SpawnNextWave();
    }

    private void Update()
    {
        if (!hasActiveWave)
        {
            return;
        }

        if (meleeEnemyPool.ActiveCount > 0 ||
            rangedEnemyPool.ActiveCount > 0)
        {
            return;
        }

        SpawnNextWave();
    }

    private void SpawnNextWave()
    {
        currentWave++;
        hasActiveWave = true;

        int meleeCount = Random.Range(
            waveConfig.MinMeleeCount,
            waveConfig.MaxMeleeCount + 1);

        int rangedCount = Random.Range(
            waveConfig.MinRangedCount,
            waveConfig.MaxRangedCount + 1);

        SpawnEnemies(meleeEnemyPool, meleeCount);
        SpawnEnemies(rangedEnemyPool, rangedCount);
    }

    private void SpawnEnemies(EnemyPool enemyPool, int count)
    {
        for (int index = 0; index < count; index++)
        {
            Transform spawnPoint =
                spawnPoints[Random.Range(0, spawnPoints.Length)];

            enemyPool.Get(
                spawnPoint.position,
                spawnPoint.rotation);
        }
    }
}
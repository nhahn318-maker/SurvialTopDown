using System.Collections.Generic;
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
        int meleeCount = Random.Range(
            waveConfig.MinMeleeCount,
            waveConfig.MaxMeleeCount + 1);

        int rangedCount = Random.Range(
            waveConfig.MinRangedCount,
            waveConfig.MaxRangedCount + 1);

        int totalEnemyCount = meleeCount + rangedCount;

        if (totalEnemyCount > spawnPoints.Length)
        {
            Debug.LogError(
                "WaveManager requires at least one spawn point per enemy.",
                this);

            hasActiveWave = false;
            return;
        }

        currentWave++;
        hasActiveWave = true;

        List<Transform> availableSpawnPoints =
            new List<Transform>(spawnPoints);

        SpawnEnemies(
            meleeEnemyPool,
            meleeCount,
            availableSpawnPoints);

        SpawnEnemies(
            rangedEnemyPool,
            rangedCount,
            availableSpawnPoints);
    }

    private static void SpawnEnemies(
        EnemyPool enemyPool,
        int count,
        List<Transform> availableSpawnPoints)
    {
        for (int index = 0; index < count; index++)
        {
            int spawnPointIndex = Random.Range(
                0,
                availableSpawnPoints.Count);

            Transform spawnPoint =
                availableSpawnPoints[spawnPointIndex];

            availableSpawnPoints.RemoveAt(spawnPointIndex);

            enemyPool.Get(
                spawnPoint.position,
                spawnPoint.rotation);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour {
    [SerializeField] private WaveConfig waveConfig;
    [SerializeField] private EnemyPool meleeEnemyPool;
    [SerializeField] private EnemyPool rangedEnemyPool;
    [SerializeField] private Transform[] spawnPoints;

    private int currentWave;
    private bool hasActiveWave;
    private Coroutine spawnWaveCoroutine;

    public int CurrentWave => currentWave;
    public event Action<int> WaveStarted;

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
        BeginNextWave();
    }

    private void Update()
    {
        if (!hasActiveWave || spawnWaveCoroutine != null)
        {
            return;
        }

        if (!AreAllEnemiesDefeated())
            return;

        BeginNextWave();
    }

    private void OnDisable()
    {
        if (spawnWaveCoroutine != null)
        {
            StopCoroutine(spawnWaveCoroutine);
            spawnWaveCoroutine = null;
        }
    }

    private void BeginNextWave()
    {
        if (spawnWaveCoroutine == null)
        {
            spawnWaveCoroutine = StartCoroutine(SpawnNextWave());
        }
    }

    private System.Collections.IEnumerator SpawnNextWave()
    {
        GetRandomEnemyCounts(out int meleeCount, out int rangedCount);

        int totalEnemyCount = meleeCount + rangedCount;

        if (!HasEnoughSpawnPoints(totalEnemyCount))
        {
            hasActiveWave = false;
            spawnWaveCoroutine = null;
            yield break;
        }

        currentWave++;
        hasActiveWave = true;
        WaveStarted?.Invoke(currentWave);

        List<Transform> availableSpawnPoints =
            new List<Transform>(spawnPoints);

        List<EnemyPool> spawnPlan = CreateSpawnPlan(
            meleeCount,
            rangedCount);

        for (int index = 0; index < spawnPlan.Count; index++)
        {
            SpawnEnemy(spawnPlan[index], availableSpawnPoints);

            if (index < spawnPlan.Count - 1 &&
                waveConfig.SpawnIntervalSeconds > 0f)
            {
                yield return new WaitForSeconds(
                    waveConfig.SpawnIntervalSeconds);
            }
        }

        spawnWaveCoroutine = null;
    }

    private bool AreAllEnemiesDefeated()
    {
        return meleeEnemyPool.ActiveCount == 0 &&
            rangedEnemyPool.ActiveCount == 0;
    }

    private void GetRandomEnemyCounts(
        out int meleeCount,
        out int rangedCount)
    {
        meleeCount = Random.Range(
            waveConfig.MinMeleeCount,
            waveConfig.MaxMeleeCount + 1);

        rangedCount = Random.Range(
            waveConfig.MinRangedCount,
            waveConfig.MaxRangedCount + 1);
    }

    private bool HasEnoughSpawnPoints(int totalEnemyCount)
    {
        if (totalEnemyCount <= spawnPoints.Length)
            return true;

        Debug.LogError(
            "WaveManager requires at least one spawn point per enemy.",
            this);

        return false;
    }

    private List<EnemyPool> CreateSpawnPlan(
        int meleeCount,
        int rangedCount)
    {
        List<EnemyPool> spawnPlan = new List<EnemyPool>(
            meleeCount + rangedCount);

        AddEnemiesToPlan(spawnPlan, meleeEnemyPool, meleeCount);
        AddEnemiesToPlan(spawnPlan, rangedEnemyPool, rangedCount);

        for (int index = spawnPlan.Count - 1; index > 0; index--)
        {
            int swapIndex = Random.Range(0, index + 1);
            (spawnPlan[index], spawnPlan[swapIndex]) =
                (spawnPlan[swapIndex], spawnPlan[index]);
        }

        return spawnPlan;
    }

    private static void AddEnemiesToPlan(
        List<EnemyPool> spawnPlan,
        EnemyPool enemyPool,
        int count)
    {
        for (int index = 0; index < count; index++)
        {
            spawnPlan.Add(enemyPool);
        }
    }

    private static void SpawnEnemy(
        EnemyPool enemyPool,
        List<Transform> availableSpawnPoints)
    {
        int spawnPointIndex = Random.Range(
            0,
            availableSpawnPoints.Count);

        Transform spawnPoint = availableSpawnPoints[spawnPointIndex];

        availableSpawnPoints.RemoveAt(spawnPointIndex);

        enemyPool.Get(
            spawnPoint.position,
            spawnPoint.rotation);
    }
}

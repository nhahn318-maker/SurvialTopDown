using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField, Min(1)] private int defaultCapacity;
    [SerializeField, Min(1)] private int maxSize;

    private readonly List<GameObject> enemies = new();

    public int ActiveCount
    {
        get
        {
            int count = 0;

            foreach (GameObject enemy in enemies)
            {
                if (enemy.activeSelf)
                {
                    count++;
                }
            }

            return count;
        }
    }

    private void Awake()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyPool requires an enemy prefab.", this);
            enabled = false;
            return;
        }

        for (int index = 0; index < defaultCapacity; index++)
        {
            CreateEnemy();
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf)
            {
                continue;
            }

            enemy.transform.SetPositionAndRotation(position, rotation);
            enemy.SetActive(true);
            return enemy;
        }

        if (enemies.Count >= maxSize)
        {
            Debug.LogWarning("EnemyPool reached its maximum size.", this);
            return null;
        }

        GameObject createdEnemy = CreateEnemy();
        createdEnemy.transform.SetPositionAndRotation(position, rotation);
        createdEnemy.SetActive(true);

        return createdEnemy;
    }

    private GameObject CreateEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform);
        enemy.SetActive(false);
        enemies.Add(enemy);

        return enemy;
    }
}
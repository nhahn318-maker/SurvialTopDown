using UnityEngine;
using UnityEngine.Pool;

public class BombPool : MonoBehaviour {
    [SerializeField] private GameObject bombPrefab;
    [SerializeField, Min(1)] private int defaultCapacity;
    [SerializeField, Min(1)] private int maxSize;

    private IObjectPool<GameObject> pool;

    private void Awake()
    {
        if (bombPrefab == null)
        {
            Debug.LogError("BombPool requires a bomb prefab.", this);
            enabled = false;
            return;
        }

        pool = new ObjectPool<GameObject>(
            CreateBomb,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledBomb,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize);
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject bomb = pool.Get();
        bomb.transform.SetPositionAndRotation(position, rotation);
        return bomb;
    }

    public void Release(GameObject bomb)
    {
        pool.Release(bomb);
    }

    private GameObject CreateBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, transform);
        bomb.SetActive(false);
        return bomb;
    }

    private void OnGetFromPool(GameObject bomb)
    {
        bomb.SetActive(true);
    }

    private void OnReleaseToPool(GameObject bomb)
    {
        bomb.SetActive(false);
        bomb.transform.SetParent(transform);
    }

    private void OnDestroyPooledBomb(GameObject bomb)
    {
        Destroy(bomb);
    }

    private void OnDestroy()
    {
        pool?.Clear();
    }
}
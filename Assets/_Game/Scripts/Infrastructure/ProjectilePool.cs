using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour {
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField, Min(1)] private int defaultCapacity;
    [SerializeField, Min(1)] private int maxSize;

    private IObjectPool<GameObject> pool;

    public ProjectileStats ProjectileStats =>
        projectilePrefab != null
            ? projectilePrefab.GetComponent<PlayerProjectile>()?.Stats
            : null;

    private void Awake()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("ProjectilePool requires a projectile prefab.", this);
            enabled = false;
            return;
        }

        pool = new ObjectPool<GameObject>(
            CreateProjectile,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledProjectile,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize);
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject projectile = pool.Get();
        projectile.transform.SetPositionAndRotation(position, rotation);
        return projectile;
    }

    public void Release(GameObject projectile)
    {
        pool.Release(projectile);
    }

    private GameObject CreateProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform);
        projectile.SetActive(false);
        return projectile;
    }

    private void OnGetFromPool(GameObject projectile)
    {
        projectile.SetActive(true);
    }

    private void OnReleaseToPool(GameObject projectile)
    {
        projectile.SetActive(false);
        projectile.transform.SetParent(transform);
    }

    private void OnDestroyPooledProjectile(GameObject projectile)
    {
        Destroy(projectile);
    }

    private void OnDestroy()
    {
        pool?.Clear();
    }
}

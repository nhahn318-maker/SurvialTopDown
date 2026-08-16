using UnityEngine;
using UnityEngine.Pool;

public class VfxPool : MonoBehaviour
{
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField, Min(1)] private int defaultCapacity;
    [SerializeField, Min(1)] private int maxSize;

    private IObjectPool<GameObject> pool;

    private void Awake()
    {
        if (vfxPrefab == null ||
            vfxPrefab.GetComponent<PooledVfx>() == null)
        {
            Debug.LogError(
                "VfxPool requires a prefab with PooledVfx.",
                this);

            enabled = false;
            return;
        }

        pool = new ObjectPool<GameObject>(
            CreateVfx,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledVfx,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize);
    }

    public void Play(Vector3 position, Quaternion rotation)
    {
        GameObject vfx = pool.Get();
        vfx.transform.SetPositionAndRotation(position, rotation);
        vfx.GetComponent<PooledVfx>().Play(this);
    }

    public void Release(GameObject vfx)
    {
        pool.Release(vfx);
    }

    private GameObject CreateVfx()
    {
        GameObject vfx = Instantiate(vfxPrefab, transform);
        vfx.SetActive(false);
        return vfx;
    }

    private static void OnGetFromPool(GameObject vfx)
    {
        vfx.SetActive(true);
    }

    private void OnReleaseToPool(GameObject vfx)
    {
        vfx.SetActive(false);
        vfx.transform.SetParent(transform);
    }

    private static void OnDestroyPooledVfx(GameObject vfx)
    {
        Destroy(vfx);
    }

    private void OnDestroy()
    {
        pool?.Clear();
    }
}

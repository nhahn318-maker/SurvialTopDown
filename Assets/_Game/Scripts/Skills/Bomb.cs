using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    [SerializeField] private BombStats bombStats;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField, Min(1)] private int maxHitColliders;

    private Collider[] overlapBuffer;
    private readonly HashSet<IDamageable> damagedTargets = new();

    private BombPool ownerPool;
    private float explosionDamage;
    private Coroutine detonationCoroutine;

    private void Awake()
    {
        overlapBuffer = new Collider[maxHitColliders];
    }

    private void OnDisable()
    {
        if (detonationCoroutine != null)
        {
            StopCoroutine(detonationCoroutine);
            detonationCoroutine = null;
        }
    }

    public void Activate(BombPool pool, float finalDamage)
    {
        ownerPool = pool;
        explosionDamage = finalDamage;

        detonationCoroutine = StartCoroutine(DetonateAfterDelay());
    }

    private IEnumerator DetonateAfterDelay()
    {
        yield return new WaitForSeconds(bombStats.DetonationDelay);

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            bombStats.ExplosionRadius,
            overlapBuffer,
            enemyLayers,
            QueryTriggerInteraction.Collide);

        damagedTargets.Clear();

        for (int index = 0; index < hitCount; index++)
        {
            IDamageable damageable =
                overlapBuffer[index].GetComponentInParent<IDamageable>();

            if (damageable != null && damagedTargets.Add(damageable))
            {
                damageable.TakeDamage(explosionDamage);
            }
        }

        ownerPool.Release(gameObject);
    }
}
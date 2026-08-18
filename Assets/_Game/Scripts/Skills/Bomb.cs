using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Bomb : MonoBehaviour {
    [SerializeField] private BombStats bombStats;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField, Min(1)] private int maxHitColliders;

    private Collider[] overlapBuffer;
    private readonly HashSet<IDamageable> damagedTargets = new();

    private BombPool ownerPool;
    private VfxPool explosionVfxPool;
    private float explosionDamage;
    private Coroutine detonationCoroutine;
    private Action onExploded;

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

    public void Activate(
        BombPool pool,
        VfxPool vfxPool,
        float finalDamage,
        Action explosionCallback)
    {
        ownerPool = pool;
        explosionVfxPool = vfxPool;
        explosionDamage = finalDamage;
        onExploded = explosionCallback;

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

        if (explosionVfxPool != null)
            explosionVfxPool.Play(transform.position, Quaternion.identity);

        onExploded?.Invoke();
        onExploded = null;
        ownerPool.Release(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (bombStats == null)
            return;

        Vector3 origin = transform.position + Vector3.up * 0.03f;

        Handles.color = new Color(0f, 0.7f, 1f, 0.18f);
        Handles.DrawSolidDisc(
            origin,
            Vector3.up,
            bombStats.ExplosionRadius);

        Handles.color = new Color(0f, 0.8f, 1f, 1f);
        Handles.DrawWireDisc(
            origin,
            Vector3.up,
            bombStats.ExplosionRadius);
    }
#endif
}

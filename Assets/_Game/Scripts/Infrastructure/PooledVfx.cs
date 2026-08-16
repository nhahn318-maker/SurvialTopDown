using UnityEngine;

public class PooledVfx : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    private VfxPool ownerPool;
    private bool isPlaying;

    private void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);

        if (particleSystems.Length == 0)
        {
            Debug.LogError("PooledVfx requires a ParticleSystem.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        StopAndClear();
        isPlaying = false;
    }

    private void Update()
    {
        if (!isPlaying || IsAnyParticleSystemAlive())
            return;

        isPlaying = false;
        ownerPool.Release(gameObject);
    }

    private void OnDisable()
    {
        isPlaying = false;
        ownerPool = null;
    }

    public void Play(VfxPool pool)
    {
        ownerPool = pool;
        StopAndClear();

        foreach (ParticleSystem particleSystem in particleSystems)
            particleSystem.Play(true);

        isPlaying = true;
    }

    private bool IsAnyParticleSystemAlive()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            if (particleSystem.IsAlive(true))
                return true;
        }

        return false;
    }

    private void StopAndClear()
    {
        if (particleSystems == null)
            return;

        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}

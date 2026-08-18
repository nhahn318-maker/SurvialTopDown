using UnityEngine;

public class PlayerLevelUpVfx : MonoBehaviour
{
    [SerializeField] private PlayerProgression playerProgression;
    [SerializeField] private VfxPool levelUpVfxPool;
    [SerializeField] private Transform levelUpVfxPoint;

    private void Awake()
    {
        if (playerProgression == null || levelUpVfxPool == null)
        {
            Debug.LogError(
                "PlayerLevelUpVfx requires all references.",
                this);

            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerProgression != null)
            playerProgression.LevelChanged += PlayLevelUpVfx;
    }

    private void OnDisable()
    {
        if (playerProgression != null)
            playerProgression.LevelChanged -= PlayLevelUpVfx;
    }

    private void PlayLevelUpVfx(int level)
    {
        if (level <= 1)
            return;

        Vector3 vfxPosition = levelUpVfxPoint != null
            ? levelUpVfxPoint.position
            : transform.position;

        levelUpVfxPool.Play(vfxPosition, Quaternion.identity);
    }
}

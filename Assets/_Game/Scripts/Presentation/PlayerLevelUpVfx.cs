using UnityEngine;

public class PlayerLevelUpVfx : MonoBehaviour
{
    [SerializeField] private PlayerProgression playerProgression;
    [SerializeField] private VfxPool levelUpVfxPool;
    [SerializeField] private Transform levelUpVfxPoint;

    [Header("Floating Text")]
    [SerializeField] private GameObject levelUpPopupPrefab;
    [SerializeField] private Transform levelUpPopupPoint;
    [SerializeField, Min(0.01f)] private float popupLifetime;

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

        if (levelUpPopupPrefab == null)
            return;

        Vector3 popupPosition = levelUpPopupPoint != null
            ? levelUpPopupPoint.position
            : vfxPosition;

        GameObject popup = Instantiate(
            levelUpPopupPrefab,
            popupPosition,
            Quaternion.identity);

        Destroy(popup, popupLifetime);
    }
}

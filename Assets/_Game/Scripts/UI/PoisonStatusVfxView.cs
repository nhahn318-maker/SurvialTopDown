using System.Collections;
using UnityEngine;

public class PoisonStatusVfxView : MonoBehaviour
{
    [SerializeField] private PoisonEffect poisonEffect;
    [SerializeField] private GameObject poisonStatusVfx;
    [SerializeField, Min(0.01f)] private float flashDuration;

    private Coroutine flashRoutine;

    private void Awake()
    {
        if (poisonEffect == null || poisonStatusVfx == null)
        {
            Debug.LogError(
                "PoisonStatusVfxView requires all references.",
                this);

            enabled = false;
            return;
        }

        poisonStatusVfx.SetActive(false);
    }

    private void OnEnable()
    {
        if (poisonEffect != null)
            poisonEffect.PoisonDamageApplied += Flash;
    }

    private void OnDisable()
    {
        if (poisonEffect != null)
            poisonEffect.PoisonDamageApplied -= Flash;

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }
    }

    private void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        poisonStatusVfx.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        poisonStatusVfx.SetActive(false);
        flashRoutine = null;
    }
}

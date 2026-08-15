using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Health))]
public class MeleeEnemyHealth : MonoBehaviour {
    [SerializeField] private MeleeEnemyStats enemyStats;
    [SerializeField, Min(0f)] private float deathDespawnDelay;

    private Health health;
    private MeleeEnemyBrain enemyBrain;
    private CharacterController characterController;
    private EnemyAnimationController animationController;
    private Coroutine deathRoutine;

    private void Awake()
    {
        health = GetComponent<Health>();
        enemyBrain = GetComponent<MeleeEnemyBrain>();
        characterController = GetComponent<CharacterController>();
        animationController = GetComponentInChildren<EnemyAnimationController>(true);

        if (enemyStats == null)
        {
            Debug.LogError("MeleeEnemyHealth requires MeleeEnemyStats.", this);
            enabled = false;
            return;
        }

        health.Died += HandleDeath;
    }

    private void OnEnable()
    {
        deathRoutine = null;

        if (enemyBrain != null)
            enemyBrain.enabled = true;

        if (characterController != null)
            characterController.enabled = true;

        if (enemyStats != null)
        {
            health.Initialize(
                enemyStats.MaxHealth,
                enemyStats.Armor);
        }
    }

    private void OnDestroy()
    {
        if (health != null)
            health.Died -= HandleDeath;
    }

    private void HandleDeath()
    {
        if (deathRoutine != null)
            return;

        if (enemyBrain != null)
            enemyBrain.enabled = false;

        if (characterController != null)
            characterController.enabled = false;

        if (animationController != null)
            animationController.TriggerDeath();

        deathRoutine = StartCoroutine(DespawnAfterDeath());
    }

    private IEnumerator DespawnAfterDeath()
    {
        yield return new WaitForSeconds(deathDespawnDelay);
        gameObject.SetActive(false);
    }
}

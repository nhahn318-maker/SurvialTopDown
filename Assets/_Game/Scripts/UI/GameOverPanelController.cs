using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanelController : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    private Health health;
    private bool isGameOver;

    private void Awake()
    {
        health = playerHealth != null
            ? playerHealth.GetComponent<Health>()
            : null;

        if (health == null || gameOverPanel == null || restartButton == null)
        {
            Debug.LogError(
                "GameOverPanelController requires all references.",
                this);

            enabled = false;
            return;
        }

        gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (health != null)
            health.Died += ShowGameOver;

        if (restartButton != null)
            restartButton.onClick.AddListener(Restart);
    }

    private void OnDisable()
    {
        if (health != null)
            health.Died -= ShowGameOver;

        if (restartButton != null)
            restartButton.onClick.RemoveListener(Restart);
    }

    private void ShowGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }
}

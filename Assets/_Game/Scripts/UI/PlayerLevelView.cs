using TMPro;
using UnityEngine;

public class PlayerLevelView : MonoBehaviour {
    [SerializeField] private PlayerProgression playerProgression;
    [SerializeField] private TMP_Text levelText;

    private void Start()
    {
        if (playerProgression == null || levelText == null)
        {
            Debug.LogError(
                "PlayerLevelView requires all references.",
                this);

            enabled = false;
            return;
        }

        playerProgression.LevelChanged += Refresh;
        Refresh(playerProgression.CurrentLevel);
    }

    private void OnDestroy()
    {
        if (playerProgression != null)
        {
            playerProgression.LevelChanged -= Refresh;
        }
    }

    private void Refresh(int level)
    {
        levelText.text = $"LEVEL {level}";
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombCooldownView : MonoBehaviour {
    [SerializeField] private PlayerBombSkill bombSkill;
    [SerializeField] private Button bombButton;
    [SerializeField] private TMP_Text cooldownText;

    private void Update()
    {
        if (bombSkill == null || bombButton == null || cooldownText == null)
            return;

        float remainingCooldown = bombSkill.RemainingCooldown;
        bool isReady = remainingCooldown <= 0f;

        bombButton.interactable = isReady;
        cooldownText.text = isReady
            ? "READY"
            : Mathf.CeilToInt(remainingCooldown).ToString();
    }
}
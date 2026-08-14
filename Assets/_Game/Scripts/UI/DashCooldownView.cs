using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownView : MonoBehaviour {
    [SerializeField] private PlayerDashSkill dashSkill;
    [SerializeField] private Button dashButton;
    [SerializeField] private TMP_Text cooldownText;

    private void Update()
    {
        if (dashSkill == null || dashButton == null || cooldownText == null)
            return;

        float remainingCooldown = dashSkill.RemainingCooldown;
        bool isReady = remainingCooldown <= 0f;

        dashButton.interactable = isReady;
        cooldownText.text = isReady
            ? "READY"
            : Mathf.CeilToInt(remainingCooldown).ToString();
    }
}
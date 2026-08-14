using TMPro;
using UnityEngine;

public class BasicAttackChargeView : MonoBehaviour {
    [SerializeField] private PlayerBasicAttack basicAttack;
    [SerializeField] private TMP_Text chargeText;

    private void OnEnable()
    {
        if (basicAttack == null || chargeText == null)
            return;

        basicAttack.ChargesChanged += Refresh;
        Refresh(basicAttack.CurrentCharges, basicAttack.MaxCharges);
    }

    private void OnDisable()
    {
        if (basicAttack != null)
            basicAttack.ChargesChanged -= Refresh;
    }

    private void Refresh(int currentCharges, int maxCharges)
    {
        chargeText.text = $"{currentCharges}/{maxCharges}";
    }
}
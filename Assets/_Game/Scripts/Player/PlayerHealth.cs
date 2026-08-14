using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerHealth : MonoBehaviour {
    [SerializeField] private PlayerStats playerStats;

    public Health Health { get; private set; }

    private void Awake()
    {
        Health = GetComponent<Health>();

        if (playerStats == null)
        {
            Debug.LogError("PlayerHealth requires PlayerStats.", this);
            enabled = false;
            return;
        }

        Health.Initialize(
            playerStats.MaxHealth,
            playerStats.Armor);
    }
}

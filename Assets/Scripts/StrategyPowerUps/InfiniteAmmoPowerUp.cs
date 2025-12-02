using UnityEngine;

public class InfiniteAmmoPowerUp : MonoBehaviour, IPowerUpStrategy
{
    public string Name => "Infinite Ammo";
    [SerializeField] private float duration = 5f;
    [SerializeField] private Color powerUpColor = Color.blue; // now blue for infinite ammo

    public void PowerUp(Simulacro.PlayerMovement player)
    {
        if (player == null) return;

        // Ensure player shows blue while infinite ammo is active
        player.ApplyPowerUpColorForDuration(powerUpColor, duration);

        // Activate real infinite ammo behavior via AmmoManager
        if (AmmoManager.Instance != null)
        {
            AmmoManager.Instance.ActivateInfiniteAmmoForDuration(duration);
            Debug.Log($"Infinite ammo activated for {duration} seconds");
        }
        else
        {
            Debug.LogWarning("AmmoManager.Instance is null. Ensure an AmmoManager exists in the scene.");
        }
    }
}
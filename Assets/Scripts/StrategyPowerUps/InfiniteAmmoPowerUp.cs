using UnityEngine;

public class InfiniteAmmoPowerUp : MonoBehaviour, IPowerUpStrategy
{
    public string Name => "InfiniteAmmoPowerUp";
    [SerializeField] private float duration = 8f;

    public void PowerUp(Simulacro.PlayerMovement player)
    {
        if (player == null) return;

        // Visual feedback for the duration of the power-up
        player.ApplyPowerUpColorForDuration(Color.blue, duration);

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
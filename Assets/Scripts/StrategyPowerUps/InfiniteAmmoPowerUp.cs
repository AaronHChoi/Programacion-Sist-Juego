using UnityEngine;

public class InfiniteAmmoPowerUp : MonoBehaviour, IPowerUpStrategy
{
    public string Name => "Infinite Ammo";
    [SerializeField] private float duration = 5f;

    public void PowerUp(Simulacro.PlayerMovement player)
    {
        if (player == null) return;

        // Activate real infinite ammo behavior via AmmoManager + will trigger Player visuals via event
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
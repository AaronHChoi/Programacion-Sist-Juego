using UnityEngine;
using Simulacro;

public class PowerUpController : MonoBehaviour
{
    private IPowerUpStrategy _current = new NonePowerUp();
   
    public void SetStrategy(IPowerUpStrategy strategy)
    {
        _current = strategy ?? new NonePowerUp();
        Debug.Log($"Equipped Powerup: {_current.Name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"PowerUpController OnTriggerEnter with: {other.name}, tags:{other.tag}");
        // When this script is on the Player, "other" will be the pickup object.
        if (other == null) return;

        // If other implements IPowerUpStrategy, apply it to this player
        if (other.TryGetComponent<IPowerUpStrategy>(out var pickupStrategy))
        {
            var playerMovement = GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                // maybe PowerUpController is on a parent; try to find PlayerMovement in parents
                playerMovement = GetComponentInParent<PlayerMovement>();
            }

            if (playerMovement != null)
            {
                // Try to auto-assign car body transform if user didn't set it manually
                // Look for a child named "truck.001" first, then for any Renderer in children
                Transform carBody = playerMovement.transform.Find("truck.001");
                if (carBody == null)
                {
                    var rend = playerMovement.GetComponentInChildren<Renderer>();
                    if (rend != null)
                        carBody = rend.transform;
                }

                if (carBody != null)
                {
                    playerMovement.SetCarBodyTransform(carBody);
                }

                pickupStrategy.PowerUp(playerMovement);
                Debug.Log($"[Pickup] Applied: {pickupStrategy.Name}");

                // deactivate pickup object
                other.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("PowerUpController: PlayerMovement not found on GameObject to apply pickup.");
            }

            return;
        }

        // If this script is on a pickup (legacy behavior), keep supporting collision with player
        if (other.CompareTag("Player"))
        {
            PowerUpController playerController = other.GetComponent<PowerUpController>();
            if (playerController != null)
            {
                IPowerUpStrategy powerUp = GetComponent<IPowerUpStrategy>();
                if (powerUp != null)
                {
                    var playerMovement = other.GetComponent<Simulacro.PlayerMovement>();
                    playerController.SetStrategy(powerUp);
                    powerUp.PowerUp(playerMovement);
                    Debug.Log($"[Pickup] Equipped: {powerUp.Name}");
                }
            }
            gameObject.SetActive(false);
            return;
        }
    }
}
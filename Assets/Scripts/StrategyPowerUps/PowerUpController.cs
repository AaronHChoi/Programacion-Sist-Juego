using UnityEngine;

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
        // If this is a dropped power-up and player touches it
        if (other.CompareTag("Player"))
        {
            PowerUpController playerController = other.GetComponent<PowerUpController>();
            if (playerController != null)
            {
                IPowerUpStrategy powerUp = GetComponent<IPowerUpStrategy>();
                if (powerUp != null)
                {
                    playerController.SetStrategy(powerUp);
                    powerUp.PowerUp();
                    Debug.Log($"[Pickup] Equipped: {powerUp.Name}");
                }
            }
            Destroy(gameObject);
            return;
        }

        // Original logic for picking up power-ups that are already on the ground
        IPowerUpStrategy powerUpStrategy = other.GetComponent<IPowerUpStrategy>();
        if (powerUpStrategy == null) return;

        SetStrategy(powerUpStrategy);
        powerUpStrategy.PowerUp();
        Debug.Log($"[Pickup] Equipped: {powerUpStrategy.Name}");
        other.gameObject.SetActive(false);
    }
}
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private IPowerUpStrategy _current = new NonePowerUp();
    [SerializeField] private float speed = 5f;

    public void SetStrategy(IPowerUpStrategy strategy)
    {
        _current = strategy ?? new NonePowerUp();
        Debug.Log($"Equipped Powerup: {_current.Name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        IPowerUpStrategy powerUp = other.GetComponent<IPowerUpStrategy>();
        if (powerUp == null) return;

        SetStrategy(powerUp);
        powerUp.PowerUp(); // Activar el power-up

        Debug.Log($"[Pickup] Equipped: {powerUp.Name}");

        // Destruir o desactivar el objeto del power-up
        other.gameObject.SetActive(false);
    }
}
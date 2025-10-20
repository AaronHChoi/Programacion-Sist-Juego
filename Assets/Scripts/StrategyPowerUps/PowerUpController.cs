using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private IPowerUpStrategy _current = new NonePowerUp();
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float despawnZ = -10f;
    [SerializeField] private float rotationSpeed = 100f;

    private bool isAttachedToPlayer = false;

    public void SetStrategy(IPowerUpStrategy strategy)
    {
        _current = strategy ?? new NonePowerUp();
        Debug.Log($"Equipped Powerup: {_current.Name}");
    }

    private void Start()
    {
        // If this GameObject has a Player tag, it's attached to the player
        isAttachedToPlayer = CompareTag("Player");
    }

    private void Update()
    {
        // Only move if NOT attached to player (i.e., it's a dropped power-up)
        if (!isAttachedToPlayer)
        {
            // Move in -Z axis toward player
            transform.position += Vector3.back * fallSpeed * Time.deltaTime;

            // Rotate for visual effect
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

            // Despawn if goes too far
            if (transform.position.z < despawnZ)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If this is a dropped power-up and player touches it
        if (!isAttachedToPlayer && other.CompareTag("Player"))
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
using Simulacro;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float despawnZ = -10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float powerUpDropChance = 0.3f;
    [SerializeField] private GameObject[] powerUpPrefabs;

    public void Init()
    {
        // Reset state or animation
    }

    void Update()
    {
        // En lugar de mover directamente, encolar el evento de movimiento
        if (EventQueue.Instance != null)
        {
            MoveEnemyEvent moveEvent = new MoveEnemyEvent(
                transform,
                Vector3.back,
                speed,
                Time.deltaTime
            );
            EventQueue.Instance.EnqueueEvent(moveEvent);
        }
        else
        {
            // Fallback si no hay EventQueue
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        }

        // Check despawn
        if (transform.position.z < despawnZ)
            gameObject.SetActive(false);
    }

    public void OnHit()
    {
        TrySpawnPowerUp();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            OnHit();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            gameObject.SetActive(false);
        }
    }

    private void TrySpawnPowerUp()
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
            return;

        if (Random.value <= powerUpDropChance)
        {
            GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
            GameObject powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }
}
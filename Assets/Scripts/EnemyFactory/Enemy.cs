using Simulacro;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float despawnZ = -10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float powerUpDropChance = 0.3f; // 30% chance to drop power-up
    [SerializeField] private GameObject[] powerUpPrefabs; // Assign your power-up sphere prefabs

    public void Init()
    {
        // Reset state or animation
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        if (transform.position.z < despawnZ)
            gameObject.SetActive(false);
    }

    public void OnHit()
    {
        // Spawn power-up before deactivating
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
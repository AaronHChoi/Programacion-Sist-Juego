using Simulacro;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float horizontalAmplitude = 2f;
    [SerializeField] private float horizontalFrequency = 2f;
    [SerializeField] private float despawnZ = -10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float powerUpDropChance = 0.3f;
    [SerializeField] private GameObject[] powerUpPrefabs;

    private float timeOffset;

    public void Init()
    {
        timeOffset = Random.value * Mathf.PI * 2f;
    }

    void Update()
    {
        if (EventQueue.Instance != null)
        {
            // Compute oscillation on X
            float horizontal = Mathf.Sin(Time.time * horizontalFrequency + timeOffset) * horizontalAmplitude;
            Vector3 moveDir = (Vector3.back + Vector3.right * horizontal).normalized;

            MoveEnemyEvent moveEvent = new MoveEnemyEvent(transform, moveDir, speed, Time.deltaTime);
            EventQueue.Instance.EnqueueEvent(moveEvent);
        }
        else
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        }

        if (transform.position.z < despawnZ)
            gameObject.SetActive(false);
    }

    public void OnHit()
    {
        TrySpawnPowerUp();
        GameManager.Instance?.OnEnemyKilled();
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
            if (other.TryGetComponent(out PlayerMovement player))
                player.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }

    private void TrySpawnPowerUp()
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0) return;
        if (Random.value <= powerUpDropChance)
        {
            var prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}

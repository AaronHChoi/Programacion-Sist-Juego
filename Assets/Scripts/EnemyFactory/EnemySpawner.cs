    using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 1.5f;
    [Header("Primary enemy type (default)")]
    public string enemyType = "BasicEnemy";

    [Header("Optional secondary enemy type used on Nivel 2")]
    [Tooltip("If set, on scene 'Nivel 2' the spawner will randomly spawn this type as well.")]
    public string enemyTypeB = "";

    [Range(0f, 1f)]
    [Tooltip("Probability to spawn enemyTypeB instead of enemyType when in Nivel 2.")]
    public float enemyTypeBProbability = 0.5f;

    public Vector2 spawnArea = new Vector2(10f, 5f);

    private float timer;
    private bool canSpawn = true;

    void Update()
    {
        if (!canSpawn) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        float x = Random.Range(-spawnArea.x, spawnArea.x);
        Vector3 pos = new Vector3(x, 0, 10f);

        string typeToSpawn = enemyType;

        // On Nivel 2, optionally spawn from a second enemy type according to probability
        if (!string.IsNullOrEmpty(enemyTypeB) && SceneManager.GetActiveScene().name == "Nivel 2")
        {
            if (Random.value < enemyTypeBProbability)
                typeToSpawn = enemyTypeB;
        }

        if (EnemyFactory.Instance == null)
        {
            Debug.LogWarning("EnemyFactory instance not found. Cannot spawn enemies.");
            return;
        }

        EnemyFactory.Instance.CreateEnemy(typeToSpawn, pos);
    }

    public void StopSpawning()
    {
        canSpawn = false;
        Debug.Log("Enemy spawning stopped!");
    }

    public void ResumeSpawning()
    {
        canSpawn = true;
        Debug.Log("Enemy spawning resumed!");
    }
}
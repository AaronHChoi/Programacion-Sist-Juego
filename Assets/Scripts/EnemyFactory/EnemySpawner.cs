using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 1.5f;
    public string enemyType = "BasicEnemy";
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
        EnemyFactory.Instance.CreateEnemy(enemyType, pos);
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
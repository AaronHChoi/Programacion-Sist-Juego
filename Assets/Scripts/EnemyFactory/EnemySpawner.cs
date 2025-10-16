using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 1.5f;
    public string enemyType = "BasicEnemy";
    public Vector2 spawnArea = new Vector2(10f, 5f); // ancho y profundidad

    private float timer;

    void Update()
    {
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
        Vector3 pos = new Vector3(x, 0, 10f); // 10f al frente de la cámara
        EnemyFactory.Instance.CreateEnemy(enemyType, pos);
    }
}


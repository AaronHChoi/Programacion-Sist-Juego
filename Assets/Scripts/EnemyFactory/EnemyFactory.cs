using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string enemyName;
        public GameObject prefab;
        public int size = 10;
    }

    [SerializeField] private List<Pool> pools;
    private readonly Dictionary<string, Queue<GameObject>> poolMap = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        poolMap.Clear();

        if (pools == null) return;

        foreach (var pool in pools)
        {
            var queue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            if (!poolMap.ContainsKey(pool.enemyName))
                poolMap.Add(pool.enemyName, queue);
        }
    }

    public GameObject CreateEnemy(string enemyName, Vector3 pos)
    {
        if (!poolMap.TryGetValue(enemyName, out var queue))
        {
            Debug.LogWarning($"No pool found for enemy: {enemyName}");
            return null;
        }

        GameObject enemy = null;
        int poolCount = queue.Count;
        for (int i = 0; i <= poolCount; i++)
        {
            var obj = queue.Dequeue();
            if (!obj.activeInHierarchy)
            {
                enemy = obj;
                break;
            }
            queue.Enqueue(obj);
        }

        if (enemy == null)
        {
            return null;
        }

        enemy.transform.position = pos;
        enemy.SetActive(true);

        if (enemy.TryGetComponent<IEnemy>(out var e))
            e.Init();

        queue.Enqueue(enemy);
        return enemy;
    }
}
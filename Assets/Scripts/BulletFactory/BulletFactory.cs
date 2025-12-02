using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public BulletData bulletData;
        public int size = 10;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        if (pools == null) return;

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.bulletData.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            if (!poolDictionary.ContainsKey(pool.bulletData.bulletName))
                poolDictionary.Add(pool.bulletData.bulletName, objectPool);
        }
    }

    public GameObject CreateBullet(BulletData data, Vector3 pos, Quaternion rot)
    {
        if (!poolDictionary.TryGetValue(data.bulletName, out var queue))
            return null;

        GameObject bullet = queue.Dequeue();

        if (bullet.activeInHierarchy)
        {
            queue.Enqueue(bullet);
            return null; // All busy
        }

        bullet.transform.SetPositionAndRotation(pos, rot);
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().Init(data);

        queue.Enqueue(bullet);

        return bullet;
    }
}
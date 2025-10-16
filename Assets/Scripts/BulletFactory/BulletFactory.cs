using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;

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
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.bulletData.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.bulletData.bulletName, objectPool);
        }
    }

    public GameObject CreateBullet(BulletData data, Vector3 pos, Quaternion rot)
    {
        var queue = poolDictionary[data.bulletName];
        GameObject bullet = queue.Dequeue();

        if (bullet.activeInHierarchy)
        {
            queue.Enqueue(bullet);
            return null; // Todas ocupadas
        }

        bullet.transform.SetPositionAndRotation(pos, rot);
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().Init(data);

        queue.Enqueue(bullet);

        return bullet;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "Scriptable Objects/BulletData")]
public class BulletData : ScriptableObject
{
    public string bulletName;
    public GameObject prefab;
    public float speed = 10f;
    public int damage = 1;
}

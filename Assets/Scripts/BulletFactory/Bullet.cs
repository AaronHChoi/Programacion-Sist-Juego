using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime = 2f;
    private float speed;
    
    public void Init(BulletData data)
    {
        speed = data.speed;
        Invoke(nameof(Deactivate), lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
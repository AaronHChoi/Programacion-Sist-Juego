using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float despawnZ = -10f;

    public void Init()
    {
        // Reinicia estado o animación
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        if (transform.position.z < despawnZ)
            gameObject.SetActive(false);
    }

    public void OnHit()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            OnHit();
            other.gameObject.SetActive(false);
        }
    }
}
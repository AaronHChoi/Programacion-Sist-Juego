using UnityEngine;

public class MoveDown : MonoBehaviour
{

    [SerializeField] private float fallSpeed = 20f;
    [SerializeField] private float despawnZ = -10f;
    [SerializeField] private float rotationSpeed = 100f;
    private bool isAttachedToPlayer = false;
    private void Update()
    {
        if (!isAttachedToPlayer)
        {
            transform.position += Vector3.back * fallSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

            if (transform.position.z < despawnZ)
            {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class MoveDown : MonoBehaviour
{

    [SerializeField] private float fallSpeed = 20f;
    [SerializeField] private float despawnZ = -10f;
    [SerializeField] private float rotationSpeed = 100f;
    private bool isAttachedToPlayer = false;
    private void Update()
    {
        // Only move if NOT attached to player (i.e., it's a dropped power-up)
        if (!isAttachedToPlayer)
        {
            // Move in -Z axis toward player
            transform.position += Vector3.back * fallSpeed * Time.deltaTime;

            // Rotate for visual effect
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

            // Despawn if goes too far
            if (transform.position.z < despawnZ)
            {
                Destroy(gameObject);
            }
        }
    }
}

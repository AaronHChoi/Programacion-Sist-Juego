using UnityEngine;

namespace _2.Factory
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
    
        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
        
            transform.Translate(horizontal * Time.deltaTime * speed, 0, vertical * Time.deltaTime * speed);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Attack();
            }
        }
    }
}

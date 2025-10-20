using UnityEngine;
namespace Simulacro
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;
        public BulletData bulletData;
        public Transform firePoint;
        public int health = 100;

        [SerializeField] private int enemyDamage = 10; // Damage per collision

        private PlayerState _currentState;

        void Start()
        {
            _currentState = new IdleState();
        }

        void Update()
        {
            _currentState.Handle(this);
        }

        public void SetState(PlayerState newState)
        {
            _currentState = newState;
        }

        public void TakeDamage(int damage)
        {
            health -= enemyDamage;
            Debug.Log($"Player hit! Health: {health}");

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died!");
            // Add death logic here
            gameObject.SetActive(false);
        }
    }
}
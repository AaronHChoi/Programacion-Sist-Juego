using UnityEngine;

namespace Simulacro
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;
        public BulletData bulletData;
        public Transform firePoint;
        public int health = 100;

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
        
    }
}
        
    


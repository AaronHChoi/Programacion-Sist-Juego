using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;
        public BulletData bulletData;
        public Transform firePoint;

        private PlayerState currentState;

        void Start()
        {
            currentState = new IdleState();
        }

        void Update()
        {
            currentState.Handle(this);
        }

        public void SetState(PlayerState newState)
        {
            currentState = newState;
        }
        
    }
        
    


using UnityEngine;

    public class MoveEnemyEvent : IGameEvent
    {
        private readonly Transform transform;
        private readonly Vector3 direction;
        private readonly float speed;
        private readonly float deltaTime;

        public MoveEnemyEvent(Transform transform, Vector3 direction, float speed, float deltaTime)
        {
            this.transform = transform;
            this.direction = direction;
            this.speed = speed;
            this.deltaTime = deltaTime;
        }

        public void Execute()
        {
            if (transform == null) return;
            transform.Translate(direction * speed * deltaTime, Space.World);
        }
    }
using UnityEngine;

namespace Memento
{
    public class PlayerMemento
    {
        public Vector3 Position { get; private set; }
        public int Health {get; private set;}

        public PlayerMemento(Vector3 position, int health)
        {
            this.Position = position;
            this.Health = health;
        }
    }
}

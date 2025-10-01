using UnityEngine;

namespace Memento
{
    public class PlayerMemento : MonoBehaviour
    {
        public Vector3 Position { get; private set; }
        public int Health {get; private set;}

        // Update is called once per frame
        public PlayerMemento(Vector3 position, int health)
        {
            this.Position = position;
            this.Health = health;
        }
    }
}

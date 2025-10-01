using UnityEngine;

namespace Memento
{
    public class Originator: MonoBehaviour

    {
        public PlayerMemento SaveState()
        {
            return new PlayerMemento(transform.position, health);
        }

        public void RestoreState(PlayerMemento memento)
        {
            transform.position = memento.Position;
            this.health = memento.Health;
        }
    }
}
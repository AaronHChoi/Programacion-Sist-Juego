using UnityEngine;
using Simulacro;

namespace Memento
{
    public class Originator : MonoBehaviour
    {
        private PlayerMovement _playerMovement;

        void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        public PlayerMemento SaveState()
        {
            return new PlayerMemento(transform.position, _playerMovement.health);
        }

        public void RestoreState(PlayerMemento memento)
        {
            transform.position = memento.Position;
            _playerMovement.health = memento.Health;
            Debug.Log($"Health restored to: {_playerMovement.health}");
        }
    }
}
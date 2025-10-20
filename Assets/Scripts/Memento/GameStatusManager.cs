using System.Collections.Generic;
using Simulacro;
using UnityEngine;
namespace Memento
{
    public class GameStatusManager : MonoBehaviour
    {
        private Stack<PlayerMemento> _savedStates = new Stack<PlayerMemento>();
        public Originator _player;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _savedStates.Push(_player.SaveState());
                Debug.Log("State Saved");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && _savedStates.Count > 0)
            {
                PlayerMemento lastSavedState = _savedStates.Pop();

                // Reactivate the player if it's disabled
                if (!_player.gameObject.activeSelf)
                {
                    _player.gameObject.SetActive(true);
                }

                _player.RestoreState(lastSavedState);
                Debug.Log("State Restored");
            }
        }
    }
}
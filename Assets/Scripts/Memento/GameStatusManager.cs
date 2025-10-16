using System.Collections.Generic;
using Simulacro;
using UnityEngine;

namespace Memento
{
    public class GameStatusManager: MonoBehaviour
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

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerMemento lastSavedState = _savedStates.Pop();
                _player.RestoreState(lastSavedState);
                Debug.Log("State Restored");
            }
        }
    }
}
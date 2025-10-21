using System.Collections.Generic;
using Simulacro;
using UnityEngine;

namespace Memento
{
    public class GameStatusManager : MonoBehaviour
    {
        private Stack<PlayerMemento> _savedStates = new Stack<PlayerMemento>();
        public Originator _player;

        [Header("Auto Save Settings")]
        [SerializeField] private bool autoSaveOnStart = true;
        [SerializeField] private float autoSaveDelay = 0.5f; 

        void Start()
        {
            if (autoSaveOnStart && _player != null)
            {
                Invoke(nameof(AutoSaveInitialState), autoSaveDelay);
            }
        }

        private void AutoSaveInitialState()
        {
            _savedStates.Push(_player.SaveState());
            Debug.Log("Initial State Auto-Saved at level start");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _savedStates.Push(_player.SaveState());
                Debug.Log($"State Saved manually (Total saves: {_savedStates.Count})");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && _savedStates.Count > 0)
            {
                PlayerMemento lastSavedState = _savedStates.Pop();

                if (!_player.gameObject.activeSelf)
                {
                    _player.gameObject.SetActive(true);
                }

                _player.RestoreState(lastSavedState);
                Debug.Log($"State Restored (Remaining saves: {_savedStates.Count})");
            }
        }

        public void SaveCurrentState()
        {
            if (_player != null)
            {
                _savedStates.Push(_player.SaveState());
                Debug.Log($"State Saved (Total saves: {_savedStates.Count})");
            }
        }

        public void ClearAllSaves()
        {
            _savedStates.Clear();
            Debug.Log("All saved states cleared");
        }

        public int GetSaveCount()
        {
            return _savedStates.Count;
        }
    }
}
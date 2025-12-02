using System.Collections;
using System.Collections.Generic;
using Simulacro;
using UnityEngine;

namespace Memento
{
    public class GameStatusManager : MonoBehaviour
    {
        private Stack<PlayerMemento> _savedStates = new Stack<PlayerMemento>();
        public Originator _player;
        private PlayerMovement _playerMovement;

        [Header("Auto Save Settings")]
        [SerializeField] private bool autoSaveOnStart = true;
        [SerializeField] private readonly float autoSaveDelay = 0.5f;

        void Awake()
        {
            if (_player == null)
            {
                _player = FindFirstObjectByType<Originator>();
            }
            EnsurePlayerMovementReference();
        }

        void OnEnable()
        {
            EnsurePlayerMovementReference();
            if (_playerMovement != null)
            {
                _playerMovement.OnDied += HandlePlayerDeath;
            }
        }

        private void EnsurePlayerMovementReference()
        {
            if (_playerMovement == null && _player != null)
            {
                _playerMovement = _player.GetComponent<PlayerMovement>();
            }
        }

        void OnDisable()
        {
            if (_playerMovement != null)
            {
                _playerMovement.OnDied -= HandlePlayerDeath;
            }
        }

        void Start()
        {
            if (_player == null)
            {
                Debug.LogWarning("GameStatusManager: Originator (player) not assigned.");
                return;
            }

            if (autoSaveOnStart)
            {
                // Save an initial checkpoint at level start
                if (_savedStates.Count == 0)
                {
                    _savedStates.Push(_player.SaveState());
                    Debug.Log("Initial State Auto-Saved at level start");
                }
            }
            else if (_savedStates.Count == 0)
            {
                // Ensure at least one checkpoint exists even when auto-save is off
                _savedStates.Push(_player.SaveState());
                Debug.Log("Initial State Saved (autoSave disabled)");
            }
        }

        private void HandlePlayerDeath()
        {
            // Derrota inmediata al llegar a 0 de vida. No usar contador de saves.
            GameManager.Instance?.LoseLevel();
        }

        private IEnumerator RestoreAfterDeathNextFrame()
        {
            if (_player == null || _savedStates.Count == 0)
                yield break;

            PlayerMemento lastSavedState = _savedStates.Peek(); // keep the checkpoint
            yield return null; // wait one frame

            if (!_player.gameObject.activeSelf)
            {
                _player.gameObject.SetActive(true);
            }

            _player.RestoreState(lastSavedState);
            Debug.Log($"State Restored after death (Saves kept: {_savedStates.Count})");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && _player != null)
            {
                _savedStates.Push(_player.SaveState());
                Debug.Log($"State Saved manually (Total saves: {_savedStates.Count})");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && _savedStates.Count > 0)
            {
                PlayerMemento lastSavedState = _savedStates.Pop();

                if (_player != null && !_player.gameObject.activeSelf)
                {
                    _player.gameObject.SetActive(true);
                }

                _player?.RestoreState(lastSavedState);
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
using System.Collections.Generic;
using UnityEngine;

    public interface IGameEvent
    {
        void Execute();
    }

    public class EventQueue : MonoBehaviour
    {
        public static EventQueue Instance { get; private set; }

        private readonly Queue<IGameEvent> _queue = new();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void EnqueueEvent(IGameEvent gameEvent)
        {
            _queue.Enqueue(gameEvent);
        }

        void Update()
        {
            while (_queue.Count > 0)
            {
                _queue.Dequeue().Execute();
            }
        }
    }


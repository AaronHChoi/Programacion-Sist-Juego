using System;
using System.Collections.Generic;
using UnityEngine;

// Event base class
public abstract class GameEvent
{
    public abstract void Execute();
}

// Specific movement event
public class MoveEnemyEvent : GameEvent
{
    private readonly Transform enemyTransform;
    private readonly Vector3 direction;
    private readonly float speed;
    private readonly float deltaTime;

    public MoveEnemyEvent(Transform transform, Vector3 direction, float speed, float deltaTime)
    {
        this.enemyTransform = transform;
        this.direction = direction;
        this.speed = speed;
        this.deltaTime = deltaTime;
    }

    public override void Execute()
    {
        if (enemyTransform != null && enemyTransform.gameObject.activeInHierarchy)
        {
            enemyTransform.Translate(direction * speed * deltaTime, Space.World);
        }
    }
}

// Event Queue Manager
public class EventQueue : MonoBehaviour
{
    public static EventQueue Instance { get; private set; }

    private Queue<GameEvent> eventQueue = new Queue<GameEvent>();
    private const int MAX_EVENTS_PER_FRAME = 100;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnqueueEvent(GameEvent gameEvent)
    {
        eventQueue.Enqueue(gameEvent);
    }

    void Update()
    {
        ProcessEvents();
    }

    private void ProcessEvents()
    {
        int eventsProcessed = 0;

        while (eventQueue.Count > 0 && eventsProcessed < MAX_EVENTS_PER_FRAME)
        {
            GameEvent gameEvent = eventQueue.Dequeue();
            gameEvent.Execute();
            eventsProcessed++;
        }
    }

    public int GetQueueSize()
    {
        return eventQueue.Count;
    }
}
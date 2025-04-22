using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementDetector : MonoBehaviour
{
    public static MovementDetector instance;

    public UnityEvent<Direction> onPlayerMoveEvents;

    private void Awake()
    {
        instance = this;
    }

    public void Subscribe(UnityAction<Direction> listener)
    {
        onPlayerMoveEvents.AddListener(listener);
    }

    public void Unsubscribe(UnityAction<Direction> listener)
    {
        onPlayerMoveEvents.RemoveListener(listener);
    }

    public void TriggerPlayerMovedEvents(Direction direction)
    {
        AudioManager.instance.PlaySound("Move");
        onPlayerMoveEvents?.Invoke(direction);
    }
}

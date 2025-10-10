using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Event/GameEvent")]
public class GameEvent : ScriptableObject
{
    private List<Action> _listeners = new();

    public void Raise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].Invoke();
        }
    }

    public void Register(Action action)
    {
        if (!_listeners.Contains(action))
            _listeners.Add(action);
    }

    public void Unregister(Action action)
    {
        _listeners.Remove(action);
    }
}

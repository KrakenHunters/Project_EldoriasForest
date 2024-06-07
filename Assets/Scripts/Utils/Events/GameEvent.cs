using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent<T> : ScriptableObject
{
    private List<GameEventListener<T>> listeners = new();

    public void Raise(T value)
    {
        foreach (var listener in listeners)
        {
            listener.OnEventRaised(value);
        }
    }

    public void RegisterListener(GameEventListener<T> listener) => listeners.Add(listener);

    public void UnregisterListener(GameEventListener<T> listener) => listeners.Remove(listener);
}



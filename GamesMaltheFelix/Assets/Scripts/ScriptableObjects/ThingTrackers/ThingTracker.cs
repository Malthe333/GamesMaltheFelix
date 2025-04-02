using UnityEngine;
using System.Collections.Generic;

public class ThingTracker<T> : ScriptableObject
{
    [HideInInspector] public List<T> tracked { get; private set; } = new List<T>();

    public virtual void AddTracked(T listener)
    {
        tracked.Add(listener);
    }

    public virtual void RemoveTracked(T listener)
    {
        tracked.Remove(listener);
    }

    public virtual void ListenerEvent()
    {
        Debug.Log($"Trying to invoke from {name} with no assigned ListenerEvents");
    }
}

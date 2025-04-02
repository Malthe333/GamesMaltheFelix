using UnityEngine;

[CreateAssetMenu(fileName = "TransformTracker", menuName = "Scriptable Objects/TransformTracker")]
public class TransformTracker : ThingTracker<Transform>
{
    public override void AddTracked(Transform listener)
    {
        base.AddTracked(listener);
        Debug.Log($"{listener.name} transform tracked! There are {tracked.Count} Transforms tracked by {name}");
    }

    public override void RemoveTracked(Transform listener) 
    {  
        base.RemoveTracked(listener);
        Debug.Log($"{listener.name} transform no longer tracked! There are {tracked.Count} Transforms left tracked by {name}");
    }
}


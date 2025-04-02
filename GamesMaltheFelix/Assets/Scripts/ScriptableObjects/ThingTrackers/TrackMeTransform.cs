using UnityEngine;

public class TrackMeTransform : MonoBehaviour
{
    [SerializeField] private TransformTracker tracker;

    private void Awake()
    {
        if (tracker == null)
        {
            Debug.LogWarning($"{name} does not have a tracker assigned");
        }
    }

    private void OnEnable()
    {
        tracker?.AddTracked(transform);
    }

    private void OnDisable()
    {
        tracker?.RemoveTracked(transform);
    }
}

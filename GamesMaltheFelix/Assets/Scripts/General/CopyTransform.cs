using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] private Transform transToCopy;
    [SerializeField] private bool copyPosition = false;
    [SerializeField] private bool copyRotation = false;
    [SerializeField] private bool copyScale = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transToCopy == null)
        {
            Debug.LogWarning($"{name} has CopyTransform on it but no assigned Transform to copy");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (copyPosition)
            transform.position = transToCopy.position;
        if (copyRotation)
            transform.rotation = transToCopy.rotation;
        if (copyScale)
            transform.localScale = transToCopy.localScale;
    }
}

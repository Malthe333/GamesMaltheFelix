using UnityEngine;

public class RotateCameraRecorder : MonoBehaviour
{
    public Transform target;           // The object to rotate around
    public float distance = 5.0f;      // Distance from the target
    public float height = 2.0f;        // Height offset from the target
    public float rotationSpeed = 30.0f; // Degrees per second

    private float currentAngle = 0.0f;

    void LateUpdate()
    {
        if (target == null) return;

        // Update rotation angle over time
        currentAngle += rotationSpeed * Time.deltaTime;
        currentAngle %= 360f; // Keep it within 0-360 for neatness

        // Calculate new position
        float radians = currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians) * distance;
        float z = Mathf.Sin(radians) * distance;

        Vector3 newPos = new Vector3(x, height, z) + target.position;

        // Update camera position and make it look at the target
        transform.position = newPos;
        transform.LookAt(target);
    }
}

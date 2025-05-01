using UnityEngine;
using System.Collections;

public class CameraZoomIn : MonoBehaviour
{
    
    [Header("Material to Light Up")]
    public Material maskMaterial;

    [Header("Material Fade Settings")]
    [SerializeField, ColorUsage(true, true)] private Color offMainColor = Color.gray1;
    [SerializeField, ColorUsage(true, true)] private Color onMainColor = Color.red;
    [SerializeField, ColorUsage(true, true)] private Color offGridColor = Color.gray3;
    [SerializeField, ColorUsage(true, true)] private Color onGridColor = Color.yellow;
    [SerializeField] private float fadeDuration = 2f;

    private readonly string colorPropertyName = "_Main_Color";
    private readonly string gridPropertyName = "_Grid_Color";
    private readonly string gridMovementName = "_Grid_Movement";

    [Header("Camera Settings")]
    public GameObject objectToMove;      // The GameObject that will be moved
    public Transform targetDestination;  // The Transform it should move to
    public float moveDuration = 2f;      // Time (in seconds) to reach the destination

    private bool isMoving = false;

    void Start()
    {
        // Turn off color
        maskMaterial.SetColor(colorPropertyName, offMainColor);
        maskMaterial.SetColor(gridPropertyName, offGridColor);
    }

    public void StartTheMovement()
    {
        StartCoroutine(MoveCameraTowardsRobot());
    }


    public IEnumerator LightUpMaterial()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            maskMaterial.SetColor(colorPropertyName, Color.Lerp(offMainColor, onMainColor, t));
            maskMaterial.SetColor(gridPropertyName, Color.Lerp(offGridColor, onGridColor, t));
            yield return null;
        }

        maskMaterial.SetColor(colorPropertyName, onMainColor);
        maskMaterial.SetColor(gridPropertyName, onGridColor);
    }

    public IEnumerator MoveCameraTowardsRobot()
    {
        yield return new WaitForSeconds(1.5f); // Optional delay before starting the movement
        isMoving = true;
        Vector3 startPos = objectToMove.transform.position;
        Vector3 endPos = targetDestination.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            objectToMove.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        StartCoroutine(LightUpMaterial());

        objectToMove.transform.position = endPos;
        isMoving = false;
    }
}

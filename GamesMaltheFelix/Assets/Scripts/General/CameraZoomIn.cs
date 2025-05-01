using UnityEngine;

public class CameraZoomIn : MonoBehaviour
{
    
    [Header("Material to Light Up")]
    public Material maskMaterial;

    [Header("Material Fade Settings")]
     public float fadeDuration = 2f;

     private readonly string colorPropertyName = "_Main_Color";

    [Header("Camera Settings")]
    public GameObject objectToMove;      // The GameObject that will be moved
    public Transform targetDestination;  // The Transform it should move to
    public float moveDuration = 2f;      // Time (in seconds) to reach the destination

    private bool isMoving = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTheMovement()
    {
        
            StartCoroutine(MoveCameraTowardsRobot());
            StartCoroutine(LightUpMaterial());
            Color color = maskMaterial.GetColor(colorPropertyName);
            color.a = 0f;
            maskMaterial.SetColor(colorPropertyName, color);
        
    }


    public System.Collections.IEnumerator LightUpMaterial()
    {

        Color color = maskMaterial.GetColor(colorPropertyName);
        float startAlpha = 0f;
        float endAlpha = 1f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            maskMaterial.SetColor(colorPropertyName, color);
            yield return null;
        }

        color.a = endAlpha;
        maskMaterial.SetColor(colorPropertyName, color);

        
    }

    public System.Collections.IEnumerator MoveCameraTowardsRobot()
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

        objectToMove.transform.position = endPos;
        isMoving = false;
    }
}

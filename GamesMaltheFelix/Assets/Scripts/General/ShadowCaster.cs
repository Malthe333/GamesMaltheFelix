using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
    [SerializeField] private Material shadowMaterial;
    [SerializeField] private LayerMask layersToCastShadowOn = 64;
    private Transform shadowCarrier;

    private bool spaghettiShadowCheck = false;

    private void Awake()
    {
        if (shadowMaterial == null)
        {
            Debug.LogWarning($"{name} has unassigned shadow material in it's ShadowCaster component. Removing ShadowCaster to avoid unnecessary computation");
            Destroy(this);
        }

        shadowCarrier = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
        Destroy(shadowCarrier.GetComponent<Collider>());
        shadowCarrier.SetParent(transform);
        shadowCarrier.name = "Shadow";
        shadowCarrier.GetComponent<Renderer>().material = shadowMaterial;
    }

    private void Update()
    {
        // Raycast is a query against the current state of the physics scene, so it's okay to call in update. Everything it's used for is graphics, so this is preferred in this case
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 20f, layersToCastShadowOn))
        {
            if (spaghettiShadowCheck)
            {
                spaghettiShadowCheck = false;
                shadowCarrier.gameObject.SetActive(true);
            }
            shadowCarrier.position = hit.point + 0.01f * hit.normal;
            shadowCarrier.rotation = Quaternion.LookRotation(-hit.normal);
        }
        else
        {
            if (spaghettiShadowCheck) return;
            spaghettiShadowCheck = true;
            shadowCarrier.gameObject.SetActive(false);
        }
    }
}

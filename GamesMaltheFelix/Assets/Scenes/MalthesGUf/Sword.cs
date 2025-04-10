using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField, Min(0)] private int damageAmount = 1;
    [SerializeField] private string weakspotTag = "Damageable";

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(weakspotTag))

        {
            if(other.TryGetComponent(out Health health))
            {
                health.Damage(damageAmount);
                Debug.Log("Ramte");
            }
            else
            {
                Debug.LogWarning("Ikke damageable");
            }
            
            
            
        }
    }
}
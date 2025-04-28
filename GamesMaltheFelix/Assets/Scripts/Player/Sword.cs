using UnityEngine;
using System.Collections.Generic;

public class Sword : MonoBehaviour
{
    [SerializeField, Min(0)] private int damageAmount = 1;
    [SerializeField] private string weakspotTag = "Damageable";

    private List<Collider> thingsHitThisAttack = new List<Collider>();

    private void OnDisable()
    {
        thingsHitThisAttack.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Don't hit same thing multiple times
        if (thingsHitThisAttack.Contains(other)) return;
        thingsHitThisAttack.Add(other);

        //Debug.Log($"{other.name} entered trigger, its tag is {other.tag}, which is {(other.CompareTag(weakspotTag) ? "the same as" : "different from")} {weakspotTag}");

        if (other.CompareTag(weakspotTag))
        {
            if (other.TryGetComponent(out Health health))
            {
                health.Damage(damageAmount);
                Debug.Log("Ramte");
            }
            else
            {
                Debug.LogWarning($"{other.name} is tagged {weakspotTag}, but does not carry a Health component");
            }
        }
    }
}
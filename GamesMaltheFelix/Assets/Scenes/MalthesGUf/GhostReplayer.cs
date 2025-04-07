using UnityEngine;
using System.Collections.Generic; //siden vi bruger List<>

public class GhostReplayer : MonoBehaviour
{
    // Dette script skal bruges til at afspille en ghost replay af spilleren
    public List<PlayerFrameData> playbackData; //Skal den her væres static?
    private int currentIndex = 0; // Holder styr på hvilken frame vi er på i playbackData, aka spillerens "recording"
    void FixedUpdate() // Bruger FixedUpdate for at matche frameraten på rigidbodiens bevægelse
    {
        if (currentIndex >= playbackData.Count)
        {
            Destroy(gameObject);
            return; // Stopper resten af scriptet i at køre så vi ikke får en List fejl
        }

        var data = playbackData[currentIndex]; // Tager en frame fra playbackData listen og gemmer den i "data" variablen. Super sejt do
        transform.position = data.position;
        transform.rotation = data.rotation;

        if (data.didAttack)
        {
            Debug.Log("Angreb!");
            // Her kan vi tilføje ting
        }

        currentIndex++; // Går til næste frame af playbackData fra Spilleren
    }
}

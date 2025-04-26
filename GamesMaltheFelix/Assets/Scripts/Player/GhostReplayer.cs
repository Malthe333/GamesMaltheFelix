using UnityEngine;
using System.Collections.Generic; //siden vi bruger List<>

public class GhostReplayer : MonoBehaviour
{
    // Dette script skal bruges til at afspille en ghost replay af spilleren
    [SerializeField] private Transform rigToMove;

    [HideInInspector] public List<PlayerFrameData> playbackData; //Skal den her væres static?
    private int currentIndex = 0; // Holder styr på hvilken frame vi er på i playbackData, aka spillerens "recording"

    [SerializeField] private Animator myAnimator; // Animatoren på spøgelset der tracker attacket.


    void Start()
    {
        if (myAnimator == null)
        {
            if (!TryGetComponent(out myAnimator))
                Debug.LogWarning($"Ghost {name} had no animator assigned and could not find any either. It will not function correctly");
        }

        if (rigToMove == null)
            rigToMove = transform;
    }
    void FixedUpdate() // Bruger FixedUpdate for at matche frameraten på rigidbodiens bevægelse
    {
        if (currentIndex >= playbackData.Count)
        {
            Destroy(gameObject); // Ødelægger spøgelset når vi er færdige med at afspille det.
            return; // Stopper resten af scriptet i at køre så vi ikke får en List fejl
        }

        var data = playbackData[currentIndex]; // Tager en frame fra playbackData listen og gemmer den i "data" variablen. Super sejt do
        rigToMove.position = data.position; // Sætter positionen på spøgelset til den position der er gemt i data variablen
        rigToMove.rotation = data.rotation; // Sætter rotationen på spøgelset til den rotation der er gemt i data variablen

        if (data.didAttack)
        {
            myAnimator.SetTrigger("Attack"); // Sætter triggeren til at spille angrebs animationen
        }
        

        currentIndex++; // Går til næste frame af playbackData fra Spilleren
    }
}

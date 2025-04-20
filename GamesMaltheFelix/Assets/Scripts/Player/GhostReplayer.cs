using UnityEngine;
using System.Collections.Generic; //siden vi bruger List<>

public class GhostReplayer : MonoBehaviour
{
    // Dette script skal bruges til at afspille en ghost replay af spilleren

    [Header("List over frames")]
    public List<PlayerFrameData> playbackData; //Skal den her væres static?
    private int currentIndex = 0; // Holder styr på hvilken frame vi er på i playbackData, aka spillerens "recording"

    private bool isAttacking = false;
    AnimatorStateInfo previousState; // En variable til at gemme den nuværende animation så vi kan gå tilbage til den når angrebet er færdigt
    private Animator animator; // Animatoren på spøgelset der tracker attacket.


    void Start()
    {
        animator = GetComponent<Animator>(); // Henter animatoren fra spøgelset så vi kan styre den
    }
    void FixedUpdate() // Bruger FixedUpdate for at matche frameraten på rigidbodiens bevægelse
    {
        if (currentIndex >= playbackData.Count)
        {
            Destroy(gameObject); // Ødelægger spøgelset når vi er færdige med at afspille det.
            return; // Stopper resten af scriptet i at køre så vi ikke får en List fejl
        }

        var data = playbackData[currentIndex]; // Tager en frame fra playbackData listen og gemmer den i "data" variablen. Super sejt do
        transform.position = data.position; // Sætter positionen på spøgelset til den position der er gemt i data variablen
        transform.rotation = data.rotation; // Sætter rotationen på spøgelset til den rotation der er gemt i data variablen

        if (data.didAttack)
        {
            animator.SetTrigger("Attack"); // Sætter triggeren til at spille angrebs animationen
        }
        

        currentIndex++; // Går til næste frame af playbackData fra Spilleren
    }
}

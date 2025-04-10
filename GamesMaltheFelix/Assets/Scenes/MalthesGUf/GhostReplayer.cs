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

        if (data.didAttack && !isAttacking)
{
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName("Attack")) // Don't trigger if already attacking
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
}

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0); // checker omkring hvilken animation vi er på i animatoren
        if (isAttacking && !currentState.IsName("Attack")) // Hvis vi angriber og animationen ikke er "Attack" så starter vi den animation
        {
            previousState = currentState; // Gemmer den nuværende animation så vi kan gå tilbage til den når angrebet er færdigt    
            animator.SetTrigger("Attack"); // Triggerer angrebsanimationen
        }
        if (currentState.IsName("Attack") && currentState.normalizedTime >= 1.0f) // Hvis animationen er færdig og den er på "Attack" animationen så sætter vi isAttacking til false
        {
            animator.Play(previousState.fullPathHash, 0, 0);
            isAttacking = false; // Sætter boolen til false når animationen er færdig så den ik bliver ved med at køre, samt den ikke kan spammes
        }

        currentIndex++; // Går til næste frame af playbackData fra Spilleren
    }
}

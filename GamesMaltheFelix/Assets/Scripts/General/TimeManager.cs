using UnityEngine;
using TMPro;
using System;

public class TimerManager : MonoBehaviour
{
    public float maxTime = 20f;
    private float currentTime;

    private bool isRunning = true;
    public TextMeshProUGUI timerText;

    public delegate void TimerEndAction(); // Definerer en delegate til at håndtere timerens slutning.
    public event Action OnTimerEnd; 
    void Start()
    {
        currentTime = maxTime;
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(0f, currentTime);

            if (timerText != null)
                timerText.text = currentTime.ToString("F1");

            if (currentTime <= 0f)
            {
                isRunning = false;
                OnTimerEnd?.Invoke(); // Only triggers once
            }
        }
    }

    public void ResetTimer()
    {
        currentTime = maxTime;
        isRunning = true; // Resetter timeren til at køre igen.
    }

    public float GetTimeRemaining() // Et script til hvis vi skal bruge tiden til noget andet end at vise den på skærmen idk hvad.
    {
        return currentTime;
    }



    public void PauseTimer()
    {
        isRunning = false;
    }


    public void ResumeTimer()
    {
        isRunning = true;
    }




}

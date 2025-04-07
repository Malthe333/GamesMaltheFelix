using UnityEngine;
using TMPro;
using System;

public class TimerManager : MonoBehaviour
{
    public float maxTime = 20f;
    private float currentTime;
    public TextMeshProUGUI timerText;
    public event Action OnTimerEnd; 
    void Start()
    {
        currentTime = maxTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0f, currentTime);

        if (timerText != null)
            timerText.text = currentTime.ToString("F1");

        if (currentTime <= 0f)
        {
            OnTimerEnd?.Invoke(); // Trigger eventen, men kun hvis den er subscribed til noget tror jeg.
            ResetTimer();
        }
    }

    public void ResetTimer()
    {
        currentTime = maxTime;
    }

    public float GetTimeRemaining() // Et script til hvis vi skal bruge tiden til noget andet end at vise den på skærmen idk hvad.
    {
        return currentTime;
    }
}

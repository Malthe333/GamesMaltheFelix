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
            OnTimerEnd?.Invoke(); // Fire the event
            ResetTimer();
        }
    }

    public void ResetTimer()
    {
        currentTime = maxTime;
    }

    public float GetTimeRemaining()
    {
        return currentTime;
    }
}

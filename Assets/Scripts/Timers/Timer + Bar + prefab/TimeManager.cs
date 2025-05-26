using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Time Settings")]
    [SerializeField] private float maxTime = 10f;
    [SerializeField] private float countdownDuration = 10f; // Time in seconds to fully count down

    private float currentTime;
    private float countdownRate; // Time lost per second

    public float GetNormalizedTime() => currentTime / maxTime;

    private void Awake()
    {
        Instance = this;
        currentTime = maxTime;
        CalculateCountdownRate();
    }

    private void CalculateCountdownRate()
    {
        countdownRate = maxTime / countdownDuration;
    }

    public void SetCountdownDuration(float duration)
    {
        countdownDuration = Mathf.Max(0.1f, duration); // Minimum 0.1 seconds to avoid division by zero
        CalculateCountdownRate();
    }

    public void ModifyTime(float amount)
    {
        currentTime = Mathf.Clamp(currentTime + amount, 0f, maxTime);
    }

    private void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= countdownRate * Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0f); // Ensure it doesn't go below 0
        }
    }

    // Additional helper methods
    public bool IsTimeUp() => currentTime <= 0f;
    public float GetCurrentTime() => currentTime;
    public void ResetTime() => currentTime = maxTime;
}

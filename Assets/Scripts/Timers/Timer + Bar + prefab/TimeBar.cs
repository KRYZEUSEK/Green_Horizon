using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private Image totalTimeBar;
    [SerializeField] private Image currentTimeBar;

    private void Start()
    {
        if (timeManager == null)
        {
            Debug.LogError("Time Manager reference not set in TimeBar");
            return;
        }

        // Initialize both bars to full
        totalTimeBar.fillAmount = 1f;
        currentTimeBar.fillAmount = 1f;
    }

    private void Update()
    {
        if (timeManager == null) return;

        // Update the current time bar to match the remaining time
        currentTimeBar.fillAmount = timeManager.GetNormalizedTime();
    }
}


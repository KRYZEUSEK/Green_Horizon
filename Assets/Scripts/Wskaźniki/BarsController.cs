using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarsController : MonoBehaviour
{
    [Header("Infrastructure")]
    [SerializeField] private Image totalInfrastructure; // obraz będący tłem dla wskaźnika 
    [SerializeField] private Image currentInfrastructure; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxInfrastructure = 100f; // Maksymalna wartość infrastruktury

    [Header("Budget")]
    [SerializeField] private Image totalBudget; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentBudget; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxBudget = 100f; // Maksymalna wartość budżetu

    [Header("LifeQuality")]
    [SerializeField] private Image totalLifeQuality; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentLifeQuality; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxLifeQuality = 100f; // Maksymalna wartość budżetu

    [Header("Ecology")]
    [SerializeField] private Image totalEcology; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentEcology; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxEcology = 100f; // Maksymalna wartość 


    [Header("Order")]
    [SerializeField] private Image totalOrder; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentOrder; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxOrder = 100f; // Maksymalna wartość 

    public void UpdateBars() 
    {
        float infrastructure = (float)GameManager.Instance.Infrastructure;
        float budget = (float)GameManager.Instance.Budget;
        float satisfaction = (float)GameManager.Instance.Satisfaction;
        float environment = (float)GameManager.Instance.Environment;
        float order = (float)GameManager.Instance.Order;

        currentInfrastructure.fillAmount = (infrastructure / 100f); //jeżeli zmienimy bazową max wartość wskaźnika tutaj należy przez nią podzielić
        currentBudget.fillAmount = (budget / 100f);
        currentLifeQuality.fillAmount = (satisfaction / 100f);
        currentEcology.fillAmount = (environment / 100f);
        currentOrder.fillAmount = (order / 100f);

        UpdateBarColor(currentInfrastructure, infrastructure, maxInfrastructure);
        UpdateBarColor(currentBudget, budget, maxBudget);
        UpdateBarColor(currentLifeQuality, satisfaction, maxLifeQuality);
        UpdateBarColor(currentEcology, environment, maxEcology);
        UpdateBarColor(currentOrder, order, maxOrder);
    }

    private void UpdateBarColor(Image bar, float currentValue, float maxValue)
        // funkcja zapewniającą dynamiczną zmianę koloru od zielonego do czerwonego w zależności od wypełnienia paska 
    {

        // Normalizacja wartości do zakresu [0, 1]
        float normalizedValue = currentValue / maxValue;

        // Kolory krańcowe
        Color greenColor = Color.green;
        Color yellowColor = Color.yellow;
        Color redColor = Color.red;

        // Interpolacja w dwóch etapach
        Color interpolatedColor;

        if (normalizedValue > 0.5f)
        {
            // Zielony → Żółty
            float t = (normalizedValue - 0.5f) * 2; // Zakres [0.5, 1] → [0, 1]
            interpolatedColor = Color.Lerp(yellowColor, greenColor, t);
        }
        else
        {
            // Żółty → Czerwony
            float t = normalizedValue * 2; // Zakres [0, 0.5] → [0, 1]
            interpolatedColor = Color.Lerp(redColor, yellowColor, t);
        }

        // Ustawienie koloru i wypełnienia paska
        bar.color = interpolatedColor;
    }
}
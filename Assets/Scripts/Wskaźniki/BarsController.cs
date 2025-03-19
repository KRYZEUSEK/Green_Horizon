using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarsController : MonoBehaviour
{
    [Header("Infrastructure")]
    [SerializeField] private Infrastructure playerInfrastructure; 
    [SerializeField] private Image totalInfrastructure; // obraz będący tłem dla wskaźnika 
    [SerializeField] private Image currentInfrastructure; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxInfrastructure = 100f; // Maksymalna wartość infrastruktury


    [Header("Budget")]
    [SerializeField] private Budget playerBudget;
    [SerializeField] private Image totalBudget; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentBudget; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxBudget = 100f; // Maksymalna wartość budżetu
    Color defaultColor;

    [Header("LifeQuality")]
    [SerializeField] private LifeQuality playerLifeQuality;
    [SerializeField] private Image totalLifeQuality; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentLifeQuality; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxLifeQuality = 100f; // Maksymalna wartość budżetu

    [Header("Ecology")]
    [SerializeField] private Ecology playerEcology;
    [SerializeField] private Image totalEcology; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentEcology; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxEcology = 100f; // Maksymalna wartość 


    [Header("Order")]
    [SerializeField] private Order playerOrder;
    [SerializeField] private Image totalOrder; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentOrder; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private float maxOrder = 100f; // Maksymalna wartość 


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

        void Update()
        {

        // tylko do sprawdzania czy działa 
        if (Input.GetKeyDown(KeyCode.U))
        {
            playerInfrastructure.InfrastructureUp(10);
            playerBudget.BudgetUp(10);
            playerLifeQuality.LifeQualityUp(10);
            playerEcology.EcologyUp(10);
            playerOrder.OrderUp(10);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            playerInfrastructure.InfrastructureDown(10);
            playerBudget.BudgetDown(10);
            playerLifeQuality.LifeQualityDown(10);
            playerEcology.EcologyDown(10);
            playerOrder.OrderDown(10);
        }

        currentInfrastructure.fillAmount = playerInfrastructure.currentInfrastructure / 100; //jeżeli zmienimy bazową max wartość wskaźnika tutaj należy przez nią podzielić
        currentBudget.fillAmount = playerBudget.currentBudget / 100;
        currentLifeQuality.fillAmount = playerLifeQuality.currentLifeQuality / 100;
        currentEcology.fillAmount = playerEcology.currentEcology / 100;
        currentOrder.fillAmount = playerOrder.currentOrder / 100;
        UpdateBarColor(currentInfrastructure, playerInfrastructure.currentInfrastructure, maxInfrastructure);
        UpdateBarColor(currentBudget, playerBudget.currentBudget, maxBudget);
        UpdateBarColor(currentLifeQuality, playerLifeQuality.currentLifeQuality, maxLifeQuality);
        UpdateBarColor(currentEcology, playerEcology.currentEcology, maxEcology);
        UpdateBarColor(currentOrder, playerOrder.currentOrder, maxOrder);
    }
    }
    


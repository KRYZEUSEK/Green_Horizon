using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarsController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float arrowsDisplayTime = 1f; // Czas wyświetlania strzałek po zmianie wartości wskaźnika

    [Header("Infrastructure")]
    [SerializeField] private Image totalInfrastructure; // obraz będący tłem dla wskaźnika 
    [SerializeField] private Image currentInfrastructure; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private GameObject infUp;
    [SerializeField] private GameObject infDown;
    [SerializeField] private float maxInfrastructure = 100f; // Maksymalna wartość infrastruktury

    [Header("Budget")]
    [SerializeField] private Image totalBudget; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentBudget; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private GameObject budUp;
    [SerializeField] private GameObject budDown;
    [SerializeField] private float maxBudget = 100f; // Maksymalna wartość budżetu

    [Header("LifeQuality")]
    [SerializeField] private Image totalLifeQuality; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentLifeQuality; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private GameObject satUp;
    [SerializeField] private GameObject satDown;
    [SerializeField] private float maxLifeQuality = 100f; // Maksymalna wartość budżetu

    [Header("Ecology")]
    [SerializeField] private Image totalEcology; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentEcology; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private GameObject envUp;
    [SerializeField] private GameObject envDown;
    [SerializeField] private float maxEcology = 100f; // Maksymalna wartość 


    [Header("Order")]
    [SerializeField] private Image totalOrder; // obraz będący tłem dla wskaźnika
    [SerializeField] private Image currentOrder; // obraz który jest zmieniany w tej klasie 
    [SerializeField] private GameObject ordUp;
    [SerializeField] private GameObject ordDown;
    [SerializeField] private float maxOrder = 100f; // Maksymalna wartość 

    private List<GameObject> selectedArrows = new List<GameObject>();

    public void UpdateBars(bool showChanges = true) 
    {
        float infrastructure = (float)(GameManager.Instance.Infrastructure / 100f);
        float budget = (float)(GameManager.Instance.Budget / 100f);
        float satisfaction = (float)(GameManager.Instance.Satisfaction / 100f);
        float environment = (float)(GameManager.Instance.Environment / 100f);
        float order = (float)(GameManager.Instance.Order / 100f);

        if (showChanges) {
            DisplayChanges(infrastructure - currentInfrastructure.fillAmount, infUp, infDown);
            DisplayChanges(budget - currentBudget.fillAmount, budUp, budDown);
            DisplayChanges(satisfaction - currentLifeQuality.fillAmount, satUp, satDown);
            DisplayChanges(environment - currentEcology.fillAmount, envUp, envDown);
            DisplayChanges(order - currentOrder.fillAmount, ordUp, ordDown);
        
            StartCoroutine(DisplaySelectedArrows());
        }

        currentInfrastructure.fillAmount = infrastructure; //jeżeli zmienimy bazową max wartość wskaźnika tutaj należy przez nią podzielić
        currentBudget.fillAmount = budget;
        currentLifeQuality.fillAmount = satisfaction;
        currentEcology.fillAmount = environment;
        currentOrder.fillAmount = order;

        UpdateBarColor(currentInfrastructure, infrastructure, 1f);
        UpdateBarColor(currentBudget, budget, 1f);
        UpdateBarColor(currentLifeQuality, satisfaction, 1f);
        UpdateBarColor(currentEcology, environment, 1f);
        UpdateBarColor(currentOrder, order, 1f);
    }

    private void DisplayChanges(float change, GameObject arrowUp, GameObject arrowDown) {
        if (change > 0) {
            selectedArrows.Add(arrowUp);
        }
        else if (change < 0) {
            selectedArrows.Add(arrowDown);
        }
    }

    private IEnumerator DisplaySelectedArrows() {
        foreach (GameObject arrow in selectedArrows) {
            arrow.SetActive(true);
        }

        yield return new WaitForSeconds(arrowsDisplayTime); // Czas wyświetlania strzałek

        foreach (GameObject arrow in selectedArrows) {
            arrow.SetActive(false);
        }

        selectedArrows = new List<GameObject>();
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
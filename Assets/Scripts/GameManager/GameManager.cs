using Cards;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private float riskFactor = 1f;
    
    private List<int> drawnCards = new List<int>();
    private List<int> availableCards = new List<int>();
    private Dictionary<int, int> cardsMagnitudes = new Dictionary<int, int>();

    public int Budget { get; private set; } = 50;
    public int Satisfaction { get; private set; } = 50;
    public int Infrastructure { get; private set; } = 50;
    public int Order { get; private set; } = 50;
    public int Environment { get; private set; } = 50;

    private void Awake() {
        _instance = this;
        availableCards.AddRange(Enumerable.Range(0, CardsManager.Instance.transform.childCount));

        foreach (int availableCard in availableCards) {
            cardsMagnitudes.Add(availableCard, CardsManager.Instance.GetEffectsMagnitude(availableCard));
        }

        DrawNextCard();
    }

    //TESTING

    bool wasPressed;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C) && wasPressed == false) {
            wasPressed = true;
            DrawNextCard();
        }

        if (Input.GetKeyUp(KeyCode.C)) {
            wasPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ChooseDecision(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ChooseDecision(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            ChooseDecision(2);
        }
    }

    //TESTING

    public void SetRiskFactor(float factor) {
        riskFactor = Mathf.Clamp(factor, 0.5f, 1.5f);
    }

    public void DrawNextCard() {
        // Tymczasowe rozwi¹zanie - reset kart po wyczerpaniu
        if (availableCards.Count == 0) {
            availableCards.AddRange(drawnCards);
            drawnCards.Clear();
        }

        bool isInDanger = StatsMagnitude() <= (int) (125 * riskFactor);
        int cardNumber;

        if (isInDanger) {
            cardNumber = GetRandomCard(0, (int) (60 * riskFactor));
        }
        else {
            cardNumber = GetRandomCard((int) (61 * riskFactor), int.MaxValue);
        }
        
        CardsManager.Instance.DrawCard(cardNumber);
    }

    public void ChooseDecision(int decisionNumber) {
        CardsManager.Instance.ChooseDecision(decisionNumber);
        UpdateStats(decisionNumber);
    }

    private int GetRandomCard(int minMagnitude, int maxMagnitude) {
        int randomCard;

        List<int> potentialCards = cardsMagnitudes.
            Where(card => availableCards.Contains(card.Key)).
            Where(card => card.Value >= minMagnitude && card.Value <= maxMagnitude).
            Select(card => card.Key).
            ToList();

        if (potentialCards.Count > 0) {
            randomCard = potentialCards[Random.Range(0, potentialCards.Count)];
        }
        else {
            randomCard = availableCards[Random.Range(0, availableCards.Count)];
        }

        availableCards.Remove(randomCard);
        drawnCards.Add(randomCard);

        Debug.Log($"Drawn card: {randomCard}");
        Debug.Log($"Its effects magnitude: {cardsMagnitudes[randomCard]}");
        Debug.Log($"AvailableCards count: {availableCards.Count}");

        return randomCard;
    }

    private int StatsMagnitude() {
        return Budget + Satisfaction + Infrastructure + Order + Environment;
    }

    private void UpdateStats(int decisionNumber) {
        CardLink currentCard = CardsManager.Instance.CurrentCard;

        Budget += currentCard.GetDecisionBudget(decisionNumber);
        Satisfaction += currentCard.GetDecisionSatisfaction(decisionNumber);
        Infrastructure += currentCard.GetDecisionInfrastructure(decisionNumber);
        Order += currentCard.GetDecisionOrder(decisionNumber);
        Environment += currentCard.GetDecisionEnvironment(decisionNumber);

        Debug.Log($"Stats after update");
        Debug.Log($"Budget: {Budget}, Satisfaction: {Satisfaction}, Infrastructure: {Infrastructure}, Order: {Order}, Environment: {Environment}");
    }
}
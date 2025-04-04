using Cards;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Tooltip("Reference to the pause menu game object.")]
    public GameObject pauseMenu;

    [Tooltip("Delay before spawning the article.")]
    public float articleSpawnDelay = 1f;
    [Tooltip("Delay before spawning the stamp.")]
    public float stampSpawnDelay = 1.5f;

    [Tooltip("Key to pause the game.")]
    public KeyCode pauseKey = KeyCode.Escape;
    [Tooltip("Key to hide the article.")]
    public KeyCode hideArticle = KeyCode.H;

    private BarsController barsController;
    private ArticleSpawner articleSpawner;
    private StampSpawner stampSpawner;

    private bool isPaused = false;
    private bool isArticleVisible = true;

    private void Awake() {
        _instance = this;
        
        barsController = FindObjectOfType<BarsController>();
        articleSpawner = FindObjectOfType<ArticleSpawner>();
        stampSpawner = FindObjectOfType<StampSpawner>();

        barsController.UpdateBars();

        availableCards.AddRange(Enumerable.Range(0, CardsManager.Instance.transform.childCount));

        foreach (int availableCard in availableCards) {
            cardsMagnitudes.Add(availableCard, CardsManager.Instance.GetEffectsMagnitude(availableCard));
        }

        PauseGame(false);

        DrawNextCard();
    }

    private void Update() {
        if (Input.GetKeyDown(pauseKey)) {
            PauseGame(!isPaused);
            stampSpawner.gameObject.SetActive(!isPaused);
        }

        else if (Input.GetKeyDown(hideArticle)) {
            isArticleVisible = !isArticleVisible;
            articleSpawner.ToggleVisibility(isArticleVisible);
        }
    }

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

        // Zastanowiæ siê nad elegancj¹ poni¿szych.
        articleSpawner.SpawnArticle(articleSpawnDelay);
        stampSpawner.SpawnStamp(stampSpawnDelay);
    }

    public void ChooseDecision(int decisionNumber) {
        CardsManager.Instance.ChooseDecision(decisionNumber);
        UpdateStats(decisionNumber);

        if (IsGameOver()) {
            Debug.Log("Game over!");
            PauseGame(true);
            SceneManager.LoadScene("EndScene");
        }

        DrawNextCard();
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

        barsController.UpdateBars();

        Debug.Log($"Stats after update");
        Debug.Log($"Budget: {Budget}, Satisfaction: {Satisfaction}, Infrastructure: {Infrastructure}, Order: {Order}, Environment: {Environment}");
    }

    private bool IsGameOver() {
        return Budget <= 0 || Satisfaction <= 0 || Infrastructure <= 0 || Order <= 0 || Environment <= 0;
    }

    public void PauseGame(bool pause) {
        isPaused = pause;
        Time.timeScale = pause ? 0 : 1;
        pauseMenu.SetActive(pause);
    }
}
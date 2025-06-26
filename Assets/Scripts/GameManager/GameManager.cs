using Cards;
using Minigames;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    private int currentCardNumber;
    private List<int> drawnCards = new List<int>();
    private List<int> availableCards = new List<int>();
    private Dictionary<int, int> cardsMagnitudes = new Dictionary<int, int>();
    private Dictionary<int, int> cardEffectsWatch = new Dictionary<int, int>();

    public int Budget { get; private set; } = 50;
    public int Satisfaction { get; private set; } = 50;
    public int Infrastructure { get; private set; } = 50;
    public int Order { get; private set; } = 50;
    public int Environment { get; private set; } = 50;

    [Header("Game Settings")]
    [Tooltip("Leave empty for random cards. Id is the number order of a child in the CardsManager.")]
    [SerializeField] private List<int> cardsToDraw = new List<int>();
    [Tooltip("Risk factor for drawing cards.")]
    [SerializeField] private float riskFactor = 1f;
    [Tooltip("Number of cards to win the game.")]
    [SerializeField] private int cardsToWin = 10;
    [Tooltip("Delay before drawing the next card.")]
    [SerializeField] private float drawNextCardDelay = 10f;
    [Tooltip("Delay before spawning the article card.")]
    [SerializeField] private float articleSpawnDelay = 1f;
    [Tooltip("Delay before spawning the stamp.")]
    [SerializeField] private float stampSpawnDelay = 1.5f;
    [Tooltip("Time limit to take a decision.")]
    [SerializeField] private float autoDecisionTime = 30f;
    [Tooltip("Time between minigames.")]
    [SerializeField] private float minigameCycleTime = 5f;
    [Tooltip("Chance of a minigame appearing after each cycle.")]
    [SerializeField][Range(0,1)] private float minigameChance;

    [Header("Penalties for each stat on minigame failure.")]
    [SerializeField] private int budgetPenalty = 5;
    [SerializeField] private int satisfactionPenalty = 5;
    [SerializeField] private int infrastructurePenalty = 5;
    [SerializeField] private int orderPenalty = 5;
    [SerializeField] private int environmentPenalty = 5;

    [Header("Other Settings")]
    [Tooltip("Scene to load when the game is lost.")]
    [SerializeField] private string failScene = "FailScene";
    [Tooltip("Scene to load when the game is won.")]
    [SerializeField] private string winScene = "WinScene";
    [Tooltip("Reference to the pause menu game object.")]
    [SerializeField] private GameObject pauseMenu;
    [Tooltip("Key to pause the game.")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [Tooltip("Key to hide the article.")]
    [SerializeField] private KeyCode hideArticle = KeyCode.H;

    public UnityEvent onDecisionChoice = new UnityEvent();

    private BarsController barsController;
    private ArticleSpawner articleSpawner;
    private StampSpawner stampSpawner;

    private bool isPaused;
    private bool isArticleVisible = true;
    private bool canChooseDecision = true;
    private bool isPauseMenuAvailable = true;

    private float autoDecisionTimer = 0f;
    private float minigamesCycleTimer = 0f;

    private void Awake() {
        _instance = this;

        barsController = FindObjectOfType<BarsController>();
        articleSpawner = FindObjectOfType<ArticleSpawner>();
        stampSpawner = FindObjectOfType<StampSpawner>();

        barsController.UpdateBars(false);

        if (cardsToDraw.Count == 0) {
            availableCards.AddRange(Enumerable.Range(0, CardsManager.Instance.transform.childCount));
        }
        else {
            availableCards.AddRange(cardsToDraw);
        }

        foreach (int availableCard in availableCards) {
            cardsMagnitudes.Add(availableCard, CardsManager.Instance.GetEffectsMagnitude(availableCard));
        }

        StartGame();
        ToggleVisibility(false);

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += ShowEndScreenStats;
    }

    private void ShowEndScreenStats(Scene scene, LoadSceneMode mode) {
        if (scene.name.Equals(winScene) == false && scene.name.Equals(failScene) == false) {
            return;
        }

        barsController = FindObjectOfType<BarsController>();
        barsController.UpdateBars(false);

        isPauseMenuAvailable = false;
        isPaused = true;
    }

    private void Update() {
        if (Input.GetKeyDown(pauseKey)) {
            PauseGame(!isPaused);
        }
        // TODO: Na pozniej :)
        //else if (Input.GetKeyDown(hideArticle)) {
        //    ToggleVisibility(!isArticleVisible);
        //}

        #region Debug
        //else if (Input.GetKeyDown(KeyCode.Alpha0)) {
        //    StartRandomMinigame();
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //    MinigamesManager.Instance.StartMinigame(0, budgetPenalty, satisfactionPenalty,
        //        infrastructurePenalty, orderPenalty, environmentPenalty);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2)) {
        //    MinigamesManager.Instance.StartMinigame(1, budgetPenalty, satisfactionPenalty,
        //        infrastructurePenalty, orderPenalty, environmentPenalty);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3)) {
        //    MinigamesManager.Instance.StartMinigame(2, budgetPenalty, satisfactionPenalty,
        //        infrastructurePenalty, orderPenalty, environmentPenalty);
        //}
        #endregion

        if (!isPaused) {
            HandleAutoDecision();
            HandleMinigamesCycles();
        }
    }
    
    public void ToggleVisibility(bool toggleOn) {
        isArticleVisible = toggleOn;
        articleSpawner.ToggleVisibility(isArticleVisible);
        stampSpawner.gameObject.SetActive(isArticleVisible);
    }

    public void StartGame() {
        CardsManager.Instance.DrawCard(0);
        availableCards.Remove(0);
        articleSpawner.SetAutoDecisionTime(autoDecisionTime);
        articleSpawner.SpawnArticle(0f);
        stampSpawner.SpawnStamp(0f);
        isPaused = true;
    }

    public void StartRandomMinigame() {
        int randomMinigame = Random.Range(0, MinigamesManager.Instance.MinigamesCount);

        MinigamesManager.Instance.StartMinigame(randomMinigame, budgetPenalty, 
            satisfactionPenalty, infrastructurePenalty, orderPenalty, environmentPenalty);
    }

    private void HandleAutoDecision() {
        autoDecisionTimer += Time.deltaTime;

        if (autoDecisionTimer >= autoDecisionTime) {
            ChooseDecision(0);
        }
    }

    private void HandleMinigamesCycles() {
        minigamesCycleTimer += Time.deltaTime;

        if (minigamesCycleTimer >= minigameCycleTime) {
            minigamesCycleTimer = 0f;

            if (Random.Range(0f, 1f) <= minigameChance) {
                StartRandomMinigame();
            }
        }
    }

    public void SetRiskFactor(float factor) {
        riskFactor = Mathf.Clamp(factor, 0.5f, 1.5f);
    }

    public void DrawNextCard() {
        ToggleVisibility(true);

        if (cardEffectsWatch.Count > 0) {
            for (int i = 0; i < cardEffectsWatch.Count; i++) {
                cardEffectsWatch[cardEffectsWatch.ElementAt(i).Key] -= 1;

                if (cardEffectsWatch.ElementAt(i).Value <= 0) {
                    // Debug.Log($"Ending effect of card {cardEffectsWatch.ElementAt(i).Key}");
                    CardsManager.Instance.EndDecisionEffect(cardEffectsWatch.ElementAt(i).Key);
                    cardEffectsWatch.Remove(cardEffectsWatch.ElementAt(i).Key);
                }
            }
        }

        // Tymczasowe rozwi¹zanie - reset kart po wyczerpaniu
        if (availableCards.Count == 0) {
            availableCards.AddRange(drawnCards);
            drawnCards.Clear();
        }

        bool isInDanger = StatsMagnitude() <= (int) (125 * riskFactor);

        if (isInDanger) {
            currentCardNumber = GetRandomCard(0, (int) (60 * riskFactor));
        }
        else {
            currentCardNumber = GetRandomCard((int) (61 * riskFactor), int.MaxValue);
        }

        CardsManager.Instance.DrawCard(currentCardNumber);

        articleSpawner.SpawnArticle(articleSpawnDelay);
        autoDecisionTimer = 0f;
    }

    public void ChooseDecision(int decisionNumber) {
        if (canChooseDecision == false) { return; }
        
        canChooseDecision = false;
        onDecisionChoice.Invoke();

        int effectsDuration = CardsManager.Instance.CurrentCard.card.Decisions[decisionNumber].EffectsDuration;
        cardEffectsWatch.Add(currentCardNumber, effectsDuration);

        CardsManager.Instance.ChooseDecision(decisionNumber);
        UpdateStats(decisionNumber);

        if (IsGameFailed()) {
            SceneManager.LoadScene(failScene);
            return;
        }
        else if (IsGameWon()) {
            SceneManager.LoadScene(winScene);
            return;
        }

        stampSpawner.SpawnStamp(stampSpawnDelay);
        Invoke(nameof(DrawNextCard), drawNextCardDelay);
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

        // Debug.Log($"Drawn card: {randomCard}");
        // Debug.Log($"Its effects magnitude: {cardsMagnitudes[randomCard]}");
        // Debug.Log($"AvailableCards count: {availableCards.Count}");

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

        // Debug.Log($"Stats after card {currentCard.name}");
        // Debug.Log($"Budget: {Budget}, Satisfaction: {Satisfaction}, Infrastructure: {Infrastructure}, Order: {Order}, Environment: {Environment}");

        if (IsGameFailed()) {
            Debug.Log("Game over!");
            SceneManager.LoadScene("EndScene");
        }
    }

    public void UpdateStats(int budget, int satisfaction, 
        int infrastructure, int order, int environment) {
        Budget += budget;
        Satisfaction += satisfaction;
        Infrastructure += infrastructure;
        Order += order;
        Environment += environment;

        barsController.UpdateBars();

        // Debug.Log($"Stats after update");
        // Debug.Log($"Budget: {Budget}, Satisfaction: {Satisfaction}, Infrastructure: {Infrastructure}, Order: {Order}, Environment: {Environment}");

        if (IsGameFailed()) {
            Debug.Log("Game over!");
            SceneManager.LoadScene("EndScene");
        }
    }

    private bool IsGameFailed() {
        return Budget <= 0 || Satisfaction <= 0 || Infrastructure <= 0 || Order <= 0 || Environment <= 0;
    }

    private bool IsGameWon() {
        return drawnCards.Count >= cardsToWin;
    }

    public void PauseGame(bool pause) {
        if (isPauseMenuAvailable == false) { return; }
        if (MinigamesManager.Instance.IsPaused) { return; }

        isPaused = pause;
        Time.timeScale = pause ? 0 : 1;
        stampSpawner.gameObject.SetActive(!isPaused && isArticleVisible);
        pauseMenu.SetActive(pause);
    }

    public void EnableChoice() {
        canChooseDecision = true;
    }
}
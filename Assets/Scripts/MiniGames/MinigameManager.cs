using System.Collections;
using UnityEngine;

namespace Minigames {
    internal abstract class MinigameManager<T> : MonoBehaviour, IMinigame where T : MonoBehaviour {
        protected static T _instance;
        internal static T Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }

        protected void Awake() {
            if (_instance == null) {
                _instance = this as T;
            }
            else {
                Destroy(gameObject);
            }
        }

        [Header("Minigame Area")]
        [Tooltip("Parent of all graphic features of the minigame")]
        [SerializeField] protected GameObject minigameArea;
        [SerializeField] protected TimeManager timeManager;
        [SerializeField] protected float startingTimeLimit = 10f;

        [Header("Victory and failure UI Elements")]
        [Tooltip("UI to be displayed when the game is won.")]
        [SerializeField] protected GameObject gameWonUI;
        [Tooltip("UI to be displayed when the game is lost.")]
        [SerializeField] protected GameObject gameLostUI;
        [Tooltip("How long should gameWonUI or gameLostUI be displayed.")]
        [SerializeField] protected float timeoutUI = 2f;

        protected bool isGameStarted;
        protected bool isFirstTime = true;

        public virtual void StartGame() {
            if (isGameStarted) { return; }

            timeManager.SetCountdownDuration(startingTimeLimit);

            minigameArea.SetActive(true);
            isGameStarted = true;

            gameWonUI.SetActive(false);
            gameLostUI.SetActive(false);
        }

        public virtual void EndGame() {
            gameWonUI.SetActive(false);
            gameLostUI.SetActive(false);
            MinigamesManager.Instance.HideGameArea();
            minigameArea.SetActive(false);
        }

        public void WinGame() {
            isGameStarted = false;
            StartCoroutine(WinGameDelayed());
        }

        private IEnumerator WinGameDelayed() {
            gameWonUI.SetActive(true);
            yield return new WaitForSeconds(timeoutUI);
            EndGame();
            MinigamesManager.Instance.WinGame();
        }

        public void FailGame() {
            isGameStarted = false;
            StartCoroutine(FailGameDelayed());
        }

        private IEnumerator FailGameDelayed() {
            gameLostUI.SetActive(true);
            yield return new WaitForSeconds(timeoutUI);
            EndGame();
            MinigamesManager.Instance.FailGame();
        }
    }
}
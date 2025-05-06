using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Minigames {
    public class MinigamesManager : MonoBehaviour {
        private static MinigamesManager _instance;
        public static MinigamesManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<MinigamesManager>();
                }
                return _instance;
            }
        }

        private void Awake() {
            if (_instance == null) {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }

        private List<IMinigame> minigameManagers = new List<IMinigame>();
        public int MinigamesCount { private set; get; }
        private IMinigame currentMinigame;

        private Animator animator;
        private int budgetPenalty;
        private int satisfactionPenalty;
        private int infrastructurePenalty;
        private int orderPenalty;
        private int environmentPenalty;

        private void Start() {
            minigameManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<IMinigame>().ToList();
            MinigamesCount = minigameManagers.Count;
            animator = GetComponent<Animator>();
        }

        public void StartMinigame(int i, 
            int budgetPenalty = 0,
            int satisfactionPenalty = 0,
            int infrastructurePenalty = 0,
            int orderPenalty = 0,
            int environmentPenalty = 0) {

            if (currentMinigame != null) { return; }

            this.budgetPenalty = budgetPenalty;
            this.satisfactionPenalty = satisfactionPenalty;
            this.infrastructurePenalty = infrastructurePenalty;
            this.orderPenalty = orderPenalty;
            this.environmentPenalty = environmentPenalty;

            animator.Play("Appear");
            currentMinigame = minigameManagers[i];
            currentMinigame.StartGame();
        }

        public void EndMinigame() {
            currentMinigame = null;
            animator.Play("Disappear");
        }

        public void WinGame() {
            EndMinigame();
        }

        public void FailGame() {
            GameManager.Instance.UpdateStats(
                -budgetPenalty, 
                -satisfactionPenalty, 
                -infrastructurePenalty, 
                -orderPenalty, 
                -environmentPenalty
            );

            Debug.Log("Minigame failed!");

            EndMinigame();
        }

        public void HideGameArea() {
            animator.Play("Disappear");
        }
    }
}
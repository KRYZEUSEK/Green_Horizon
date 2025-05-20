using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.PapersGame {
    internal class PapersGameManager : MinigameManager<PapersGameManager>, IMinigame {
        [System.Serializable]
        internal enum TargetType {
            Important,
            Normal,
            Trash,
            Other
        }

        [Tooltip("Parent of all graphic features of the minigame")]
        [SerializeField] private GameObject minigameArea;
        [Tooltip("References to the papers types")]
        [SerializeField] private GameObject[] papersPrefabs;
        [Tooltip("References to the targets types")]
        [SerializeField] private GameObject[] targetsPrefabs;
        [Tooltip("How many pieces should be spawned at the start of the game")]
        [SerializeField] private int startingPieces = 5;
        [Tooltip("Limit of the pieces")]
        [SerializeField] private int maxPieces = 50;
        [Tooltip("Progression rate for the pieces number")]
        [SerializeField] private float progression = 1.5f;
        [Tooltip("Starting time limit for the game in seconds (will increase with the given progression)")]
        [SerializeField] private float startingTimeLimit = 7.5f;
        [Tooltip("Area where the papers will be spawned")]
        [SerializeField] private RectTransform papersSpawnArea;
        [Tooltip("Areas where the targets will be spawned")]
        [SerializeField] private RectTransform[] targetsSpawnArea;

        private int round;
        private int numberOfPapers;
        private float timeLimit;

        public void StartGame() {
            if (isGameStarted) { return; }
            
            minigameArea.SetActive(true);
            isGameStarted = true;
            
            round++;
            timeLimit = startingTimeLimit * round;

            // Calculate the number of papers to spawn based on the round number and progression rate
            numberOfPapers = Mathf.FloorToInt(startingPieces * Mathf.Pow(progression, round));
            numberOfPapers = Mathf.Clamp(numberOfPapers, 1, maxPieces);

            Debug.Log($"Round {round}: Spawning {numberOfPapers} papers.");

            // Divide the number of papers among the different types
            int[] papersCount = new int[papersPrefabs.Length];
            int papersLeft = numberOfPapers;

            for (int i = 0; i < papersCount.Length; i++) {
                if (i == papersCount.Length - 1) {
                    papersCount[i] = papersLeft;
                }
                else {
                    papersCount[i] = Random.Range(0, papersLeft);
                    papersLeft -= papersCount[i];
                }

                SpawnPapers(papersCount[i], papersPrefabs[i]);
            }

            SpawnTargets();

            if (isFirstTime) {
                isFirstTime = false;
                MinigamesManager.Instance.PauseForTutorial();
            }
        }

        private void Update() {
            if (!isGameStarted) { return; }

            timeLimit -= Time.deltaTime;

            if (timeLimit <= 0) {
                FailGame();
            }
        }

        internal void ScorePoint() {
            numberOfPapers--;
            Debug.Log($"Papers left: {numberOfPapers}");

            if (numberOfPapers <= 0) {
                WinGame();
            }
        }

        private void SpawnPapers(int number, GameObject prefab) {
            for (int i = 0; i < number; i++) {
                GameObject paper = Instantiate(prefab, papersSpawnArea);

                paper.transform.localPosition = new Vector3(
                    Random.Range(papersSpawnArea.rect.xMin, papersSpawnArea.rect.xMax),
                    Random.Range(papersSpawnArea.rect.yMin, papersSpawnArea.rect.yMax),
                    0
                );

                paper.transform.localRotation = Quaternion.Euler(0,0,Random.Range(0, 360));
            }
        }

        private void SpawnTargets() {
            List<RectTransform> targets = new List<RectTransform>();

            foreach (GameObject targetPrefab in targetsPrefabs) {
                int chosenArea = Random.Range(0, targetsSpawnArea.Length);
                GameObject target = Instantiate(targetPrefab, targetsSpawnArea[chosenArea]);
                
                bool isOverlapping = true;
                Vector3 position = Vector3.zero;

                while (isOverlapping) {
                    isOverlapping = false;

                    position = new Vector3(
                        Random.Range(targetsSpawnArea[chosenArea].rect.xMin, targetsSpawnArea[chosenArea].rect.xMax),
                        Random.Range(targetsSpawnArea[chosenArea].rect.yMin, targetsSpawnArea[chosenArea].rect.yMax),
                        0
                    );

                    foreach (RectTransform otherTarget in targets) {
                        if (RectTransformUtility.RectangleContainsScreenPoint(otherTarget, position)) {
                            isOverlapping = true;
                            break;
                        }
                    }
                }

                target.transform.localPosition = position;
                targets.Add(target.GetComponent<RectTransform>());
            }
        }

        public void FailGame() {
            EndGame();
            MinigamesManager.Instance.FailGame();
        }

        public void WinGame() {
            EndGame();
            MinigamesManager.Instance.WinGame();
        }
        
        public void EndGame() {
            isGameStarted = false;

            // Destroy all the papers and targets
            foreach (RectTransform paperTransform in papersSpawnArea.transform) {
                Destroy(paperTransform.gameObject);
            }

            foreach (RectTransform target in targetsSpawnArea) {
                foreach (RectTransform child in target.transform) {
                    Destroy(child.gameObject);
                }
            }

            MinigamesManager.Instance.HideGameArea();
            minigameArea.SetActive(false);
        }
    }
}
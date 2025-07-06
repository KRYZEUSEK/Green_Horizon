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
        [Tooltip("Area where the papers will be spawned")]
        [SerializeField] private RectTransform papersSpawnArea;
        [Tooltip("Areas where the targets will be spawned")]
        [SerializeField] private RectTransform[] targetsSpawns;

        private int round;
        private int numberOfPapers;
        private float timeLimit;

        public override void StartGame() {
            base.StartGame();

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
            List<int> usedSpawns = new List<int>();

            foreach (GameObject targetPrefab in targetsPrefabs) {
                int chosenArea = Random.Range(0, targetsSpawns.Length);

                while (usedSpawns.Contains(chosenArea)) {
                    chosenArea = Random.Range(0, targetsSpawns.Length);
                }

                usedSpawns.Add(chosenArea);
                GameObject target = Instantiate(targetPrefab, targetsSpawns[chosenArea]);
                
                target.transform.localPosition = Vector3.zero;
                targets.Add(target.GetComponent<RectTransform>());
            }
        }
        
        public override void EndGame() {
            // Destroy all the papers and targets
            foreach (RectTransform paperTransform in papersSpawnArea.transform) {
                Destroy(paperTransform.gameObject);
            }

            foreach (RectTransform target in targetsSpawns) {
                foreach (RectTransform child in target) {
                    Destroy(child.gameObject);
                }
            }

            base.EndGame();
        }
    }
}
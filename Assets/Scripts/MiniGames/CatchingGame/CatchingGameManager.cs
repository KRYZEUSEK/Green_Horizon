using UnityEngine;

namespace Minigames.CatchingGame {
    internal class CatchingGameManager : MinigameManager<CatchingGameManager> {
        [Tooltip("Prefab of the player")]
        [SerializeField] private GameObject playerPrefab;
        [Tooltip("Area where the player will be spawned")]
        [SerializeField] private RectTransform playerSpawnArea;
        [Tooltip("Prefabs of the items")]
        [SerializeField] private GameObject[] itemsPrefabs;
        [Tooltip("Area where the items will be spawned")]
        [SerializeField] private RectTransform itemsSpawnArea;
        [Tooltip("Time between item spawns")]
        [SerializeField] private float timeBetweenSpawns = 0.5f;
        [Tooltip("Speed of the items")]
        [SerializeField] internal float itemSpeed = 1f;
        [Tooltip("How quickly should the game be prolonged")]
        [SerializeField] private float progression = 1.25f;
        [Tooltip("How many items can be lost before failing the game")]
        [SerializeField] private int maxLostItems = 3;

        private float timeSinceLastSpawn = 0f;
        private float timeLimit;
        private int lostItems = 0;
        private GameObject player;

        private void Update() {
            if (isGameStarted == false) { return; }

            HandleTimer();
            HandleSpawning();

            void HandleTimer() {
                timeLimit -= Time.deltaTime;
                if (timeLimit <= 0) {
                    WinGame();
                }
            }

            void HandleSpawning() {
                timeSinceLastSpawn += Time.deltaTime;

                if (timeLimit <= 1f) { return; }

                if (timeSinceLastSpawn >= timeBetweenSpawns) {
                    SpawnItem();
                    timeSinceLastSpawn = 0f;
                }
            }
        }

        private void SpawnItem() {
            int randomIndex = Random.Range(0, itemsPrefabs.Length);
            float xPos = Random.Range(itemsSpawnArea.rect.xMin, itemsSpawnArea.rect.xMax);
            float yPos = Random.Range(itemsSpawnArea.rect.yMin, itemsSpawnArea.rect.yMax);
            Vector3 spawnPosition = itemsSpawnArea.TransformPoint(new Vector3(xPos, yPos, 0f));

            Instantiate(itemsPrefabs[randomIndex], spawnPosition, Quaternion.identity, itemsSpawnArea);
        }

        internal void LostItem() {
            lostItems++;
            if (lostItems >= maxLostItems) {
                FailGame();
            }
        }

        public override void StartGame() {
            base.StartGame();

            timeSinceLastSpawn = 0f;
            timeLimit = startingTimeLimit;
            lostItems = 0;
            SpawnPlayer();

            if (isFirstTime) {
                isFirstTime = false;
                MinigamesManager.Instance.PauseForTutorial();
            }
        }

        private void SpawnPlayer() {
            player = Instantiate(playerPrefab, playerSpawnArea);
        }

        public override void EndGame() {
            isGameStarted = false;
            Destroy(player);

            foreach (Transform item in itemsSpawnArea) {
                Destroy(item.gameObject);
            }

            MinigamesManager.Instance.HideGameArea();
            minigameArea.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigames.TypingGame {
    internal class TypingGameManager : MinigameManager<TypingGameManager>, IMinigame {
        [Header("Game Settings")]
        [Tooltip("Words to be typed")]
        [SerializeField] private string[] words;
        [Tooltip("Time limit for the game in seconds")]
        [SerializeField] private float timeLimit = 10f;

        [Header("UI Elements")]
        [Tooltip("Parent of all graphic features of the minigame")]
        [SerializeField] private GameObject minigameArea;
        [Tooltip("TextMeshPro component to display the word to type")]
        [SerializeField] private TMP_Text wordToTypeDisplay;
        [Tooltip("TextMeshPro component to type the word")]
        [SerializeField] private TMP_Text typedWordDisplay;

        private List<string> availableWords = new List<string>();
        private string wordToType;
        private string currentWord = "";
        private float timer = 0;

        private void Start() {
            availableWords = new List<string>(words);
        }

        private void DisplayNextWord() {
            wordToType = GetRandomWord();
            DisplayWordToWrite();
            typedWordDisplay.text = string.Empty;
        }

        private void DisplayWordToWrite() {
            wordToTypeDisplay.text = wordToType;
        }

        private string GetRandomWord() {
            if (availableWords.Count == 0) {
                availableWords = new List<string>(words);
            }

            string word = availableWords[Random.Range(0, availableWords.Count)];
            availableWords.Remove(word);
            return word;
        }

        private void Update() {
            if (isGameStarted == false) { return; }

            timer += Time.deltaTime;

            if (timer >= timeLimit) { FailGame();}

            if (Input.anyKeyDown) {
                char character = Input.inputString[0];
                
                if (char.IsLetter(character)) {
                    string characterString = character.ToString();
                    TypeCharacter(characterString.ToUpper());
                }
            }
        }

        internal void TypeCharacter(string character) {
            currentWord += character;
            typedWordDisplay.text = currentWord;

            int currentWordLength = currentWord.Length;
            int wordToTypeLength = wordToType.Length;

            if (currentWordLength > wordToTypeLength) {
                FailGame();
            }
            else if (currentWord.Equals(wordToType.Substring(0, currentWordLength)) == false) {
                FailGame();
            }
            else if (currentWord.Equals(wordToType)) {
                WinGame();
            }
        }

        public void StartGame() {
            minigameArea.SetActive(true);
            isGameStarted = true;
            timer = 0;
            currentWord = "";
            wordToTypeDisplay.text = "";
            typedWordDisplay.text = "";

            DisplayNextWord();

            if (isFirstTime) {
                isFirstTime = false;
                MinigamesManager.Instance.PauseForTutorial();
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
            MinigamesManager.Instance.HideGameArea();
            minigameArea.SetActive(false);
        }
    }
}
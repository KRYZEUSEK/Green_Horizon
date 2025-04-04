using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Cards {
    [ExecuteAlways]
    public class CardsManager : MonoBehaviour {
        private static CardsManager _instance;

        public static CardsManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<CardsManager>();
                }
                return _instance;
            }
        }

        [SerializeField] private List<Card> cards = new List<Card>();
        [SerializeField] private List<CardLink> cardsLinks = new List<CardLink>();

        private int currentCardNumber;

        public CardLink CurrentCard {
            get { return cardsLinks[currentCardNumber]; }
        }

        private void Awake() {
            _instance = this;
        }

#if UNITY_EDITOR
        [ContextMenu("Reload cards")]
        private void ReloadCards() {
            List<Card> cardsResources = Resources.LoadAll<Card>("Cards").ToList();
            List<CardLink> linksToIgnore = new List<CardLink>();

            if (cardsResources.Count == 0) {
                Debug.LogWarning("No cards found in Resources/Cards!");
                return;
            }

            cards = new List<Card>();
            cards = cardsResources;
            cardsLinks = new List<CardLink>();
            cardsLinks = GetComponentsInChildren<CardLink>().ToList();

            // Przepatrz, czy wszystkie karty maj¹ linki.
            foreach (Card card in cards) {
                if (cardsLinks.Find(cardLink => cardLink.cardId.Equals(card.Id)) == null) {
                    GameObject cardLinkObject = new GameObject(card.Title);

                    cardLinkObject.transform.SetParent(transform);
                    cardLinkObject.AddComponent<CardLink>().AssignCard(card);

                    cardsLinks.Add(cardLinkObject.GetComponent<CardLink>());
                    linksToIgnore.Add(cardLinkObject.GetComponent<CardLink>());
                }
            }

            // Przepatrz, czy wszystkie pozosta³e linki maj¹ przypisane karty.
            foreach (CardLink cardLink in cardsLinks.Except(linksToIgnore)) {
                if (cardLink.card == null) {
                    Card card = cards.Find(c => c.Id.Equals(cardLink.cardId));

                    if (card == null) {
                        DestroyImmediate(cardLink.gameObject);
                    }
                    else {
                        cardLink.AssignCard(card);
                    }
                }

                cardLink.name = cardLink.card.Title;
            }
        }

        // DODAÆ ID DO KART
        [ContextMenu("Reload cards from .tsv")]
        private void ReloadCardsFromTSV() {
            // Usuniêcie wszystkich kart z Resources/Cards.
            string[] guids = AssetDatabase.FindAssets("t:Card", new[] { "Assets/Resources/Cards" });

            foreach (string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AssetDatabase.DeleteAsset(path);
            }

            // Wczytanie kart z pliku .tsv.
            using StreamReader reader = new StreamReader($"{Application.dataPath}/Resources/Cards.tsv");

            string line = reader.ReadLine();
            string[] headers = line.Split('\t');

            while (true) {
                line = reader.ReadLine();
                if (line == null) { break; }

                string[] fields = line.Split('\t');

                Debug.Log($"Loading card: {String.Join(" | ", fields)}");

                Card card = ScriptableObject.CreateInstance<Card>();

                card.Id = fields[0];
                card.Title = fields[1];
                card.Description = fields[2];
                card.Decisions = new CardDecision[3];

                for (int i = 0; i < 3; i++) {
                    card.Decisions[i] = new CardDecision();
                    card.Decisions[i].Description = fields[i * 7 + 3];
                    card.Decisions[i].Budget = int.Parse(fields[i * 7 + 4]);
                    card.Decisions[i].Satisfaction = int.Parse(fields[i * 7 + 5]);
                    card.Decisions[i].Infrastructure = int.Parse(fields[i * 7 + 6]);
                    card.Decisions[i].Order = int.Parse(fields[i * 7 + 7]);
                    card.Decisions[i].Environment = int.Parse(fields[i * 7 + 8]);
                    card.Decisions[i].EffectsDuration = int.Parse(fields[i * 7 + 9]);
                }

                string name = AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Cards/{card.Title}.asset");

                AssetDatabase.CreateAsset(card, name);
                AssetDatabase.SaveAssets();
            }

            // Reset kart na scenie.
            ReloadCards();
        }
#endif

        public void DrawCard(int cardNumber) {
            currentCardNumber = cardNumber;
            CurrentCard.DrawCard();
        }

        public void EndCardEffect(int cardNumber) {
            cardsLinks[cardNumber].EndCardEffect();
        }

        public void ChooseDecision(int decisionNumber) {
            CurrentCard.ChooseDecision(decisionNumber);
        }

        public void EndDecisionEffect(int cardNumber) {
            cardsLinks[cardNumber].EndDecisionEffect();
        }

        public int GetEffectsMagnitude(int cardNumber) {
            if (cardNumber < 0 || cardNumber >= cards.Count) { return 0; }

            List<int> effects = new List<int>();

            foreach(CardDecision decision in cards[cardNumber].Decisions) {
                effects.Add(decision.Budget);
                effects.Add(decision.Satisfaction);
                effects.Add(decision.Infrastructure);
                effects.Add(decision.Order);
                effects.Add(decision.Environment);
            }

            int magnitude = 0;

            effects.ForEach(effect => magnitude += Math.Abs(effect));

            return magnitude;
        }
    }
}
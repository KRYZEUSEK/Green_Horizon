using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [ContextMenu("Read the cards in")]
        private void ReadInCards() {
            List<Card> cardsResources = Resources.LoadAll<Card>("Cards").ToList();

            if (cardsResources.Count == 0) {
                Debug.LogWarning("No cards found in Resources/Cards!");
                return;
            }

            if (cardsResources.Count != cards.Count) {
                foreach (Card card in cardsResources) {
                    if (cards.Contains(card) == false) {
                        cards.Add(card);
                        GameObject missingLink = new GameObject(card.Title);
                        missingLink.transform.SetParent(transform);
                        missingLink.AddComponent<CardLink>().AssignCard(card);
                    }
                }
            }

            cardsLinks = new List<CardLink>();

            foreach (CardLink cardLink in GetComponentsInChildren<CardLink>()) {
                cardsLinks.Add(cardLink);
            }
        }

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
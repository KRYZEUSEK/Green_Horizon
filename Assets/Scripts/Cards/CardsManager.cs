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
                        missingLink.AddComponent<CardLink>().card = card;
                    }
                }
            }

            cardsLinks = new List<CardLink>();

            foreach (CardLink cardLink in GetComponentsInChildren<CardLink>()) {
                cardsLinks.Add(cardLink);
            }
        }

        public void DrawCard(int i) {
            currentCardNumber = i;
            CurrentCard.DrawCard();
        }

        public void EndCardEffect(int i) {
            cardsLinks[i].EndCardEffect();
        }

        public void ChooseDecision(int i) {
            CurrentCard.ChooseDecision(i);
        }
    }
}
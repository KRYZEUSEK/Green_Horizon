using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cards {
    public class CardLink : MonoBehaviour {
        [SerializeField] internal Card card;

        public UnityEvent onAppearEvent;
        public UnityEvent EndCardEffectEvent;
        public UnityEvent[] onDecisionEvent = new UnityEvent[3];
        public UnityEvent[] onEndDecisionEvent = new UnityEvent[3];

        public int ChosenDecision { get; private set; }

        public void DrawCard() {
            onAppearEvent?.Invoke();
        }

        public void EndCardEffect() {
            EndCardEffectEvent?.Invoke();
        }

        public void ChooseDecision(int decisionNumber) {
            ChosenDecision = decisionNumber;
            onDecisionEvent[decisionNumber]?.Invoke();
        }

        public void EndDecisionEffect() {
            onEndDecisionEvent[ChosenDecision]?.Invoke();
        }

        public void AssignCard(Card card) {
            this.card = card;
        }

        public int GetDecisionSatisfaction(int decisionNumber) {
            return card.Decisions[decisionNumber].Satisfaction;
        }

        public int GetDecisionBudget(int decisionNumber) {
            return card.Decisions[decisionNumber].Budget;
        }

        public int GetDecisionInfrastructure(int decisionNumber) {
            return card.Decisions[decisionNumber].Infrastructure;
        }

        public int GetDecisionOrder(int decisionNumber) {
            return card.Decisions[decisionNumber].Order;
        }

        public int GetDecisionEnvironment(int decisionNumber) {
            return card.Decisions[decisionNumber].Environment;
        }
    }
}
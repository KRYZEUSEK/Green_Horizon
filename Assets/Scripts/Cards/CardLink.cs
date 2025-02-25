using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cards {
    public class CardLink : MonoBehaviour {
        public Card card;

        public UnityEvent onAppearEvent;
        public UnityEvent EndCardEffectEvent;
        public UnityEvent[] onDecisionEvent = new UnityEvent[3];

        public void DrawCard() {
            onAppearEvent?.Invoke();
        }

        public void EndCardEffect() {
            EndCardEffectEvent?.Invoke();
        }

        public void ChooseDecision(int i) {
            onDecisionEvent[i]?.Invoke();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards {
    [System.Serializable]
    public class CardDecision {
        [SerializeField] private string _description = "";
        
        public string Description {
            get { return _description; }
            private set { _description = value; }
        }

        [SerializeField] private int _budget = 0;

        public int Budget {
            get { return _budget; }
            private set { _budget = value; }
        }

        [SerializeField] private int _satisfaction = 0;

        public int Satisfaction {
            get { return _satisfaction; }
            private set { _satisfaction = value; }
        }

        [SerializeField] private int _infrastructure = 0;

        public int Infrastructure {
            get { return _infrastructure; }
            private set { _infrastructure = value; }
        }

        [SerializeField] private int _order = 0;

        public int Order {
            get { return _order; }
            private set { _order = value; }
        }

        [SerializeField] private int _environment = 0;

        public int Environment {
            get { return _environment; }
            private set { _environment = value; }
        }
    }

    [System.Serializable]
    [CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
    public class Card : ScriptableObject {
        [SerializeField] private string _title = "";

        public string Title {
            get { return _title; }
            private set { _title = value; }
        }

        [SerializeField] private string _description = "";

        public string Description {
            get { return _description; }
            private set { _description = value; }
        }

        [SerializeField] private Sprite _image = null;

        public Sprite Image {
            get { return _image; }
            private set { _image = value; }
        }

        [SerializeField] private CardDecision[] _decisions = new CardDecision[3];

        public CardDecision[] Decisions {
            get { return _decisions; }
            private set { _decisions = value; }
        }
    }
}
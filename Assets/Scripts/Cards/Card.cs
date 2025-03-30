using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards {
    [System.Serializable]
    public class CardDecision {
        [SerializeField] private string _description = "";
        
        public string Description {
            get { return _description; }
            internal set { _description = value; }
        }

        [SerializeField] private int _budget = 0;

        public int Budget {
            get { return _budget; }
            internal set { _budget = value; }
        }

        [SerializeField] private int _satisfaction = 0;

        public int Satisfaction {
            get { return _satisfaction; }
            internal set { _satisfaction = value; }
        }

        [SerializeField] private int _infrastructure = 0;

        public int Infrastructure {
            get { return _infrastructure; }
            internal set { _infrastructure = value; }
        }

        [SerializeField] private int _order = 0;

        public int Order {
            get { return _order; }
            internal set { _order = value; }
        }

        [SerializeField] private int _environment = 0;

        public int Environment {
            get { return _environment; }
            internal set { _environment = value; }
        }

        [SerializeField] private int _effectsDuration = 1;

        public int EffectsDuration {
            get { return _effectsDuration; }
            internal set { _effectsDuration = value; }
        }
    }

    [System.Serializable]
    [CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
    public class Card : ScriptableObject {
        [SerializeField] private string _id = "";

        public string Id {
            get { return _id; }
            internal set { _id = value; }
        }

        [SerializeField] private string _title = "";

        public string Title {
            get { return _title; }
            internal set { _title = value; }
        }

        [SerializeField] private string _description = "";

        public string Description {
            get { return _description; }
            internal set { _description = value; }
        }

        [SerializeField] private Sprite _image = null;

        public Sprite Image {
            get { return _image; }
            internal set { _image = value; }
        }

        [SerializeField] private CardDecision[] _decisions = new CardDecision[3];

        public CardDecision[] Decisions {
            get { return _decisions; }
            internal set { _decisions = value; }
        }
    }
}
using UnityEngine;

namespace Minigames {
    internal abstract class MinigameManager<T> : MonoBehaviour where T : MonoBehaviour {
        protected static T _instance;
        internal static T Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }

        protected void Awake() {
            if (_instance == null) {
                _instance = this as T;
            }
            else {
                Destroy(gameObject);
            }
        }

        protected bool isGameStarted;
        protected bool isFirstTime = true;
    }
}
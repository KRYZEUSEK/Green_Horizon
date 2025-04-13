using UnityEngine;

namespace Minigames {
    internal abstract class MinigameManager<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        internal static T Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }

        private void Awake() {
            if (_instance == null) {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
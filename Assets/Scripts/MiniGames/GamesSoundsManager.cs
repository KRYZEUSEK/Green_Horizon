using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames {
    [RequireComponent(typeof(AudioSource))]
    internal class GamesSoundsManager : MonoBehaviour {
        private static GamesSoundsManager _instance;

        internal static GamesSoundsManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<GamesSoundsManager>();
                }
                return _instance;
            }
        }

        private AudioSource audioSource;

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
        }

        internal void PlayClip(AudioClip clip) {
            if (clip != null) {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
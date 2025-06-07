using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MainSoundsManager : MonoBehaviour {
    private static MainSoundsManager _instance;

    internal static MainSoundsManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<MainSoundsManager>();
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
using UnityEngine;
using UnityEngine.UI;

public class ArticleSpawner : MonoBehaviour {
    [SerializeField] private GameObject articlePrefab;
    [SerializeField] private GameObject articleTimer;
    [SerializeField] private TimeManager articleTimeManager;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private AudioClip spawnClip;

    public Article currentArticle {
        get {
            return currentArticleInstance;
        }
    }

    private float autoDecisionTime;
    private bool isVisible = true;
    private Article currentArticleInstance;

    public void ToggleVisibility(bool turnOn) {
        isVisible = turnOn;
        articleTimer.SetActive(turnOn);

        foreach (Article article in GetComponentsInChildren<Article>()) {
            foreach (Transform child in article.transform) {
                child.gameObject.SetActive(turnOn);
            }
        }

        foreach (Transform child in transform) {
            if (child.TryGetComponent(out Image image)) {
                image.enabled = turnOn;
            }
        }
    }

    public void SpawnArticle(float delay = 1f) {
        Invoke(nameof(InstantiateArticle), delay);
    }

    private void InstantiateArticle() {
        Debug.Log($"Instantiating article with {autoDecisionTime} seconds...");
        articleTimeManager.SetCountdownDuration(autoDecisionTime);
        articleTimer.SetActive(isVisible);

        if (isVisible) { 
            MainSoundsManager.Instance.PlayClip(spawnClip);
        }

        GameObject newArticle = Instantiate(articlePrefab, spawnTransform);
        currentArticleInstance = newArticle.GetComponent<Article>();

        foreach (Transform child in newArticle.transform) {
            child.gameObject.SetActive(isVisible);
        }
    }

    public void SetAutoDecisionTime(float time) {
        Debug.Log($"Setting auto decision time to {time} seconds.");
        autoDecisionTime = time;
    }
}

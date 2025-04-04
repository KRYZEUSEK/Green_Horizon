using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticleSpawner : MonoBehaviour {
    public GameObject articlePrefab;
    private bool isVisible = true;

    public void ToggleVisibility(bool turnOn) {
        isVisible = turnOn;

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
        GameObject newArticle = Instantiate(articlePrefab, transform);

        foreach (Transform child in newArticle.transform) {
            child.gameObject.SetActive(isVisible);
        }
    }
}
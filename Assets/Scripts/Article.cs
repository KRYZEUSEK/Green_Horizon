using Cards;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Article : MonoBehaviour {
    [Header("Text fields")]
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text[] decisions = new TMP_Text[3];

    [Header("Settings")]
    [Tooltip("Time to wait after decision is made before hiding the article")]
    public float waitAfterDecision = 1f;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

        title.text = CardsManager.Instance.CurrentCard.card.Title;
        description.text = CardsManager.Instance.CurrentCard.card.Description;
        decisions[0].text = CardsManager.Instance.CurrentCard.card.Decisions[0].Description;
        decisions[1].text = CardsManager.Instance.CurrentCard.card.Decisions[1].Description;
        decisions[2].text = CardsManager.Instance.CurrentCard.card.Decisions[2].Description;

        //DrawArticle();
    }

    public void ChooseDecision(int i) {
        GameManager.Instance.ChooseDecision(i);
        //Invoke(nameof(HideArticle), waitAfterDecision);
        Invoke(nameof(DestroyAfterHide), waitAfterDecision);
    }

    public void DrawArticle() {
        animator.Play("DrawArticle");
    }

    public void HideArticle() {
        animator.Play("HideArticle");
    }

    public void DestroyAfterHide() {
        GameManager.Instance.ToggleVisibility(false);
        Destroy(gameObject);
    }
}
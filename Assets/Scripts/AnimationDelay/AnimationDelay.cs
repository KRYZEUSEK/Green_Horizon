using System.Collections;
using UnityEngine;

public class DelayedAnimationOnSelf : MonoBehaviour
{
    [Tooltip("Nazwa triggera w Animatorze, który ma zostaæ uruchomiony")]
    public string animationTriggerName = "Start";

    [Tooltip("Minimalne opóŸnienie w sekundach")]
    public float minDelay = 1f;

    [Tooltip("Maksymalne opóŸnienie w sekundach")]
    public float maxDelay = 4f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Brak komponentu Animator na obiekcie " + gameObject.name);
            return;
        }

        float delay = Random.Range(minDelay, maxDelay);
        StartCoroutine(PlayAnimationAfterDelay(delay));
    }

    private IEnumerator PlayAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(animationTriggerName);
    }
}

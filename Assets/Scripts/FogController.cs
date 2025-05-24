using System.Collections;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("Fog Settings")]
    public float targetFogDensity = 0.05f;
    public float fadeDuration = 2f;

    private Coroutine fogRoutine;

    private void Start()
    {
        RenderSettings.fog = false;
        RenderSettings.fogDensity = 0f;
    }

    public void FadeInFog()
    {
        if (fogRoutine != null) StopCoroutine(fogRoutine);
        fogRoutine = StartCoroutine(FadeFog(0f, targetFogDensity, true));
    }

    public void FadeOutFog()
    {
        if (fogRoutine != null) StopCoroutine(fogRoutine);
        fogRoutine = StartCoroutine(FadeFog(RenderSettings.fogDensity, 0f, false));
    }

    private IEnumerator FadeFog(float from, float to, bool enableFog)
    {
        RenderSettings.fog = true;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            RenderSettings.fogDensity = Mathf.Lerp(from, to, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        RenderSettings.fogDensity = to;

        if (!enableFog)
            RenderSettings.fog = false;

        fogRoutine = null;
    }
}


using UnityEngine;
using System.Collections;

public class CanvasFadeManager : MonoBehaviour
{
    public static CanvasFadeManager Instance { get; private set; }

    public CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void FadeIn(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(0f, 1f, duration));
    }

    public void FadeOut(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(1f, 0f, duration));
    }

    public void FadeInThenOut(float fadeInDuration, float waitTime, float fadeOutDuration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOutSequence(fadeInDuration, waitTime, fadeOutDuration));
    }

    private IEnumerator FadeCanvas(float startAlpha, float endAlpha, float duration)
    {
        if (canvasGroup == null)
            yield break;

        float time = 0f;
        canvasGroup.alpha = startAlpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    private IEnumerator FadeInOutSequence(float fadeInDuration, float waitTime, float fadeOutDuration)
    {
        yield return StartCoroutine(FadeCanvas(0f, 1f, fadeInDuration));
        yield return new WaitForSeconds(waitTime);
        yield return StartCoroutine(FadeCanvas(1f, 0f, fadeOutDuration));
    }
}

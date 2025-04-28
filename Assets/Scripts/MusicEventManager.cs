using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicEventManager : MonoBehaviour
{
    [System.Serializable]
    public class LyricEvent
    {
        public float triggerTime;
        public string lyricText;
    }

    public AudioSource audioSource;
    public TextMeshProUGUI uiText;
    public List<LyricEvent> lyricEvents = new List<LyricEvent>();

    private int currentIndex = 0;
    private bool isFading = false;
    private bool isActive = false;

    private void Awake()
    {
        uiText.text = "";
        uiText.alpha = 0f;
        enabled = true; // Disable this component by default
    }

    private void OnEnable()
    {
        // Reset everything clean when re-enabled
        currentIndex = 0;
        isFading = false;
        uiText.text = "";
        uiText.alpha = 0f;
        isActive = true;
    }

    private void Update()
    {
        if (!isActive || audioSource == null || currentIndex >= lyricEvents.Count)
            return;

        if (audioSource.time >= lyricEvents[currentIndex].triggerTime)
        {
            StartCoroutine(FadeToNewText(lyricEvents[currentIndex].lyricText));
            currentIndex++;
        }
    }

    private IEnumerator FadeToNewText(string newText)
    {
        if (isFading)
            yield break;

        isFading = true;

        // Fade out
        yield return StartCoroutine(FadeTextAlpha(0f, 0.5f));

        // Change text
        uiText.text = newText;

        // Fade in
        yield return StartCoroutine(FadeTextAlpha(1f, 0.5f));

        isFading = false;
    }

    private IEnumerator FadeTextAlpha(float targetAlpha, float duration)
    {
        float startAlpha = uiText.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            uiText.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        uiText.alpha = targetAlpha;
    }
}

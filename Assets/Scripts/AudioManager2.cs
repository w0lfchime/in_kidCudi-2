using System.Collections;
using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    public static AudioManager2 Instance { get; private set; }

    [Header("Audio Settings")]
    public AudioSource sfxSource;
    public AudioClip[] soundEffects;

    [Header("Canvas Groups")]
    public CanvasGroup canvasGroup1; // Assign in Inspector
    public CanvasGroup canvasGroup2; // Assign in Inspector

    public static int itemsLeft = 4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void PlaySFX(AudioClip clip)
    {
        if (Instance == null || Instance.sfxSource == null || clip == null)
            return;

        Instance.sfxSource.PlayOneShot(clip);
    }

    public static void PlaySFXByIndex(int index)
    {
        if (Instance == null || Instance.sfxSource == null)
            return;

        if (index == 0)
        {
            itemsLeft--;

            if (itemsLeft == 0)
            {
                // Start fading when all items are used
                Instance.StartCoroutine(Instance.FadeCanvasGroup(Instance.canvasGroup1, 3f));
                Instance.StartCoroutine(Instance.FadeCanvasGroup(Instance.canvasGroup2, 5f));
            }
        }

        if (index >= 0 && index < Instance.soundEffects.Length)
        {
            Instance.sfxSource.PlayOneShot(Instance.soundEffects[index]);
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float duration)
    {
        if (cg == null)
            yield break;

        float elapsed = 0f;
        float startAlpha = cg.alpha;
        float targetAlpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }

        cg.alpha = targetAlpha;
    }
}

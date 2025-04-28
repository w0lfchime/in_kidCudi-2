using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    public static AudioManager2 Instance { get; private set; }

    [Header("Audio Settings")]
    public AudioSource sfxSource; // Assign in inspector or create dynamically
    public AudioClip[] soundEffects; // Assign clips in inspector and reference by index or name

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persists across scenes
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

        if (index >= 0 && index < Instance.soundEffects.Length)
            Instance.sfxSource.PlayOneShot(Instance.soundEffects[index]);
    }
}

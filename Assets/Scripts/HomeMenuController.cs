using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeMenuController : MonoBehaviour
{
    public CanvasGroup uiCanvasGroup;
    public Button playButton;
    public Button exitButton;

    public Transform cameraTarget;
    public float cameraMoveDuration = 2.5f;

    public Camera mainCamera;
    public Renderer screenRenderer;
    public Texture newScreenTexture;

    [Header("Objects To Enable Later")]
    public List<GameObject> objectsToEnable = new List<GameObject>();

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayPressed);
        exitButton.onClick.AddListener(OnExitPressed);

        foreach (var obj in objectsToEnable)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    private void OnPlayPressed()
    {
        StartCoroutine(PlaySequence());
    }

    private void OnExitPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator PlaySequence()
    {
        // Fade out UI
        float fadeDuration = 1f;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            uiCanvasGroup.alpha = 1f - (t / fadeDuration);
            yield return null;
        }
        uiCanvasGroup.gameObject.SetActive(false);

        // Move camera smoothly
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;
        float startFOV = mainCamera.fieldOfView;

        Vector3 targetPos = cameraTarget.position;
        Quaternion targetRot = cameraTarget.rotation;
        float targetFOV = 52f;

        t = 0f;
        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            float lerpT = t / cameraMoveDuration;
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, lerpT);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, lerpT);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, lerpT);
            yield return null;
        }

        // Final snap to ensure exact end values
        mainCamera.transform.position = targetPos;
        mainCamera.transform.rotation = targetRot;
        mainCamera.fieldOfView = targetFOV;

        yield return new WaitForSeconds(0.1f);

        // Set screen texture
        if (screenRenderer != null && newScreenTexture != null)
        {
            screenRenderer.material.mainTexture = newScreenTexture;
        }

        // Enable the objects
        foreach (var obj in objectsToEnable)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}

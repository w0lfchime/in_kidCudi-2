using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFadeToWhite : MonoBehaviour
{
    public List<Renderer> objectsToWhite = new List<Renderer>();
    public float fadeDuration = 3f;
    private bool triggered = false;

    private void Update()
    {
        if (!triggered && Time.time >= 10f) // trigger at 10 seconds for example
        {
            triggered = true;
            StartCoroutine(FadeAllToWhite());
        }
    }

    private IEnumerator FadeAllToWhite()
    {
        float timer = 0f;

        // Cache original colors
        Dictionary<Material, Color> originalColors = new Dictionary<Material, Color>();
        foreach (var rend in objectsToWhite)
        {
            foreach (var mat in rend.materials)
            {
                if (!originalColors.ContainsKey(mat))
                    originalColors.Add(mat, mat.color);
            }
        }

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            foreach (var pair in originalColors)
            {
                pair.Key.color = Color.Lerp(pair.Value, Color.white, t);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Force set to pure white at the end
        foreach (var pair in originalColors)
        {
            pair.Key.color = Color.white;
        }
    }
}

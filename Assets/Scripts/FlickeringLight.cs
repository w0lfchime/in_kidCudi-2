using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    public float minIntensity = 0.5f;   // Minimum light intensity
    public float maxIntensity = 2f;     // Maximum light intensity
    public float flickerSpeed = 0.1f;   // Speed of flickering

    private Light pointLight;
    private float timer;

    void Start()
    {
        pointLight = GetComponent<Light>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            pointLight.intensity = Random.Range(minIntensity, maxIntensity);
            timer = Random.Range(0f, flickerSpeed);
        }
    }
}

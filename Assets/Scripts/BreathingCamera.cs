using UnityEngine;

public class BreathingCamera : MonoBehaviour
{
    [Header("Breathing Settings")]
    public float pitchAmplitude = 1.0f; // Up/down tilt
    public float pitchFrequency = 0.5f;

    public float yawAmplitude = 0.5f; // Left/right turn
    public float yawFrequency = 0.3f;

    public float rollAmplitude = 0.3f; // Roll (side tilt)
    public float rollFrequency = 0.4f;

    public float smoothSpeed = 2f;

    private Quaternion startLocalRot;

    private void Start()
    {
        startLocalRot = transform.localRotation;
    }

    private void Update()
    {
        float pitch = Mathf.Sin(Time.time * pitchFrequency) * pitchAmplitude;
        float yaw = Mathf.Sin(Time.time * yawFrequency) * yawAmplitude;
        float roll = Mathf.Sin(Time.time * rollFrequency) * rollAmplitude;

        Quaternion targetRot = startLocalRot * Quaternion.Euler(pitch, yaw, roll);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * smoothSpeed);
    }
}

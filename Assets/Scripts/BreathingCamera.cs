using UnityEngine;

public class BreathingCamera : MonoBehaviour
{
    [Header("Breathing Settings")]
    public float verticalAmplitude = 0.05f;
    public float verticalFrequency = 1.2f;

    public float swayAmplitude = 0.02f;
    public float swayFrequency = 0.8f;

    public float rotationAmplitude = 1f;
    public float rotationFrequency = 0.5f;

    public float smoothSpeed = 2f;

    private Vector3 startLocalPos;
    private Quaternion startLocalRot;

    private void Start()
    {

    }

    private void Update()
    {
        startLocalPos = transform.localPosition;
        startLocalRot = transform.localRotation;
        // Breathing motions
        float verticalOffset = Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        float swayOffset = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;
        float rotationOffset = Mathf.Sin(Time.time * rotationFrequency) * rotationAmplitude;

        // Target local position and rotation
        Vector3 targetLocalPos = startLocalPos + new Vector3(swayOffset, verticalOffset, 0f);
        Quaternion targetLocalRot = startLocalRot * Quaternion.Euler(0f, 0f, rotationOffset);

        // Smooth transition
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPos, Time.deltaTime * smoothSpeed);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLocalRot, Time.deltaTime * smoothSpeed);
    }
}

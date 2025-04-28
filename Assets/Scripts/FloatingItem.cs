using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float rotationSpeed = 50f; // degrees per second
    public float bobbingAmplitude = 1.25f; // how far up and down
    public float bobbingSpeed = 0.05f; // cycles per second

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Rotate slowly
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Bob up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed * Mathf.PI * 2) * bobbingAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}

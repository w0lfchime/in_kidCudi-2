using UnityEngine;

public class RotateX : MonoBehaviour
{
    public float rotationSpeed = 1300f; // degrees per second

    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}

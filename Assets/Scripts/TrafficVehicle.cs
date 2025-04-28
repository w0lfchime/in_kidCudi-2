using UnityEngine;

public class TrafficVehicle : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed;

    public void Initialize(Vector3 destination, float speed)
    {
        targetPosition = destination;
        moveSpeed = speed;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}

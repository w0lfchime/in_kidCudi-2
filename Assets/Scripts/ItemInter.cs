using UnityEngine;
using PSXShaderKit;

public class PickupItem : MonoBehaviour
{
    public float pickupRange = 20.0f;
    public string playerTag = "Player";

    public AudioSource source;
    public float movementSpeedDecreaseAmount = 0.5f; // Amount to decrease per pickup
    public float minMoveSpeed = 2.0f; // Minimum movement speed

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= pickupRange)
        {
            if (CanvasFadeManager.Instance != null)
            {
                AudioManager2.PlaySFXByIndex(0);
            }

            gameObject.SetActive(false); // Disable the item
            source.pitch -= 0.1f;


            // Find the FirstPersonController script and reduce moveSpeed
            FirstPersonController playerController = player.GetComponent<FirstPersonController>();
            if (playerController != null)
            {
                playerController.moveSpeed = Mathf.Max(minMoveSpeed, playerController.moveSpeed - movementSpeedDecreaseAmount);
            }
        }
    }
}

            

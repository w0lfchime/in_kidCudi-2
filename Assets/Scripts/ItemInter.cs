using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public float pickupRange = 20.0f;
    public string playerTag = "Player";

    //reference to tmpro ui text

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
        }
    }
}

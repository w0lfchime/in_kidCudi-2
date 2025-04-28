using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Head Bobbing")]
    public float bobSpeed = 14f;
    public float bobAmount = 0.05f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    private float defaultYPos;
    private float bobTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        defaultYPos = cameraTransform.localPosition.y;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleHeadBobbing();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleHeadBobbing()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        if (controller.isGrounded && isMoving)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmount;
            Vector3 localPos = cameraTransform.localPosition;
            localPos.y = defaultYPos + bobOffset;
            cameraTransform.localPosition = localPos;
        }
        else
        {
            // Smoothly reset to original Y position
            Vector3 localPos = cameraTransform.localPosition;
            localPos.y = Mathf.Lerp(localPos.y, defaultYPos, Time.deltaTime * bobSpeed);
            cameraTransform.localPosition = localPos;
        }
    }
}

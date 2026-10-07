using UnityEngine;
using UnityEngine.InputSystem;

namespace DoNotRemember.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float gravity = -20f;

        [Header("Mouse Look")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float mouseSensitivity = 0.12f;
        [SerializeField] private float maxLookAngle = 85f;

        private CharacterController controller;

        private float verticalVelocity;
        private float cameraPitch;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            LockCursor();
        }

        private void Update()
        {
            HandleCursor();
            HandleMovement();

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                HandleMouseLook();
            }
        }

        private void HandleMovement()
        {
            if (Keyboard.current == null)
                return;

            Vector2 input = Vector2.zero;

            if (Keyboard.current.wKey.isPressed)
                input.y += 1f;

            if (Keyboard.current.sKey.isPressed)
                input.y -= 1f;

            if (Keyboard.current.dKey.isPressed)
                input.x += 1f;

            if (Keyboard.current.aKey.isPressed)
                input.x -= 1f;

            input = Vector2.ClampMagnitude(input, 1f);

            Vector3 movement =
                transform.forward * input.y +
                transform.right * input.x;

            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            verticalVelocity += gravity * Time.deltaTime;

            movement *= moveSpeed;
            movement.y = verticalVelocity;

            controller.Move(movement * Time.deltaTime);
        }

        private void HandleMouseLook()
        {
            if (Mouse.current == null || cameraTransform == null)
                return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            float mouseX = mouseDelta.x * mouseSensitivity;
            float mouseY = mouseDelta.y * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(
                cameraPitch,
                -maxLookAngle,
                maxLookAngle
            );

            cameraTransform.localRotation =
                Quaternion.Euler(cameraPitch, 0f, 0f);
        }

        private void HandleCursor()
        {
            if (Keyboard.current != null &&
                Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                UnlockCursor();
            }

            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame &&
                Cursor.lockState != CursorLockMode.Locked)
            {
                LockCursor();
            }
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
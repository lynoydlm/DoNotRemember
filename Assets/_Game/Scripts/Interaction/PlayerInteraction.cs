using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace DoNotRemember.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private TMP_Text interactionPrompt;

        [Header("Settings")]
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private float interactionRadius = 0.12f;

        private Interactable currentInteractable;

        private void Update()
        {
            FindInteractable();

            if (currentInteractable != null &&
                Keyboard.current != null &&
                Keyboard.current.fKey.wasPressedThisFrame)
            {
                currentInteractable.Interact();
            }
        }

        private void FindInteractable()
        {
            currentInteractable = null;

            if (playerCamera == null)
            {
                UpdatePrompt();
                return;
            }

            Ray ray = new Ray(
                playerCamera.transform.position,
                playerCamera.transform.forward
            );

            RaycastHit[] hits = Physics.SphereCastAll(
                ray,
                interactionRadius,
                interactionDistance
            );

            float closestDistance = float.MaxValue;

            foreach (RaycastHit hit in hits)
            {
                Interactable interactable =
                    hit.collider.GetComponentInParent<Interactable>();

                if (interactable == null)
                    continue;

                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    currentInteractable = interactable;
                }
            }

            UpdatePrompt();
        }

        private void UpdatePrompt()
        {
            if (interactionPrompt == null)
                return;

            if (currentInteractable == null)
            {
                interactionPrompt.text = "";
                return;
            }

            interactionPrompt.text =
                $"[F] {currentInteractable.InteractionText}";
        }
    }
}
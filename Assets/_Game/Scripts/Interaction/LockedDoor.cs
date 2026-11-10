using DoNotRemember.Anchor;
using DoNotRemember.Core;
using UnityEngine;

namespace DoNotRemember.Interaction
{
    public class LockedDoor : Interactable
    {
        [Header("Lock")]
        [SerializeField] private MemoryObjectData requiredObject;

        [Header("Door")]
        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float openSpeed = 3f;

        private bool isUnlocked;
        private bool isOpen;

        private Quaternion closedRotation;
        private Quaternion targetRotation;

        private void Awake()
        {
            closedRotation = transform.localRotation;
            targetRotation = closedRotation;
        }

        private void Update()
        {
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                openSpeed * Time.deltaTime
            );
        }

        public override void Interact()
        {
            if (isOpen)
                return;

            if (!isUnlocked)
            {
                TryUnlock();
                return;
            }

            OpenDoor();
        }

        private void TryUnlock()
        {
            if (InventoryManager.Instance == null ||
                AnchorManager.Instance == null)
            {
                Debug.LogError(
                    "LockedDoor: required systems were not found."
                );

                return;
            }

            if (requiredObject == null)
            {
                Debug.LogError(
                    $"{name}: Required Object is missing."
                );

                return;
            }

            if (!InventoryManager.Instance.HasItem(requiredObject))
            {
                InventoryManager.Instance.ShowNotification(
                    "The door is locked."
                );

                return;
            }

            if (!AnchorManager.Instance.IsAnchored(requiredObject))
            {
                InventoryManager.Instance.ShowNotification(
                    "The object is not anchored in this reconstruction."
                );

                return;
            }

            isUnlocked = true;

            InventoryManager.Instance.ShowNotification(
                "The key fits."
            );

            OpenDoor();
        }

        private void OpenDoor()
        {
            isOpen = true;

            targetRotation =
                closedRotation *
                Quaternion.Euler(
                    0f,
                    openAngle,
                    0f
                );
        }
    }
}
using DoNotRemember.Core;
using UnityEngine;

namespace DoNotRemember.Interaction
{
    public class LockedDoor : Interactable
    {
        [Header("Lock")]
        [SerializeField] private string requiredItemId = "apartment_key";

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
            if (InventoryManager.Instance == null)
            {
                Debug.LogError(
                    "LockedDoor: InventoryManager was not found."
                );

                return;
            }

            if (!InventoryManager.Instance.HasItem(requiredItemId))
            {
                InventoryManager.Instance.ShowNotification(
                    "The door is locked."
                );

                return;
            }

            if (!InventoryManager.Instance.IsAnchored(requiredItemId))
            {
                InventoryManager.Instance.ShowNotification(
                    "The key does not belong to this memory."
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
                Quaternion.Euler(0f, openAngle, 0f);
        }
    }
}
using DoNotRemember.Core;
using UnityEngine;

namespace DoNotRemember.Interaction
{
    public class PickupObject : Interactable
    {
        [Header("Object Data")]
        [SerializeField] private MemoryObjectData objectData;

        private Renderer[] itemRenderers;
        private Collider[] itemColliders;

        private void Awake()
        {
            itemRenderers =
                GetComponentsInChildren<Renderer>();

            itemColliders =
                GetComponentsInChildren<Collider>();
        }

        private void OnEnable()
        {
            RefreshVisibility();
        }

        public override void Interact()
        {
            if (InventoryManager.Instance == null)
            {
                Debug.LogError(
                    "PickupObject: InventoryManager was not found."
                );

                return;
            }

            if (objectData == null)
            {
                Debug.LogError(
                    $"{name}: MemoryObjectData is not assigned."
                );

                return;
            }

            bool added =
                InventoryManager.Instance.AddItem(objectData);

            if (!added)
                return;

            SetVisible(false);
        }

        private void RefreshVisibility()
        {
            if (InventoryManager.Instance == null ||
                objectData == null)
            {
                SetVisible(true);
                return;
            }

            bool playerHasItem =
                InventoryManager.Instance.HasItem(objectData);

            SetVisible(!playerHasItem);
        }

        private void SetVisible(bool visible)
        {
            foreach (Renderer itemRenderer in itemRenderers)
            {
                itemRenderer.enabled = visible;
            }

            foreach (Collider itemCollider in itemColliders)
            {
                itemCollider.enabled = visible;
            }
        }
    }
}
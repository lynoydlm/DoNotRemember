using DoNotRemember.Core;
using UnityEngine;

namespace DoNotRemember.Interaction
{
    public class PickupObject : Interactable
    {
        [Header("Item")]
        [SerializeField] private string itemId = "item";
        [SerializeField] private string itemName = "Item";

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

            bool added =
                InventoryManager.Instance.AddItem(
                    itemId,
                    itemName
                );

            if (!added)
                return;

            SetVisible(false);
        }

        private void RefreshVisibility()
        {
            if (InventoryManager.Instance == null)
            {
                SetVisible(true);
                return;
            }

            bool playerHasItem =
                InventoryManager.Instance.HasItem(itemId);

            SetVisible(!playerHasItem);
        }

        private void SetVisible(bool visible)
        {
            foreach (Renderer itemRenderer
                     in itemRenderers)
            {
                itemRenderer.enabled = visible;
            }

            foreach (Collider itemCollider
                     in itemColliders)
            {
                itemCollider.enabled = visible;
            }
        }
    }
}
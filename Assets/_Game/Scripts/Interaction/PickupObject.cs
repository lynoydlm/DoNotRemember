using DoNotRemember.Core;
using UnityEngine;

namespace DoNotRemember.Interaction
{
    public class PickupObject : Interactable
    {
        [Header("Item")]
        [SerializeField] private string itemId = "item";
        [SerializeField] private string itemName = "Item";

        public override void Interact()
        {
            if (InventoryManager.Instance == null)
            {
                Debug.LogError("PickupObject: InventoryManager was not found.");
                return;
            }

            bool added = InventoryManager.Instance.AddItem(
                itemId,
                itemName
            );

            if (!added)
                return;

            gameObject.SetActive(false);
        }
    }
}
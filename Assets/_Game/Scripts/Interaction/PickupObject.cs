using UnityEngine;

namespace DoNotRemember.Interaction
{
    public class PickupObject : Interactable
    {
        [Header("Item")]
        [SerializeField] private string itemName = "Item";

        public override void Interact()
        {
            Debug.Log($"Picked up: {itemName}");

            gameObject.SetActive(false);
        }
    }
}
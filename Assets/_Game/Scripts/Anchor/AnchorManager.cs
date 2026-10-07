using DoNotRemember.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DoNotRemember.Anchor
{
    public class AnchorManager : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                TryAnchorItem();
            }
        }

        private void TryAnchorItem()
        {
            if (InventoryManager.Instance == null)
            {
                Debug.LogError(
                    "AnchorManager: InventoryManager was not found."
                );

                return;
            }

            InventoryManager.Instance.AnchorLastCollectedItem();
        }
    }
}
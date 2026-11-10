using System;
using System.Collections.Generic;
using DoNotRemember.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DoNotRemember.Anchor
{
    public class AnchorManager : MonoBehaviour
    {
        public static AnchorManager Instance { get; private set; }

        public event Action<string> AnchorActivated;

        [Header("Anchor Capacity")]
        [Min(1)]
        [SerializeField] private int maxActiveAnchors = 1;

        private readonly List<string> activeAnchors = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                TryAnchorLastCollectedItem();
            }
        }

        public void TryAnchorLastCollectedItem()
        {
            if (InventoryManager.Instance == null)
            {
                Debug.LogError(
                    "AnchorManager: InventoryManager was not found."
                );

                return;
            }

            string itemId =
                InventoryManager.Instance.LastCollectedItemId;

            if (string.IsNullOrEmpty(itemId))
            {
                InventoryManager.Instance.ShowNotification(
                    "Nothing to anchor."
                );

                return;
            }

            TryActivateAnchor(itemId);
        }

        public bool TryActivateAnchor(string itemId)
        {
            if (InventoryManager.Instance == null)
                return false;

            if (!InventoryManager.Instance.HasItem(itemId))
                return false;

            if (IsAnchored(itemId))
            {
                InventoryManager.Instance.ShowNotification(
                    "This object is already anchored."
                );

                return false;
            }

            if (activeAnchors.Count >= maxActiveAnchors)
            {
                InventoryManager.Instance.ShowNotification(
                    "ANCHOR capacity reached."
                );

                return false;
            }

            activeAnchors.Add(itemId);

            string itemName =
                InventoryManager.Instance.GetItemName(itemId);

            InventoryManager.Instance.ShowNotification(
                $"{itemName} ANCHORED"
            );

            Debug.Log(
                $"ANCHOR activated: {itemId}"
            );

            AnchorActivated?.Invoke(itemId);

            return true;
        }

        public bool IsAnchored(string itemId)
        {
            return activeAnchors.Contains(itemId);
        }

        public void HandleMemoryShift()
        {
            if (InventoryManager.Instance == null)
                return;

            List<string> carriedItems =
                InventoryManager.Instance.GetItemIdsSnapshot();

            foreach (string itemId in carriedItems)
            {
                if (IsAnchored(itemId))
                    continue;

                string itemName =
                    InventoryManager.Instance.GetItemName(itemId);

                InventoryManager.Instance.RemoveItem(itemId);

                InventoryManager.Instance.ShowNotification(
                    $"{itemName} was lost in the memory shift"
                );

                Debug.Log(
                    $"Memory shift removed: {itemId}"
                );
            }
        }
    }
}
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

        public event Action<MemoryObjectData> AnchorActivated;

        [Header("Anchor Capacity")]
        [Min(1)]
        [SerializeField] private int maxActiveAnchors = 1;

        private readonly List<MemoryObjectData> activeAnchors = new();

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

            MemoryObjectData itemData =
                InventoryManager.Instance.LastCollectedItem;

            if (itemData == null)
            {
                InventoryManager.Instance.ShowNotification(
                    "Nothing to anchor."
                );

                return;
            }

            TryActivateAnchor(itemData);
        }

        public bool TryActivateAnchor(
            MemoryObjectData itemData)
        {
            if (InventoryManager.Instance == null ||
                itemData == null)
            {
                return false;
            }

            if (!InventoryManager.Instance.HasItem(itemData))
                return false;

            if (!itemData.CanBeAnchored)
            {
                InventoryManager.Instance.ShowNotification(
                    "This object cannot be anchored."
                );

                return false;
            }

            if (IsAnchored(itemData))
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

            activeAnchors.Add(itemData);

            InventoryManager.Instance.ShowNotification(
                $"{itemData.DisplayName} ANCHORED"
            );

            Debug.Log(
                $"ANCHOR activated: {itemData.ObjectId}"
            );

            AnchorActivated?.Invoke(itemData);

            return true;
        }

        public bool IsAnchored(
            MemoryObjectData itemData)
        {
            return itemData != null &&
                   activeAnchors.Contains(itemData);
        }

        public void HandleMemoryShift()
        {
            if (InventoryManager.Instance == null)
                return;

            List<MemoryObjectData> carriedItems =
                InventoryManager.Instance.GetItemsSnapshot();

            foreach (MemoryObjectData itemData in carriedItems)
            {
                if (IsAnchored(itemData))
                    continue;

                InventoryManager.Instance.RemoveItem(itemData);

                InventoryManager.Instance.ShowNotification(
                    $"{itemData.DisplayName} was lost in the memory shift"
                );

                Debug.Log(
                    $"Memory shift removed: {itemData.ObjectId}"
                );
            }
        }
    }
}
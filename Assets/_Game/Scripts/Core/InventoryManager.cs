using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DoNotRemember.Core
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        private class InventoryItem
        {
            public string Id;
            public string Name;
            public bool IsAnchored;

            public InventoryItem(string id, string name)
            {
                Id = id;
                Name = name;
                IsAnchored = false;
            }
        }

        [Header("UI")]
        [SerializeField] private TMP_Text notificationText;

        [Header("Notification")]
        [SerializeField] private float notificationDuration = 2.5f;

        private readonly Dictionary<string, InventoryItem> items = new();

        private string lastCollectedItemId;

        private float notificationTimer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            HideNotification();
        }

        private void Update()
        {
            if (notificationTimer <= 0f)
                return;

            notificationTimer -= Time.deltaTime;

            if (notificationTimer <= 0f)
            {
                HideNotification();
            }
        }

        public bool AddItem(string itemId, string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                Debug.LogError("InventoryManager: Item ID is empty.");
                return false;
            }

            if (items.ContainsKey(itemId))
                return false;

            InventoryItem newItem = new InventoryItem(
                itemId,
                itemName
            );

            items.Add(itemId, newItem);

            lastCollectedItemId = itemId;

            ShowNotification(
                $"{itemName} added\n[R] Anchor item"
            );

            Debug.Log($"Inventory: added {itemId}");

            return true;
        }

        public bool HasItem(string itemId)
        {
            return items.ContainsKey(itemId);
        }

        public bool IsAnchored(string itemId)
        {
            if (!items.TryGetValue(itemId, out InventoryItem item))
                return false;

            return item.IsAnchored;
        }

        public bool AnchorLastCollectedItem()
        {
            if (string.IsNullOrEmpty(lastCollectedItemId))
                return false;

            if (!items.TryGetValue(
                    lastCollectedItemId,
                    out InventoryItem item))
            {
                return false;
            }

            if (item.IsAnchored)
                return false;

            item.IsAnchored = true;

            ShowNotification(
                $"{item.Name} ANCHORED"
            );

            Debug.Log(
                $"Inventory: anchored {item.Id}"
            );

            return true;
        }

        public void RemoveUnanchoredItems()
        {
            List<string> itemsToRemove = new();

            foreach (KeyValuePair<string, InventoryItem> pair in items)
            {
                if (!pair.Value.IsAnchored)
                {
                    itemsToRemove.Add(pair.Key);
                }
            }

            foreach (string itemId in itemsToRemove)
            {
                InventoryItem item = items[itemId];

                items.Remove(itemId);

                Debug.Log(
                    $"Memory shift removed unanchored item: {item.Name}"
                );

                ShowNotification(
                    $"{item.Name} was lost in the memory shift"
                );
            }

            if (!string.IsNullOrEmpty(lastCollectedItemId) &&
                !items.ContainsKey(lastCollectedItemId))
            {
                lastCollectedItemId = null;
            }
        }

        public void ShowNotification(string message)
        {
            if (notificationText == null)
                return;

            notificationText.text = message;
            notificationText.gameObject.SetActive(true);

            notificationTimer = notificationDuration;
        }

        private void HideNotification()
        {
            if (notificationText == null)
                return;

            notificationText.text = "";
            notificationText.gameObject.SetActive(false);

            notificationTimer = 0f;
        }
    }
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DoNotRemember.Core
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [Header("UI")]
        [SerializeField] private TMP_Text notificationText;

        [Header("Notification")]
        [SerializeField] private float notificationDuration = 2.5f;

        private readonly Dictionary<string, string> items = new();

        private string lastCollectedItemId;
        private float notificationTimer;

        public string LastCollectedItemId => lastCollectedItemId;

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
                Debug.LogError(
                    "InventoryManager: Item ID is empty."
                );

                return false;
            }

            if (items.ContainsKey(itemId))
                return false;

            items.Add(itemId, itemName);

            lastCollectedItemId = itemId;

            ShowNotification(
                $"{itemName} added\n[R] Anchor item"
            );

            Debug.Log(
                $"Inventory: added {itemId}"
            );

            return true;
        }

        public bool HasItem(string itemId)
        {
            return items.ContainsKey(itemId);
        }

        public string GetItemName(string itemId)
        {
            if (items.TryGetValue(
                    itemId,
                    out string itemName))
            {
                return itemName;
            }

            return itemId;
        }

        public List<string> GetItemIdsSnapshot()
        {
            return new List<string>(items.Keys);
        }

        public bool RemoveItem(string itemId)
        {
            bool removed = items.Remove(itemId);

            if (removed &&
                lastCollectedItemId == itemId)
            {
                lastCollectedItemId = null;
            }

            return removed;
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
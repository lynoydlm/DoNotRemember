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

        private readonly HashSet<MemoryObjectData> items = new();

        private MemoryObjectData lastCollectedItem;
        private float notificationTimer;

        public MemoryObjectData LastCollectedItem => lastCollectedItem;

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

        public bool AddItem(MemoryObjectData itemData)
        {
            if (itemData == null)
            {
                Debug.LogError(
                    "InventoryManager: Item Data is missing."
                );

                return false;
            }

            if (items.Contains(itemData))
                return false;

            items.Add(itemData);

            lastCollectedItem = itemData;

            ShowNotification(
                $"{itemData.DisplayName} added\n[R] Anchor item"
            );

            Debug.Log(
                $"Inventory added: {itemData.ObjectId}"
            );

            return true;
        }

        public bool HasItem(MemoryObjectData itemData)
        {
            return itemData != null &&
                   items.Contains(itemData);
        }

        public bool RemoveItem(MemoryObjectData itemData)
        {
            if (itemData == null)
                return false;

            bool removed = items.Remove(itemData);

            if (removed &&
                lastCollectedItem == itemData)
            {
                lastCollectedItem = null;
            }

            return removed;
        }

        public List<MemoryObjectData> GetItemsSnapshot()
        {
            return new List<MemoryObjectData>(items);
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
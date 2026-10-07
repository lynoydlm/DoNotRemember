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
        [SerializeField] private float notificationDuration = 2f;

        private readonly HashSet<string> collectedItems = new();

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

            if (collectedItems.Contains(itemId))
                return false;

            collectedItems.Add(itemId);

            ShowNotification($"{itemName} added");

            Debug.Log($"Inventory: added {itemId}");

            return true;
        }

        public bool HasItem(string itemId)
        {
            return collectedItems.Contains(itemId);
        }

        public bool RemoveItem(string itemId)
        {
            return collectedItems.Remove(itemId);
        }

        private void ShowNotification(string message)
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
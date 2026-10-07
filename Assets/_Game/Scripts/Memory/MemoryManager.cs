using DoNotRemember.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DoNotRemember.Memory
{
    public class MemoryManager : MonoBehaviour
    {
        [Header("Memory Layers")]
        [SerializeField] private MemoryLayer[] memoryLayers;

        [Header("Starting Memory")]
        [SerializeField] private int startingMemoryIndex = 0;

        private int currentMemoryIndex;

        private bool memoryInitialized;

        public MemoryLayer CurrentMemory
        {
            get
            {
                if (memoryLayers == null ||
                    memoryLayers.Length == 0)
                {
                    return null;
                }

                return memoryLayers[currentMemoryIndex];
            }
        }

        private void Start()
        {
            if (memoryLayers == null ||
                memoryLayers.Length == 0)
            {
                Debug.LogError(
                    "MemoryManager: memory layers are not assigned."
                );

                return;
            }

            currentMemoryIndex = Mathf.Clamp(
                startingMemoryIndex,
                0,
                memoryLayers.Length - 1
            );

            ActivateMemory(
                currentMemoryIndex,
                false
            );

            memoryInitialized = true;
        }

        private void Update()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                NextMemory();
            }

            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                PreviousMemory();
            }
        }

        public void NextMemory()
        {
            if (memoryLayers == null ||
                memoryLayers.Length <= 1)
            {
                return;
            }

            int nextIndex = currentMemoryIndex + 1;

            if (nextIndex >= memoryLayers.Length)
            {
                nextIndex = 0;
            }

            ActivateMemory(nextIndex);
        }

        public void PreviousMemory()
        {
            if (memoryLayers == null ||
                memoryLayers.Length <= 1)
            {
                return;
            }

            int previousIndex = currentMemoryIndex - 1;

            if (previousIndex < 0)
            {
                previousIndex =
                    memoryLayers.Length - 1;
            }

            ActivateMemory(previousIndex);
        }

        public void ActivateMemory(
            int index,
            bool clearUnanchoredItems = true)
        {
            if (memoryLayers == null)
                return;

            if (index < 0 ||
                index >= memoryLayers.Length)
            {
                return;
            }

            if (memoryInitialized &&
                clearUnanchoredItems &&
                InventoryManager.Instance != null)
            {
                InventoryManager.Instance
                    .RemoveUnanchoredItems();
            }

            for (int i = 0;
                 i < memoryLayers.Length;
                 i++)
            {
                if (memoryLayers[i] == null)
                    continue;

                if (i == index)
                {
                    memoryLayers[i].Activate();
                }
                else
                {
                    memoryLayers[i].Deactivate();
                }
            }

            currentMemoryIndex = index;

            Debug.Log(
                $"Memory changed: " +
                $"{CurrentMemory.MemoryTime} — " +
                $"{CurrentMemory.MemoryName}"
            );
        }
    }
}
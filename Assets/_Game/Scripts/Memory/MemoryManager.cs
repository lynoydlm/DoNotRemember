using System;
using DoNotRemember.Anchor;
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

        [Header("Transition")]
        [SerializeField] private MemoryTransition memoryTransition;

        private int currentMemoryIndex;

        private bool memoryInputEnabled = true;

        public event Action<MemoryLayer> MemoryChanged;

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

            ApplyMemory(
                currentMemoryIndex,
                false
            );
        }

        private void Update()
        {
            if (!memoryInputEnabled)
                return;

            if (Keyboard.current == null)
                return;

            if (memoryTransition != null &&
                memoryTransition.IsTransitioning)
            {
                return;
            }

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

            int nextIndex =
                currentMemoryIndex + 1;

            if (nextIndex >= memoryLayers.Length)
            {
                nextIndex = 0;
            }

            RequestMemoryChange(nextIndex);
        }

        public void PreviousMemory()
        {
            if (memoryLayers == null ||
                memoryLayers.Length <= 1)
            {
                return;
            }

            int previousIndex =
                currentMemoryIndex - 1;

            if (previousIndex < 0)
            {
                previousIndex =
                    memoryLayers.Length - 1;
            }

            RequestMemoryChange(previousIndex);
        }

        private void RequestMemoryChange(int index)
        {
            if (index < 0 ||
                index >= memoryLayers.Length)
            {
                return;
            }

            if (memoryTransition != null)
            {
                memoryTransition.PlayTransition(
                    () => ApplyMemory(index, true)
                );
            }
            else
            {
                ApplyMemory(index, true);
            }
        }

        private void ApplyMemory(
            int index,
            bool handleMemoryShift)
        {
            if (handleMemoryShift &&
                AnchorManager.Instance != null)
            {
                AnchorManager.Instance.HandleMemoryShift();
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

            MemoryChanged?.Invoke(CurrentMemory);
        }

        public void SetMemoryInputEnabled(bool enabled)
        {
            memoryInputEnabled = enabled;
        }
    }
}
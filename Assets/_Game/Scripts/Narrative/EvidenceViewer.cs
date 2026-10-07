using DoNotRemember.Interaction;
using DoNotRemember.Memory;
using DoNotRemember.Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DoNotRemember.Narrative
{
    public class EvidenceViewer : MonoBehaviour
    {
        public static EvidenceViewer Instance { get; private set; }

        [Header("UI")]
        [SerializeField] private GameObject evidencePanel;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;

        [Header("Player")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerInteraction playerInteraction;

        [Header("Memory")]
        [SerializeField] private MemoryManager memoryManager;

        private bool isOpen;

        public bool IsOpen => isOpen;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            CloseEvidence();
        }

        private void Update()
        {
            if (!isOpen || Keyboard.current == null)
                return;

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseEvidence();
            }
        }

        public void ShowEvidence(
            string title,
            string description)
        {
            if (evidencePanel == null)
            {
                Debug.LogError(
                    "EvidenceViewer: Evidence Panel is not assigned."
                );

                return;
            }

            if (titleText == null ||
                descriptionText == null)
            {
                Debug.LogError(
                    "EvidenceViewer: Text fields are not assigned."
                );

                return;
            }

            titleText.text = title;
            descriptionText.text = description;

            evidencePanel.SetActive(true);
            isOpen = true;

            if (playerController != null)
            {
                playerController.SetControlsEnabled(false);
            }

            if (playerInteraction != null)
            {
                playerInteraction.SetInteractionEnabled(false);
            }

            if (memoryManager != null)
            {
                memoryManager.SetMemoryInputEnabled(false);
            }
        }

        public void CloseEvidence()
        {
            if (evidencePanel != null)
            {
                evidencePanel.SetActive(false);
            }

            isOpen = false;

            if (playerController != null)
            {
                playerController.SetControlsEnabled(true);
            }

            if (playerInteraction != null)
            {
                playerInteraction.SetInteractionEnabled(true);
            }

            if (memoryManager != null)
            {
                memoryManager.SetMemoryInputEnabled(true);
            }
        }
    }
}
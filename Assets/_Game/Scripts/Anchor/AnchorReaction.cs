using DoNotRemember.Core;
using UnityEngine;

namespace DoNotRemember.Anchor
{
    public class AnchorReaction : MonoBehaviour
    {
        [Header("Condition")]
        [SerializeField] private string requiredItemId;

        [Header("Reaction")]
        [SerializeField] private GameObject targetObject;

        [SerializeField] private bool showWhenAnchored = true;

        private bool hasStarted;
        private bool subscribed;

        private void Start()
        {
            hasStarted = true;

            Subscribe();
            EvaluateReaction();
        }

        private void OnEnable()
        {
            if (!hasStarted)
                return;

            Subscribe();
            EvaluateReaction();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (subscribed)
                return;

            if (AnchorManager.Instance == null)
                return;

            AnchorManager.Instance.AnchorActivated += OnAnchorActivated;

            subscribed = true;
        }

        private void Unsubscribe()
        {
            if (!subscribed)
                return;

            if (AnchorManager.Instance != null)
            {
                AnchorManager.Instance.AnchorActivated -= OnAnchorActivated;
            }

            subscribed = false;
        }

        private void OnAnchorActivated(string itemId)
        {
            if (itemId != requiredItemId)
                return;

            EvaluateReaction();
        }

        private void EvaluateReaction()
        {
            if (targetObject == null)
                return;

            if (AnchorManager.Instance == null)
            {
                targetObject.SetActive(!showWhenAnchored);
                return;
            }

            bool anchored =
                AnchorManager.Instance.IsAnchored(requiredItemId);

            targetObject.SetActive(
                showWhenAnchored
                    ? anchored
                    : !anchored
            );
        }
    }
}
using DoNotRemember.Interaction;
using UnityEngine;

namespace DoNotRemember.Narrative
{
    public class EvidenceObject : Interactable
    {
        [Header("Evidence")]
        [SerializeField] private string evidenceTitle = "Evidence";

        [TextArea(3, 8)]
        [SerializeField] private string evidenceDescription;

        public override void Interact()
        {
            if (EvidenceViewer.Instance == null)
            {
                Debug.LogError("EvidenceViewer was not found.");
                return;
            }

            EvidenceViewer.Instance.ShowEvidence(
                evidenceTitle,
                evidenceDescription
            );
        }
    }
}
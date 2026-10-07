using UnityEngine;

namespace DoNotRemember.Interaction
{
    public abstract class Interactable : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private string interactionText = "Interact";

        public string InteractionText => interactionText;

        public abstract void Interact();
    }
}
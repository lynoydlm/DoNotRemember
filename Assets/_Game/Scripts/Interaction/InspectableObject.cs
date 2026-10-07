using UnityEngine;

namespace DoNotRemember.Interaction
{
    public class InspectableObject : Interactable
    {
        [Header("Inspection")]
        [SerializeField] private string objectName = "Object";

        [TextArea(2, 5)]
        [SerializeField] private string description = "Nothing unusual.";

        public override void Interact()
        {
            Debug.Log($"{objectName}: {description}");
        }
    }
}
using UnityEngine;

namespace DoNotRemember.Core
{
    [CreateAssetMenu(
        fileName = "Object_",
        menuName = "Do Not Remember/Memory Object Data"
    )]
    public class MemoryObjectData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string objectId;
        [SerializeField] private string displayName;

        [Header("ANCHOR")]
        [SerializeField] private bool canBeAnchored = true;

        public string ObjectId => objectId;
        public string DisplayName => displayName;
        public bool CanBeAnchored => canBeAnchored;
    }
}
using UnityEngine;

namespace DoNotRemember.Memory
{
    public class MemoryLayer : MonoBehaviour
    {
        [Header("Memory Info")]
        [SerializeField] private string memoryName = "Memory";
        [SerializeField] private string memoryTime = "00:00";

        public string MemoryName => memoryName;
        public string MemoryTime => memoryTime;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
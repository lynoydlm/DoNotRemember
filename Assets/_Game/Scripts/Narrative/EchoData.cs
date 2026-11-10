using UnityEngine;

namespace DoNotRemember.Narrative
{
    public enum EchoType
    {
        Memory,
        Present
    }

    [CreateAssetMenu(
        fileName = "Echo_",
        menuName = "Do Not Remember/Narrative/Echo Data"
    )]
    public class EchoData : ScriptableObject
    {
        [Header("Type")]
        [SerializeField] private EchoType echoType;

        [Header("Content")]
        [SerializeField] private string speaker;

        [TextArea(2, 6)]
        [SerializeField] private string subtitle;

        [Header("Audio")]
        [SerializeField] private AudioClip audioClip;

        [Header("Fallback")]
        [SerializeField] private float fallbackDuration = 3.5f;

        public EchoType Type => echoType;
        public string Speaker => speaker;
        public string Subtitle => subtitle;
        public AudioClip AudioClip => audioClip;
        public float FallbackDuration => fallbackDuration;
    }
}
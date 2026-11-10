using DoNotRemember.Interaction;
using UnityEngine;

namespace DoNotRemember.Narrative
{
    public class EchoInteractable : Interactable
    {
        [Header("Echo")]
        [SerializeField] private EchoData echoData;

        public override void Interact()
        {
            if (EchoPlayer.Instance == null)
            {
                Debug.LogError(
                    "EchoInteractable: EchoPlayer was not found."
                );

                return;
            }

            EchoPlayer.Instance.PlayEcho(echoData);
        }
    }
}
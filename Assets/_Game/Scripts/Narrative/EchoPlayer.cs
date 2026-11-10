using System.Collections;
using TMPro;
using UnityEngine;

namespace DoNotRemember.Narrative
{
    public class EchoPlayer : MonoBehaviour
    {
        public static EchoPlayer Instance { get; private set; }

        [Header("UI")]
        [SerializeField] private CanvasGroup echoCanvas;
        [SerializeField] private TMP_Text typeText;
        [SerializeField] private TMP_Text speakerText;
        [SerializeField] private TMP_Text subtitleText;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;

        [Header("Animation")]
        [SerializeField] private float fadeDuration = 0.25f;

        private Coroutine currentEcho;

        private void Awake()
        {
            Instance = this;

            if (echoCanvas != null)
            {
                echoCanvas.alpha = 0f;
            }
        }

        public void PlayEcho(EchoData data)
        {
            if (data == null)
                return;

            if (currentEcho != null)
            {
                StopCoroutine(currentEcho);
            }

            if (audioSource != null)
            {
                audioSource.Stop();
            }

            currentEcho = StartCoroutine(
                PlayEchoRoutine(data)
            );
        }

        private IEnumerator PlayEchoRoutine(EchoData data)
        {
            typeText.text = data.Type == EchoType.Memory
                ? "MEMORY ECHO"
                : "PRESENT ECHO";

            speakerText.text = data.Speaker;
            subtitleText.text = data.Subtitle;

            yield return Fade(0f, 1f);

            float duration = data.FallbackDuration;

            if (data.AudioClip != null &&
                audioSource != null)
            {
                audioSource.clip = data.AudioClip;
                audioSource.Play();

                duration = data.AudioClip.length;
            }

            yield return new WaitForSeconds(duration);

            yield return Fade(1f, 0f);

            currentEcho = null;
        }

        private IEnumerator Fade(float from, float to)
        {
            if (echoCanvas == null)
                yield break;

            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;

                echoCanvas.alpha = Mathf.Lerp(
                    from,
                    to,
                    Mathf.Clamp01(elapsed / fadeDuration)
                );

                yield return null;
            }

            echoCanvas.alpha = to;
        }
    }
}
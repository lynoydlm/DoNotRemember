using System;
using System.Collections;
using UnityEngine;

namespace DoNotRemember.Memory
{
    public class MemoryTransition : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private CanvasGroup fadeCanvas;

        [Header("Timing")]
        [SerializeField] private float fadeInDuration = 0.25f;
        [SerializeField] private float holdDuration = 0.08f;
        [SerializeField] private float fadeOutDuration = 0.4f;

        [Header("Appearance")]
        [Range(0f, 1f)]
        [SerializeField] private float maxFadeAlpha = 0.92f;

        public bool IsTransitioning { get; private set; }

        private void Awake()
        {
            if (fadeCanvas != null)
            {
                fadeCanvas.alpha = 0f;
            }
        }

        public void PlayTransition(Action changeMemory)
        {
            if (IsTransitioning)
                return;

            StartCoroutine(
                TransitionRoutine(changeMemory)
            );
        }

        private IEnumerator TransitionRoutine(
            Action changeMemory)
        {
            IsTransitioning = true;

            yield return Fade(
                0f,
                maxFadeAlpha,
                fadeInDuration
            );

            changeMemory?.Invoke();

            yield return new WaitForSeconds(
                holdDuration
            );

            yield return Fade(
                maxFadeAlpha,
                0f,
                fadeOutDuration
            );

            IsTransitioning = false;
        }

        private IEnumerator Fade(
            float from,
            float to,
            float duration)
        {
            if (fadeCanvas == null)
                yield break;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                float progress =
                    Mathf.Clamp01(elapsed / duration);

                fadeCanvas.alpha =
                    Mathf.Lerp(from, to, progress);

                yield return null;
            }

            fadeCanvas.alpha = to;
        }
    }
}
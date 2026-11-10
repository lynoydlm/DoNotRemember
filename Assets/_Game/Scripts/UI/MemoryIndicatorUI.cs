using System.Collections;
using DoNotRemember.Memory;
using TMPro;
using UnityEngine;

namespace DoNotRemember.UI
{
    public class MemoryIndicatorUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MemoryManager memoryManager;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text nameText;

        [Header("Animation")]
        [SerializeField] private float fadeDuration = 0.35f;
        [SerializeField] private float visibleDuration = 1.5f;

        private Coroutine animationCoroutine;

        private void Awake()
        {
            canvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            if (memoryManager != null)
            {
                memoryManager.MemoryChanged += ShowMemory;
            }
        }

        private void OnDisable()
        {
            if (memoryManager != null)
            {
                memoryManager.MemoryChanged -= ShowMemory;
            }
        }

        private void ShowMemory(MemoryLayer memory)
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            timeText.text = memory.MemoryTime;
            nameText.text = memory.MemoryName.ToUpper();

            animationCoroutine = StartCoroutine(
                AnimateIndicator()
            );
        }

        private IEnumerator AnimateIndicator()
        {
            yield return Fade(0f, 1f);

            yield return new WaitForSeconds(
                visibleDuration
            );

            yield return Fade(1f, 0f);

            animationCoroutine = null;
        }

        private IEnumerator Fade(
            float from,
            float to)
        {
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;

                float progress =
                    elapsed / fadeDuration;

                canvasGroup.alpha =
                    Mathf.Lerp(from, to, progress);

                yield return null;
            }

            canvasGroup.alpha = to;
        }
    }
}
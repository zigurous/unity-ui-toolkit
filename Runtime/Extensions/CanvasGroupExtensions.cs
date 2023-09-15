using System.Collections;
using UnityEngine;

namespace Zigurous.UI
{
    /// <summary>
    /// Extension methods for UI canvas groups.
    /// </summary>
    public static class CanvasGroupExtensions
    {
        /// <summary>
        /// Coroutine to fade the alpha of the canvas group to 100%.
        /// </summary>
        /// <param name="canvasGroup">The canvas group to fade.</param>
        /// <param name="duration">The amount of seconds it takes to fade the canvas group.</param>
        public static IEnumerator FadeIn(this CanvasGroup canvasGroup, float duration)
        {
            float elapsed = Mathf.Lerp(0f, duration, Mathf.InverseLerp(0f, 1f, canvasGroup.alpha));
            return Fade(canvasGroup, 0f, 1f, elapsed, duration);
        }

        /// <summary>
        /// Coroutine to fade the alpha of the canvas group to 0%.
        /// </summary>
        /// <param name="canvasGroup">The canvas group to fade.</param>
        /// <param name="duration">The amount of seconds it takes to fade the canvas group.</param>
        public static IEnumerator FadeOut(this CanvasGroup canvasGroup, float duration)
        {
            float elapsed = Mathf.Lerp(0f, duration, Mathf.InverseLerp(1f, 0f, canvasGroup.alpha));
            return Fade(canvasGroup, 1f, 0f, elapsed, duration);
        }

        /// <summary>
        /// Coroutine to fade the alpha of a canvas group.
        /// </summary>
        /// <param name="canvasGroup">The canvas group to fade.</param>
        /// <param name="alpha">The alpha value to fade to.</param>
        /// <param name="elapsed">The initial amount of seconds that have elapsed.</param>
        /// <param name="duration">The amount of seconds it takes to fade the canvas group.</param>
        private static IEnumerator Fade(CanvasGroup canvasGroup, float from, float to, float elapsed, float duration)
        {
            while (elapsed < duration)
            {
                float percent = elapsed / duration;
                canvasGroup.alpha = Mathf.Lerp(from, to, percent);
                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = to;
        }

    }

}

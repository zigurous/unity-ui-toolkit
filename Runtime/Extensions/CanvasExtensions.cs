using UnityEngine;
using UnityEngine.UI;

namespace Zigurous.UI
{
    /// <summary>
    /// Extension methods for UI canvas.
    /// </summary>
    public static class CanvasExtensions
    {
        /// <summary>
        /// Fades the alpha of all graphics of the canvas to 100% over the given
        /// duration.
        /// </summary>
        /// <param name="canvas">The canvas to fade.</param>
        /// <param name="duration">The amount of seconds it takes to fade the graphics.</param>
        /// <param name="ignoreTimeScale">Ignores the time scale when fading the graphics.</param>
        public static void FadeInGraphics(this Canvas canvas, float duration, bool ignoreTimeScale = false)
        {
            Graphic[] graphics = canvas.GetComponentsInChildren<Graphic>();

            for (int i = 0; i < graphics.Length; i++) {
                graphics[i].CrossFadeAlpha(1.0f, duration, ignoreTimeScale);
            }
        }

        /// <summary>
        /// Fades the alpha of all graphics of the canvas to 0% over the given
        /// duration.
        /// </summary>
        /// <param name="canvas">The canvas to fade.</param>
        /// <param name="duration">The amount of seconds it takes to fade the graphics.</param>
        /// <param name="ignoreTimeScale">Ignores the time scale when fading the graphics.</param>
        public static void FadeOutGraphics(this Canvas canvas, float duration, bool ignoreTimeScale = false)
        {
            Graphic[] graphics = canvas.GetComponentsInChildren<Graphic>();

            for (int i = 0; i < graphics.Length; i++) {
                graphics[i].CrossFadeAlpha(0.0f, duration, ignoreTimeScale);
            }
        }

    }

}

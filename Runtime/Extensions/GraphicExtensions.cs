using UnityEngine.UI;

namespace Zigurous.UI
{
    /// <summary>
    /// Extension methods for UI graphics.
    /// </summary>
    public static class GraphicExtensions
    {
        /// <summary>
        /// Fades the alpha of the graphic to 100% over the given duration.
        /// </summary>
        /// <param name="graphic">The graphic to fade.</param>
        /// <param name="duration">The amount of seconds it takes to fade the graphic.</param>
        /// <param name="ignoreTimeScale">Ignores the time scale when fading the graphic.</param>
        public static void FadeIn(this Graphic graphic, float duration, bool ignoreTimeScale = false)
        {
            graphic.CrossFadeAlpha(1.0f, duration, ignoreTimeScale);
        }

        /// <summary>
        /// Fades the alpha of the graphic to 0% over the given duration.
        /// </summary>
        /// <param name="graphic">The graphic to fade.</param>
        /// <param name="duration">The amount of seconds it takes to fade the graphic.</param>
        /// <param name="ignoreTimeScale">Ignores the time scale when fading the graphic.</param>
        public static void FadeOut(this Graphic graphic, float duration, bool ignoreTimeScale = false)
        {
            graphic.CrossFadeAlpha(0.0f, duration, ignoreTimeScale);
        }

    }

}

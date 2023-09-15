namespace Zigurous.UI
{
    public interface IShowableAnimatedDisplay : IShowableDisplay
    {
        bool IsAnimating { get; }

        void ShowAnimated();
        void ShowAnimated(float duration);

        void HideAnimated();
        void HideAnimated(float duration);
    }

    public static class IShowableAnimatedDisplayExtensions
    {
        public static void ToggleShowHideAnimated(this IShowableAnimatedDisplay display)
        {
            if (display.IsShown) {
                display.HideAnimated();
            } else {
                display.ShowAnimated();
            }
        }

        public static void ToggleShowHideAnimated(this IShowableAnimatedDisplay display, float duration)
        {
            if (display.IsShown) {
                display.HideAnimated(duration);
            } else {
                display.ShowAnimated(duration);
            }
        }

    }

}

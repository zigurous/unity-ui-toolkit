namespace Zigurous.UI
{
    public interface IExpandableAnimatedDisplay : IExpandableDisplay
    {
        bool IsAnimating { get; }

        void ExpandAnimated();
        void ExpandAnimated(float duration);

        void CollapseAnimated();
        void CollapseAnimated(float duration);
    }

    public static class IExpandableAnimatedDisplayExtensions
    {
        public static void ToggleExpandCollapseAnimated(this IExpandableAnimatedDisplay display)
        {
            if (display.IsExpanded) {
                display.CollapseAnimated();
            } else {
                display.ExpandAnimated();
            }
        }

        public static void ToggleExpandCollapseAnimated(this IExpandableAnimatedDisplay display, float duration)
        {
            if (display.IsExpanded) {
                display.CollapseAnimated(duration);
            } else {
                display.ExpandAnimated(duration);
            }
        }

    }

}

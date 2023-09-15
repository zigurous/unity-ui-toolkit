namespace Zigurous.UI
{
    public interface IExpandableDisplay : IInterfaceDisplay
    {
        bool IsExpanded { get; }
        bool IsCollapsed { get; }

        void Expand();
        void Collapse();
    }

    public static class IExpandableDisplayExtensions
    {
        public static void ToggleExpandCollapse(this IExpandableDisplay display)
        {
            if (display.IsExpanded) {
                display.Collapse();
            } else {
                display.Expand();
            }
        }

    }

}

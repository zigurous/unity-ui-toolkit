namespace Zigurous.UI
{
    public interface IShowableDisplay : IInterfaceDisplay
    {
        bool IsShown { get; }
        bool IsHidden { get; }

        void Show();
        void Hide();
    }

    public static class IShowableDisplayExtensions
    {
        public static void ToggleShowHide(this IShowableDisplay display)
        {
            if (display.IsShown) {
                display.Hide();
            } else {
                display.Show();
            }
        }

    }

}

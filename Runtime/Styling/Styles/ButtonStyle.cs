using UnityEngine;

namespace Zigurous.UI
{
    [CreateAssetMenu(menuName = "Zigurous/UI/Styling/Button Style")]
    public class ButtonStyle : ScriptableObject
    {
        public Color normalColor;
        public Color highlightedColor;
        public Color pressedColor;
        public Color selectedColor;
        public Color disabledColor;
    }

}

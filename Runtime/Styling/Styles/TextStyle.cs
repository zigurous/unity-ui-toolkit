using UnityEngine;

namespace Zigurous.UI
{
    [CreateAssetMenu(menuName = "Zigurous/UI/Styling/Text Style")]
    public class TextStyle : ScriptableObject
    {
        public int fontSize = 16;
        public FontStyle fontStyle = FontStyle.Normal;
        public ColorStyle color;
    }

}

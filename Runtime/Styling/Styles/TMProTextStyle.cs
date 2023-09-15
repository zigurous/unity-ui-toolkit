using TMPro;
using UnityEngine;

namespace Zigurous.UI
{
    [CreateAssetMenu(menuName = "Zigurous/UI/Styling/Text Style (TMPro)")]
    public class TMProTextStyle : ScriptableObject
    {
        public float fontSize = 16f;
        public FontWeight fontWeight = FontWeight.Regular;
        public ColorStyle color;
    }

}

using TMPro;
using UnityEngine;

namespace Zigurous.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class StyledTMProText : StyledComponent
    {
        private TextMeshProUGUI m_Text;
        public TextMeshProUGUI text
        {
            get
            {
                if (m_Text == null) {
                    m_Text = GetComponent<TextMeshProUGUI>();
                }
                return m_Text;
            }
        }

        [SerializeField] private TMProTextStyle m_TextStyle;
        public TMProTextStyle textStyle
        {
            get => m_TextStyle;
            set
            {
                m_TextStyle = value;
                RequestUpdate();
            }
        }

        [SerializeField] private ColorStyle m_ColorOverride;
        public ColorStyle colorOverride
        {
            get => m_ColorOverride;
            set
            {
                m_ColorOverride = value;
                RequestUpdate();
            }
        }

        public override void ApplyStyles()
        {
            var textStyle = this.textStyle;
            var colorOverride = this.colorOverride;

            if (textStyle != null)
            {
                text.fontSize = textStyle.fontSize;
                text.fontWeight = textStyle.fontWeight;

                if (textStyle.color != null) {
                    text.color = textStyle.color.value;
                }
            }

            if (colorOverride != null) {
                text.color = colorOverride.value;
            }
        }

    }

}

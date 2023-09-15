using UnityEngine;
using UnityEngine.UI;

namespace Zigurous.UI
{
    [RequireComponent(typeof(Text))]
    public class StyledText : StyledComponent
    {
        private Text m_Text;
        public Text text
        {
            get
            {
                if (m_Text == null) {
                    m_Text = GetComponent<Text>();
                }
                return m_Text;
            }
        }

        [SerializeField] private TextStyle m_TextStyle;
        public TextStyle textStyle
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

            if (textStyle != null)
            {
                text.fontSize = textStyle.fontSize;
                text.fontStyle = textStyle.fontStyle;

                if (textStyle.color != null) {
                    text.color = textStyle.color.value;
                }
            }
        }

    }

}

using UnityEngine;
using UnityEngine.UI;

namespace Zigurous.UI
{
    [RequireComponent(typeof(Button))]
    public class StyledButton : StyledComponent
    {
        private Button m_Button;
        public Button button
        {
            get
            {
                if (m_Button == null) {
                    m_Button = GetComponent<Button>();
                }
                return m_Button;
            }
        }

        [SerializeField] private ButtonStyle m_ButtonStyle;
        public ButtonStyle buttonStyle
        {
            get => m_ButtonStyle;
            set
            {
                m_ButtonStyle = value;
                RequestUpdate();
            }
        }

        public override void ApplyStyles()
        {
            var buttonStyle = this.buttonStyle;

            if (buttonStyle != null)
            {
                ColorBlock colors = button.colors;

                colors.normalColor = buttonStyle.normalColor;
                colors.highlightedColor = buttonStyle.highlightedColor;
                colors.pressedColor = buttonStyle.pressedColor;
                colors.selectedColor = buttonStyle.selectedColor;
                colors.disabledColor = buttonStyle.disabledColor;

                button.colors = colors;
            }
        }

    }

}

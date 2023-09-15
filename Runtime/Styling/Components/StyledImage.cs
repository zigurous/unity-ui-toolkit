using UnityEngine;
using UnityEngine.UI;

namespace Zigurous.UI
{
    [RequireComponent(typeof(Image))]
    public class StyledImage : StyledComponent
    {
        private Image m_Image;
        public Image image
        {
            get
            {
                if (m_Image == null) {
                    m_Image = GetComponent<Image>();
                }
                return m_Image;
            }
        }

        [SerializeField] private ColorStyle m_ColorStyle;
        public ColorStyle colorStyle
        {
            get => m_ColorStyle;
            set
            {
                m_ColorStyle = value;
                RequestUpdate();
            }
        }

        public override void ApplyStyles()
        {
            if (colorStyle != null) {
                image.color = colorStyle.value;
            }
        }

    }

}

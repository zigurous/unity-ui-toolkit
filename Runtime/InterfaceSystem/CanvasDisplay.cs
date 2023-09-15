using UnityEngine;

namespace Zigurous.UI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasDisplay : ShowableDisplay
    {
        private Canvas m_Canvas;
        public Canvas canvas
        {
            get
            {
                if (m_Canvas == null) {
                    m_Canvas = GetComponent<Canvas>();
                }
                return m_Canvas;
            }
        }

        public override bool IsShown => canvas.enabled;

        public override void Show()
        {
            canvas.enabled = true;
            OnShown();
        }

        public override void Hide()
        {
            canvas.enabled = false;
            OnHidden();
        }

    }

}

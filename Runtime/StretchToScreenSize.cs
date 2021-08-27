using UnityEngine;

namespace Zigurous.UI
{
    /// <summary>
    /// Stretches a UI element to the screen size.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Zigurous/UI/Misc/Stretch to Screen Size")]
    public sealed class StretchToScreenSize : MonoBehaviour
    {
        /// <summary>
        /// The RectTransform component of the object.
        /// </summary>
        public RectTransform rectTransform { get; private set; }

        /// <summary>
        /// Stretches the width of the RectTransform to match the screen width.
        /// </summary>
        [Tooltip("Stretches the width of the RectTransform to match the screen width.")]
        public bool stretchWidth = true;

        /// <summary>
        /// Stretches the height of the RectTransform to match the screen height.
        /// </summary>
        [Tooltip("Stretches the height of the RectTransform to match the screen height.")]
        public bool stretchHeight = true;

        private void Awake()
        {
            this.rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            ScreenSizeListener.Instance.resized += OnScreenResize;
        }

        private void OnDestroy()
        {
            if (ScreenSizeListener.HasInstance) {
                ScreenSizeListener.Instance.resized -= OnScreenResize;
            }
        }

        private void OnEnable()
        {
            Stretch();
        }

        private void OnScreenResize(int width, int height)
        {
            if (this.enabled) {
                Stretch();
            }
        }

        /// <summary>
        /// Stretches the RectTransform to match the screen size.
        /// </summary>
        public void Stretch()
        {
            Stretch(this.stretchWidth, this.stretchHeight);
        }

        /// <summary>
        /// Stretches the RectTransform to match the screen size.
        /// </summary>
        /// <param name="stretchWidth">Whether to stretch the width.</param>
        /// <param name="stretchHeight">Whether to stretch the height.</param>
        public void Stretch(bool stretchWidth, bool stretchHeight)
        {
            if (stretchWidth)
            {
                this.rectTransform.SetAnchorMinX(0.0f);
                this.rectTransform.SetAnchorMaxX(1.0f);
                this.rectTransform.SetLeft(0.0f);
                this.rectTransform.SetRight(0.0f);
            }

            if (stretchHeight)
            {
                this.rectTransform.SetAnchorMinY(0.0f);
                this.rectTransform.SetAnchorMaxY(1.0f);
                this.rectTransform.SetBottom(0.0f);
                this.rectTransform.SetTop(0.0f);
            }
        }

    }

}

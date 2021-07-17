using UnityEngine;

namespace Zigurous.UI
{
    /// <summary>
    /// Stretches a UI element to the screen size.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class StretchToScreenSize : MonoBehaviour
    {
        /// <summary>
        /// The RectTransform component of the object.
        /// </summary>
        private RectTransform _rectTransform;

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
            _rectTransform = GetComponent<RectTransform>();
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
                _rectTransform.SetAnchorMinX(0.0f);
                _rectTransform.SetAnchorMaxX(1.0f);
                _rectTransform.SetLeft(0.0f);
                _rectTransform.SetRight(0.0f);
            }

            if (stretchHeight)
            {
                _rectTransform.SetAnchorMinY(0.0f);
                _rectTransform.SetAnchorMaxY(1.0f);
                _rectTransform.SetBottom(0.0f);
                _rectTransform.SetTop(0.0f);
            }
        }

    }

}

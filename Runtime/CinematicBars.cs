using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Zigurous.UI
{
    /// <summary>
    /// Displays mattes on the top and bottom of the screen to crop the screen
    /// to a specified aspect ratio. This is also referred to as letterboxing
    /// and is useful for cutscenes.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Zigurous/UI/Misc/Cinematic Bars")]
    public sealed class CinematicBars : MonoBehaviour
    {
        /// <summary>
        /// The top letterbox matte (Read only).
        /// </summary>
        public RectTransform matteTop { get; private set; }

        /// <summary>
        /// The bottom letterbox matte (Read only).
        /// </summary>
        public RectTransform matteBottom { get; private set; }

        /// <summary>
        /// The current height of the letterbox mattes (Read only).
        /// </summary>
        public float matteHeight { get; private set; }

        [SerializeField]
        [Tooltip("The color of the mattes.")]
        private Color _color = Color.black;

        /// <summary>
        /// The color of the mattes.
        /// </summary>
        public Color color
        {
            get { return _color; }
            set { _color = value; UpdateStyles(); }
        }

        [SerializeField]
        [Tooltip("The material of the mattes.")]
        private Material _material = null;

        /// <summary>
        /// The material of the mattes.
        /// </summary>
        public Material material
        {
            get { return _material; }
            set { _material = value; UpdateStyles(); }
        }

        [SerializeField]
        [Tooltip("The aspect ratio of the mattes.")]
        private float _aspectRatio = 2.35f;

        /// <summary>
        /// The aspect ratio of the mattes.
        /// </summary>
        public float aspectRatio
        {
            get { return _aspectRatio; }
            set { _aspectRatio = value; UpdateMattes(); }
        }

        /// <summary>
        /// The amount of seconds it takes to animate the mattes.
        /// </summary>
        [Tooltip("The amount of seconds it takes to animate the mattes.")]
        public float animationDuration = 0.5f;

        /// <summary>
        /// The coroutine that animates the mattes.
        /// </summary>
        private Coroutine _animation;

        #if UNITY_EDITOR
        /// <summary>
        /// Whether the current settings have been invalidated.
        /// </summary>
        private bool _invalidated;
        #endif

        private void Awake()
        {
            StretchToScreenSize stretch = this.gameObject.AddComponent<StretchToScreenSize>();
            stretch.hideFlags = HideFlags.HideInInspector;
            stretch.stretchWidth = true;
            stretch.stretchHeight = true;

            this.matteBottom = CreateMatte();
            this.matteTop = CreateMatte();
            this.matteTop.localScale = new Vector3(1f, -1f, 1f);

            SetDesiredHeight(0f);
        }

        private RectTransform CreateMatte()
        {
            GameObject matte = new GameObject("Matte");
            matte.transform.parent = this.transform;

            RectTransform matteRect = matte.AddComponent<RectTransform>();
            matteRect.SetHeight(0f);

            matte.AddComponent<CanvasRenderer>();

            Image graphic = matte.AddComponent<Image>();
            graphic.color = this.color;
            graphic.material = this.material;

            StretchToScreenSize stretch = matte.AddComponent<StretchToScreenSize>();
            stretch.stretchWidth = true;
            stretch.stretchHeight = false;

            return matteRect;
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

        private void OnValidate()
        {
            _invalidated = true;
        }

        private void OnEnable()
        {
            UpdateMattes();
        }

        private void OnDisable()
        {
            UpdateMattes();
        }

        private void OnScreenResize(int width, int height)
        {
            UpdateMattes(animated: false);
        }

        private void Update()
        {
            if (_invalidated)
            {
                UpdateStyles();
                UpdateMattes(animated: false);

                _invalidated = false;
            }
        }

        private void UpdateMattes()
        {
            UpdateMattes(animated: this.animationDuration > 0f);
        }

        private void UpdateMattes(bool animated)
        {
            if (!this.gameObject.activeInHierarchy)
            {
                SetDesiredHeight(0f);
                return;
            }

            float desiredHeight = CalculateMatteHeight();

            if (animated)
            {
                if (_animation != null) {
                    StopCoroutine(_animation);
                }

                _animation = StartCoroutine(Animate(this.matteHeight, desiredHeight));
            }
            else
            {
                SetDesiredHeight(desiredHeight);
            }
        }

        private IEnumerator Animate(float currentHeight, float desiredHeight)
        {
            float elapsed = 0f;

            while (elapsed < this.animationDuration)
            {
                float percent = Mathf.Clamp01(elapsed / this.animationDuration);
                float height = Mathf.SmoothStep(currentHeight, desiredHeight, percent);

                SetDesiredHeight(height);

                elapsed += Time.deltaTime;
                yield return null;
            }

            SetDesiredHeight(desiredHeight);
        }

        private float CalculateMatteHeight()
        {
            if (!this.enabled) {
                return 0f;
            }

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // Screen.width and Screen.height oddly does not always give the
            // correct values so try to use ScreenSizeListener if available
            if (ScreenSizeListener.HasInstance)
            {
                screenWidth = ScreenSizeListener.Instance.width;
                screenHeight = ScreenSizeListener.Instance.height;
            }

            float letterbox = screenWidth / this.aspectRatio;
            float height = (screenHeight - letterbox) / 2f;

            return float.IsNaN(height) ? 0f : height;
        }

        private void SetDesiredHeight(float height)
        {
            this.matteHeight = height;

            if (this.matteTop != null) {
                this.matteTop.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, height);
            }

            if (this.matteBottom != null) {
                this.matteBottom.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0f, height);
            }
        }

        private void UpdateStyles()
        {
            if (this.matteTop != null)
            {
                Graphic graphic = this.matteTop.GetComponent<Graphic>();
                graphic.color = this.color;
                graphic.material = this.material;
            }

            if (this.matteBottom != null)
            {
                Graphic graphic = this.matteBottom.GetComponent<Graphic>();
                graphic.color = this.color;
                graphic.material = this.material;
            }
        }

    }

}

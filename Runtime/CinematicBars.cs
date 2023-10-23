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
    [HelpURL("https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI/CinematicBars")]
    public sealed class CinematicBars : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The color of the mattes.")]
        private Color m_Color = Color.black;

        [SerializeField]
        [Tooltip("The material of the mattes.")]
        private Material m_Material = null;

        [SerializeField]
        [Tooltip("The aspect ratio of the mattes.")]
        private float m_AspectRatio = 2.35f;

        [SerializeField]
        [Tooltip("The amount of seconds it takes to animate the mattes.")]
        private float m_AnimationDuration = 0.5f;

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

        /// <summary>
        /// The color of the mattes.
        /// </summary>
        public Color color
        {
            get => m_Color;
            set { m_Color = value; UpdateStyles(); }
        }

        /// <summary>
        /// The material of the mattes.
        /// </summary>
        public Material material
        {
            get => m_Material;
            set { m_Material = value; UpdateStyles(); }
        }

        /// <summary>
        /// The aspect ratio of the mattes.
        /// </summary>
        public float aspectRatio
        {
            get => m_AspectRatio;
            set { m_AspectRatio = value; UpdateMattes(); }
        }

        /// <summary>
        /// The amount of seconds it takes to animate the mattes.
        /// </summary>
        public float animationDuration
        {
            get => m_AnimationDuration;
            set => m_AnimationDuration = value;
        }

        /// <summary>
        /// The coroutine that animates the mattes.
        /// </summary>
        private Coroutine coroutine;

        #if UNITY_EDITOR
        private bool invalidated;
        #endif

        private void Awake()
        {
            StretchToScreenSize stretch = gameObject.AddComponent<StretchToScreenSize>();
            stretch.hideFlags = HideFlags.HideInInspector;
            stretch.stretchWidth = true;
            stretch.stretchHeight = true;

            matteBottom = CreateMatte();
            matteTop = CreateMatte();
            matteTop.localScale = new Vector3(1f, -1f, 1f);

            SetDesiredHeight(0f);
        }

        private RectTransform CreateMatte()
        {
            GameObject matte = new GameObject("Matte");
            matte.transform.parent = transform;

            RectTransform matteRect = matte.AddComponent<RectTransform>();
            matteRect.SetHeight(0f);

            matte.AddComponent<CanvasRenderer>();

            Image graphic = matte.AddComponent<Image>();
            graphic.color = color;
            graphic.material = material;

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

        #if UNITY_EDITOR
        private void OnValidate()
        {
            invalidated = true;
        }
        #endif

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

        #if UNITY_EDITOR
        private void Update()
        {
            if (invalidated)
            {
                UpdateStyles();
                UpdateMattes(animated: false);

                invalidated = false;
            }
        }
        #endif

        private void UpdateMattes()
        {
            UpdateMattes(animated: animationDuration > 0f);
        }

        private void UpdateMattes(bool animated)
        {
            if (!gameObject.activeInHierarchy)
            {
                SetDesiredHeight(0f);
                return;
            }

            float desiredHeight = CalculateMatteHeight();

            if (animated)
            {
                if (coroutine != null) {
                    StopCoroutine(coroutine);
                }

                coroutine = StartCoroutine(Animate(matteHeight, desiredHeight));
            }
            else
            {
                SetDesiredHeight(desiredHeight);
            }
        }

        private IEnumerator Animate(float currentHeight, float desiredHeight)
        {
            float elapsed = 0f;

            while (elapsed < animationDuration)
            {
                float percent = Mathf.Clamp01(elapsed / animationDuration);
                float height = Mathf.SmoothStep(currentHeight, desiredHeight, percent);

                SetDesiredHeight(height);

                elapsed += Time.deltaTime;
                yield return null;
            }

            SetDesiredHeight(desiredHeight);

            coroutine = null;
        }

        private float CalculateMatteHeight()
        {
            if (!enabled) {
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

            float letterbox = screenWidth / aspectRatio;
            float height = (screenHeight - letterbox) / 2f;

            return float.IsNaN(height) ? 0f : height;
        }

        private void SetDesiredHeight(float height)
        {
            matteHeight = height;

            if (matteTop != null) {
                matteTop.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, height);
            }

            if (matteBottom != null) {
                matteBottom.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0f, height);
            }
        }

        private void UpdateStyles()
        {
            if (matteTop != null)
            {
                Graphic graphic = matteTop.GetComponent<Graphic>();
                graphic.color = color;
                graphic.material = material;
            }

            if (matteBottom != null)
            {
                Graphic graphic = matteBottom.GetComponent<Graphic>();
                graphic.color = color;
                graphic.material = material;
            }
        }

    }

}

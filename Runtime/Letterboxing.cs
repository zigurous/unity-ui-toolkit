using System.Collections;
using UnityEngine;

namespace Zigurous.UI
{
    /// <summary>
    /// Crops the screen to a specified aspect ratio by changing the camera's
    /// viewport. This is known as letterboxing and is useful for cutscenes.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Zigurous/UI/Misc/Letterboxing")]
    [HelpURL("https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI/Letterboxing")]
    public sealed class Letterboxing : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The aspect ratio of the mattes.")]
        private float m_AspectRatio = 2.35f;

        [SerializeField]
        [Tooltip("The amount of seconds it takes to animate the mattes.")]
        private float m_AnimationDuration = 0.5f;

        /// <summary>
        /// The camera being letterboxed (Read only).
        /// </summary>
        public new Camera camera { get; private set; }

        /// <summary>
        /// The current width of the viewport (Read only).
        /// </summary>
        public float viewportWidth => camera.rect.width;

        /// <summary>
        /// The current height of the viewport (Read only).
        /// </summary>
        public float viewportHeight => camera.rect.height;

        /// <summary>
        /// The aspect ratio of the mattes.
        /// </summary>
        public float aspectRatio
        {
            get => m_AspectRatio;
            set { m_AspectRatio = value; UpdateViewport(); }
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
            camera = GetComponent<Camera>();
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
            UpdateViewport();
        }

        private void OnDisable()
        {
            UpdateViewport();
        }

        private void OnScreenResize(int width, int height)
        {
            UpdateViewport(animated: false);
        }

        #if UNITY_EDITOR
        private void Update()
        {
            if (invalidated)
            {
                UpdateViewport(animated: false);

                invalidated = false;
            }
        }
        #endif

        private void UpdateViewport()
        {
            UpdateViewport(animated: animationDuration > 0f);
        }

        private void UpdateViewport(bool animated)
        {
            if (!gameObject.activeInHierarchy)
            {
                SetViewportHeight(1f);
                return;
            }

            float desiredHeight = CalculateViewportHeight();

            if (animated)
            {
                if (coroutine != null) {
                    StopCoroutine(coroutine);
                }

                coroutine = StartCoroutine(Animate(viewportHeight, desiredHeight));
            }
            else
            {
                SetViewportHeight(desiredHeight);
            }
        }

        private IEnumerator Animate(float currentHeight, float desiredHeight)
        {
            float elapsed = 0f;

            while (elapsed < animationDuration)
            {
                float percent = Mathf.Clamp01(elapsed / animationDuration);
                float height = Mathf.SmoothStep(currentHeight, desiredHeight, percent);

                SetViewportHeight(height);

                elapsed += Time.deltaTime;
                yield return null;
            }

            SetViewportHeight(desiredHeight);

            coroutine = null;
        }

        private float CalculateViewportHeight()
        {
            if (!enabled) {
                return 1f;
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
            float viewport = 1f - ((height / screenHeight) * 2f);

            return float.IsNaN(viewport) ? 1f : viewport;
        }

        private void SetViewportHeight(float height)
        {
            Rect rect = camera.rect;
            rect.height = height;
            rect.y = (1f - height) / 2f;

            camera.rect = rect;
        }

    }

}

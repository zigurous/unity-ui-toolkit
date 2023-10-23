using UnityEngine;

namespace Zigurous.UI
{
    /// <summary>
    /// Listens for changes in the screen size.
    /// </summary>
    [AddComponentMenu("")]
    [HelpURL("https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI/ScreenSizeListener")]
    public sealed class ScreenSizeListener : MonoBehaviour
    {
        private static volatile ScreenSizeListener instance;
        private static object lockObject = new object();
        private static bool isUnloading = false;

        /// <summary>
        /// The current instance of the class. The instance will be created if
        /// it does not already exist.
        /// </summary>
        /// <returns>The instance of the class.</returns>
        public static ScreenSizeListener Instance
        {
            get
            {
                if (isUnloading) {
                    return null;
                }

                if (instance == null)
                {
                    lock (lockObject)
                    {
                        instance = FindObjectOfType<ScreenSizeListener>();

                        if (instance == null)
                        {
                            GameObject singleton = new GameObject();
                            singleton.name = typeof(ScreenSizeListener).Name;
                            singleton.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                            singleton.AddComponent<ScreenSizeListener>();
                            DontDestroyOnLoad(singleton);
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Checks if the singleton has been initialized and an instance is
        /// available to use.
        /// </summary>
        /// <returns>True if an instance is available, false otherwise.</returns>
        public static bool HasInstance => instance != null;

        /// <summary>
        /// A function delegate invoked when the screen size changes.
        /// </summary>
        /// <param name="width">The new width of the screen.</param>
        /// <param name="height">The new height of the screen.</param>
        public delegate void OnResize(int width, int height);

        /// <summary>
        /// A callback invoked when the screen size changes.
        /// </summary>
        public OnResize resized;

        /// <summary>
        /// The current size of the screen (Read only).
        /// </summary>
        public Vector2Int size => new Vector2Int(width, height);

        /// <summary>
        /// The current width of the screen (Read only).
        /// </summary>
        public int width { get; private set; }

        /// <summary>
        /// The current height of the screen (Read only).
        /// </summary>
        public int height { get; private set; }

        private ScreenSizeListener() {}

        private void Awake()
        {
            isUnloading = false;

            if (instance == null)
            {
                instance = this;

                width = Screen.width;
                height = Screen.height;
            }
            else {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            resized = null;

            isUnloading = true;

            if (instance == this) {
                instance = null;
            }
        }

        private void Update()
        {
            if (Screen.width != width || Screen.height != height)
            {
                width = Screen.width;
                height = Screen.height;

                if (resized != null) {
                    resized.Invoke(width, height);
                }
            }
        }

    }

}

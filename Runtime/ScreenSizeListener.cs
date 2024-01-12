﻿using UnityEngine;

namespace Zigurous.UI
{
    /// <summary>
    /// Listens for changes in the screen size.
    /// </summary>
    [AddComponentMenu("")]
    [HelpURL("https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI/ScreenSizeListener")]
    public sealed class ScreenSizeListener : MonoBehaviour
    {
        internal static volatile ScreenSizeListener instance;
        private static readonly object threadLock = new();
        private static bool isUnloading = false;

        private static ScreenSizeListener GetInstance()
        {
            if (instance == null)
            {
                lock (threadLock)
                {
                    instance = FindObjectOfType<ScreenSizeListener>();

                    if (instance == null && !isUnloading)
                    {
                        GameObject singleton = new()
                        {
                            name = typeof(ScreenSizeListener).Name,
                            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector
                        };

                        return singleton.AddComponent<ScreenSizeListener>();
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// The current instance of the class.
        /// The instance will be created if it does not already exist.
        /// </summary>
        /// <returns>The instance of the class.</returns>
        public static ScreenSizeListener Instance => GetInstance();

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
        public delegate void ResizeDelegate(int width, int height);

        /// <summary>
        /// A callback invoked when the screen size changes.
        /// </summary>
        public ResizeDelegate resized;

        /// <summary>
        /// The current size of the screen (Read only).
        /// </summary>
        public Vector2Int size => new(width, height);

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
            if (instance == null)
            {
                instance = this;
                width = Screen.width;
                height = Screen.height;

                if (Application.isPlaying) {
                    DontDestroyOnLoad(this);
                }
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
                resized = null;
            }
        }

        private void OnApplicationQuit()
        {
            isUnloading = true;
        }

        private void Update()
        {
            if (Screen.width != width || Screen.height != height)
            {
                width = Screen.width;
                height = Screen.height;
                resized?.Invoke(width, height);
            }
        }

    }

}

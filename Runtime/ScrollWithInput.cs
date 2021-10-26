using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.UI;

namespace Zigurous.UI
{
    /// <summary>
    /// Handles scrolling a ScrollRect component with user input. This is
    /// especially useful for controller support.
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    [AddComponentMenu("Zigurous/UI/Navigation/Scroll With Input")]
    public class ScrollWithInput : MonoBehaviour
    {
        /// <summary>
        /// The ScrollRect component being scrolled (Read only).
        /// </summary>
        public ScrollRect scrollRect { get; private set; }

        /// <summary>
        /// The direction to scroll the ScrollRect.
        /// </summary>
        [Tooltip("The direction to scroll the ScrollRect.")]
        public ScrollDirection scrollDirection = ScrollDirection.Vertical;

        #if ENABLE_INPUT_SYSTEM

        /// <summary>
        /// The input action that handles scrolling.
        /// </summary>
        [Tooltip("The input action that handles scrolling.")]
        public InputAction scrollInput = new InputAction("ScrollInput", InputActionType.Value, null, null, null, "Vector2");

        #elif ENABLE_LEGACY_INPUT_MANAGER

        /// <summary>
        /// The input axis that handles scrolling in the y-axis.
        /// </summary>
        [Tooltip("The input axis that handles scrolling in the y-axis.")]
        public string scrollInputAxisY = "";

        /// <summary>
        /// The input axis that handles scrolling in the x-axis.
        /// </summary>
        [Tooltip("The input axis that handles scrolling in the x-axis.")]
        public string scrollInputAxisX = "";

        #endif

        /// <summary>
        /// The sensitivity multiplier applied to the input.
        /// </summary>
        [Tooltip("The sensitivity multiplier applied to the input.")]
        public float sensitivity = 1f;

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        #if ENABLE_INPUT_SYSTEM
        private void Reset()
        {
            scrollInput = new InputAction("ScrollInput", InputActionType.Value, null, null, null, "Vector2");
            scrollInput.AddBinding("<Gamepad>/rightStick");
            scrollInput.AddBinding("<Gamepad>/leftStick");
            scrollInput.AddBinding("<Gamepad>/dpad");
        }

        private void OnEnable()
        {
            scrollInput.Enable();
        }

        private void OnDisable()
        {
            scrollInput.Disable();
        }
        #endif

        private void Update()
        {
            EventSystem eventSystem = EventSystem.current;

            if (eventSystem == null || eventSystem.currentSelectedGameObject == null) {
                return;
            }

            if (eventSystem.currentSelectedGameObject == scrollRect.gameObject ||
                eventSystem.currentSelectedGameObject.transform.parent == scrollRect.content)
            {
                Vector2 input = Vector2.zero;

                #if ENABLE_INPUT_SYSTEM
                input = scrollInput.ReadValue<Vector2>();
                #elif ENABLE_LEGACY_INPUT_MANAGER
                if (scrollInputAxisY != "") {
                    input.y = Input.GetAxis(scrollInputAxisY);
                }

                if (scrollInputAxisX != "") {
                    input.x = Input.GetAxis(scrollInputAxisX);
                }
                #endif

                switch (scrollDirection)
                {
                    case ScrollDirection.Vertical:
                        input.x = 0f;
                        break;

                    case ScrollDirection.Horizontal:
                        input.y = 0f;
                        break;
                }

                scrollRect.normalizedPosition += input * sensitivity * Time.unscaledDeltaTime;
            }
        }

    }

}

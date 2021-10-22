using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
        /// The input action that handles scrolling.
        /// </summary>
        [Tooltip("The input action that handles scrolling.")]
        public InputAction scrollInput = new InputAction("ScrollInput", InputActionType.Value, null, null, null, "Vector2");

        /// <summary>
        /// The sensitivity multiplier applied to the input.
        /// </summary>
        [Tooltip("The sensitivity multiplier applied to the input.")]
        public float sensitivity = 1f;

        private void Reset()
        {
            scrollInput = new InputAction("ScrollInput", InputActionType.Value, null, null, null, "Vector2");
            scrollInput.AddBinding("<Gamepad>/rightStick");
            scrollInput.AddBinding("<Gamepad>/leftStick");
            scrollInput.AddBinding("<Gamepad>/dpad");
        }

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        private void OnEnable()
        {
            scrollInput.Enable();
        }

        private void OnDisable()
        {
            scrollInput.Disable();
        }

        private void Update()
        {
            EventSystem eventSystem = EventSystem.current;

            if (eventSystem == null || eventSystem.currentSelectedGameObject == null) {
                return;
            }

            if (eventSystem.currentSelectedGameObject == scrollRect.gameObject ||
                eventSystem.currentSelectedGameObject.transform.parent == scrollRect.content)
            {
                Vector2 input = scrollInput.ReadValue<Vector2>();
                scrollRect.normalizedPosition += input * sensitivity * Time.unscaledDeltaTime;
            }
        }

    }

}

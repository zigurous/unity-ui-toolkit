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
        /// The ScrollRect component being scrolled.
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
        public float sensitivity = 1.0f;

        private void Reset()
        {
            this.scrollInput = new InputAction("ScrollInput", InputActionType.Value, null, null, null, "Vector2");
            this.scrollInput.AddBinding("<Gamepad>/rightStick");
            this.scrollInput.AddBinding("<Gamepad>/leftStick");
            this.scrollInput.AddBinding("<Gamepad>/dpad");
        }

        private void Awake()
        {
            this.scrollRect = GetComponent<ScrollRect>();
        }

        private void OnEnable()
        {
            this.scrollInput.Enable();
        }

        private void OnDisable()
        {
            this.scrollInput.Disable();
        }

        private void Update()
        {
            EventSystem eventSystem = EventSystem.current;

            if (eventSystem == null || eventSystem.currentSelectedGameObject == null) {
                return;
            }

            if (eventSystem.currentSelectedGameObject == this.scrollRect.gameObject ||
                eventSystem.currentSelectedGameObject.transform.parent == this.scrollRect.content)
            {
                Vector2 input = this.scrollInput.ReadValue<Vector2>();
                this.scrollRect.normalizedPosition += input * this.sensitivity * Time.unscaledDeltaTime;
            }
        }

    }

}

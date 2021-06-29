using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Zigurous.UI
{
    [RequireComponent(typeof(ScrollRect))]
    [AddComponentMenu("Zigurous/UI/Navigation/Scroll With Input")]
    public class ScrollWithInput : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        public InputAction scrollInput;
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
            _scrollRect = GetComponent<ScrollRect>();
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

            if (eventSystem.currentSelectedGameObject == _scrollRect.gameObject ||
                eventSystem.currentSelectedGameObject.transform.parent == _scrollRect.content)
            {
                Vector2 input = this.scrollInput.ReadValue<Vector2>();
                _scrollRect.normalizedPosition += input * this.sensitivity * Time.unscaledDeltaTime;
            }
        }

    }

}

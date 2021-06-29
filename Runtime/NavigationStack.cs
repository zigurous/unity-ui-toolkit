using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Zigurous.UI
{
    [RequireComponent(typeof(EventSystem))]
    [AddComponentMenu("Zigurous/UI/Navigation/Navigation Stack")]
    public class NavigationStack : MonoBehaviour
    {
        public EventSystem eventSystem { get; private set; }
        public Stack<GameObject> navigationStack { get; private set; }
        public GameObject CurrentSelectedGameObject => this.navigationStack.Peek();
        public InputAction backInput;
        public bool allowEmptyStack = false;
        public bool allowNullSelections = false;

        private void Reset()
        {
            this.backInput = new InputAction("MenuBackNavigation", InputActionType.Button);
            this.backInput.AddBinding("<Keyboard>/escape");
            this.backInput.AddBinding("<Keyboard>/backspace");
            this.backInput.AddBinding("<Gamepad>/select");
            this.backInput.AddBinding("<Gamepad>/buttonEast");
        }

        private void Awake()
        {
            this.eventSystem = GetComponent<EventSystem>();
            this.navigationStack = new Stack<GameObject>(8);
        }

        private void OnEnable()
        {
            this.backInput.Enable();
            this.backInput.performed += OnBack;
        }

        private void OnDisable()
        {
            this.backInput.Disable();
            this.backInput.performed -= OnBack;
        }

        private void Update()
        {
            if (this.eventSystem.currentSelectedGameObject != CurrentSelectedGameObject)
            {
                if (this.eventSystem.currentSelectedGameObject != null || this.allowNullSelections) {
                    this.navigationStack.Push(this.eventSystem.currentSelectedGameObject);
                }
            }
        }

        public void Back()
        {
            if (this.navigationStack.Count == 0) {
                return;
            }

            if (this.navigationStack.Count > 1 || this.allowEmptyStack) {
                this.navigationStack.Pop();
            }

            this.eventSystem.SetSelectedGameObject(null);

            if (this.navigationStack.Count > 0) {
                this.eventSystem.SetSelectedGameObject(this.navigationStack.Peek());
            }
        }

        private void OnBack(InputAction.CallbackContext context)
        {
            if (context.performed) {
                Back();
            }
        }

    }

}

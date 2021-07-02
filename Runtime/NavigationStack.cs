using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Zigurous.UI
{
    /// <summary>
    /// Keeps track of a stack of selected game objects as to allow for easy
    /// back navigation.
    /// </summary>
    [RequireComponent(typeof(EventSystem))]
    [AddComponentMenu("Zigurous/UI/Navigation/Navigation Stack")]
    public class NavigationStack : MonoBehaviour
    {
        /// <summary>
        /// The event system being tracked by the navigation stack.
        /// </summary>
        public EventSystem eventSystem { get; private set; }

        /// <summary>
        /// The stack of game objects that have been selected.
        /// </summary>
        public Stack<GameObject> navigationStack { get; private set; }

        /// <summary>
        /// The current selected game object at the top of the stack.
        /// </summary>
        public GameObject CurrentSelectedGameObject => this.navigationStack.Peek();

        /// <summary>
        /// The input action to handle navigating backwards in the stack by
        /// popping items off.
        /// </summary>
        [Tooltip("The input action to handle navigating backwards in the stack by popping items off.")]
        public InputAction backNavigationInput = new InputAction("MenuBackNavigation", InputActionType.Button);

        /// <summary>
        /// Allows for all items to be popped off the stack. Often times you
        /// want to maintain at least the root game object.
        /// </summary>
        [Tooltip("Allows for all items to be popped off the stack. Often times you want to maintain at least the root game object.")]
        public bool allowEmptyStack = false;

        /// <summary>
        /// Allows for null game objects to be pushed onto the stack.
        /// </summary>
        [Tooltip("Allows for null game objects to be pushed onto the stack.")]
        public bool allowNullSelections = false;

        private void Reset()
        {
            this.backNavigationInput = new InputAction("MenuBackNavigation", InputActionType.Button);
            this.backNavigationInput.AddBinding("<Keyboard>/escape");
            this.backNavigationInput.AddBinding("<Keyboard>/backspace");
            this.backNavigationInput.AddBinding("<Gamepad>/select");
            this.backNavigationInput.AddBinding("<Gamepad>/buttonEast");
        }

        private void Awake()
        {
            this.eventSystem = GetComponent<EventSystem>();
            this.navigationStack = new Stack<GameObject>(8);
            this.backNavigationInput.performed += OnBack;
        }

        private void OnEnable()
        {
            this.backNavigationInput.Enable();
        }

        private void OnDisable()
        {
            this.backNavigationInput.Disable();
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

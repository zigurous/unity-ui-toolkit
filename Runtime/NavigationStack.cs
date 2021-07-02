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
        /// The game objects added to the stack.
        /// </summary>
        public Stack<GameObject> items { get; private set; }

        /// <summary>
        /// The current selected game object at the top of the stack.
        /// </summary>
        public GameObject Top => this.items.Count > 0 ? this.items.Peek() : null;

        /// <summary>
        /// The input action to handle navigating backwards in the stack by
        /// popping items off.
        /// </summary>
        [Tooltip("The input action to handle navigating backwards in the stack by popping items off.")]
        public InputAction backNavigationInput = new InputAction("MenuBackNavigation", InputActionType.Button);

        /// <summary>
        /// The root game object added to the bottom of the stack.
        /// </summary>
        [Tooltip("The root game object added to the bottom of the stack.")]
        public GameObject rootGameObject;

        /// <summary>
        /// Automatically sets the active state of game objects as they are
        /// pushed on and off the stack.
        /// </summary>
        [Tooltip("Automatically sets the active state of game objects as they are pushed on and off the stack.")]
        public bool setActiveState = true;

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
            this.items = new Stack<GameObject>(8);
            this.eventSystem = GetComponent<EventSystem>();
            this.backNavigationInput.performed += OnBack;
        }

        private void Start()
        {
            Push(this.rootGameObject);
        }

        private void OnEnable()
        {
            this.backNavigationInput.Enable();
        }

        private void OnDisable()
        {
            this.backNavigationInput.Disable();
        }

        public void Push(GameObject selected)
        {
            if (selected != null || this.allowNullSelections)
            {
                if (this.setActiveState && selected != null) {
                    selected.SetActive(true);
                }

                this.eventSystem.SetSelectedGameObject(selected);
                this.items.Push(selected);
            }
        }

        public void Back()
        {
            if (this.items.Count == 0) {
                return;
            }

            // Pop off the top of the stack
            if (this.items.Count > 1 || this.allowEmptyStack)
            {
                GameObject top = this.items.Pop();

                if (this.setActiveState && top != null) {
                    top.SetActive(false);
                }

                this.eventSystem.SetSelectedGameObject(null);
            }

            // Set the previous item to be active
            if (this.items.Count > 0)
            {
                GameObject previous = this.items.Peek();

                if (this.setActiveState && previous != null) {
                    previous.SetActive(true);
                }

                this.eventSystem.SetSelectedGameObject(previous);
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

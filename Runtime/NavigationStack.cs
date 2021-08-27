using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Zigurous.UI
{
    /// <summary>
    /// Manages a stack of game objects for menu navigation purposes. This is
    /// especially useful to handle backwards navigation by simply popping off
    /// the last item in the stack.
    /// </summary>
    [RequireComponent(typeof(EventSystem))]
    [AddComponentMenu("Zigurous/UI/Navigation/Navigation Stack")]
    public class NavigationStack : MonoBehaviour
    {
        /// <summary>
        /// The event system being tracked by the navigation stack (Read only).
        /// </summary>
        public EventSystem eventSystem { get; private set; }

        /// <summary>
        /// The game objects added to the stack (Read only).
        /// </summary>
        public Stack<GameObject> items { get; private set; }

        /// <summary>
        /// The current selected game object at the top of the stack (Read only).
        /// </summary>
        public GameObject top => this.items.Count > 0 ? this.items.Peek() : null;

        /// <summary>
        /// The input action that handles backwards navigation by popping items
        /// off the stack.
        /// </summary>
        [Tooltip("The input action that handles backwards navigation by popping items off the stack.")]
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

        /// <summary>
        /// Pushes a game object onto the stack, effectively navigating
        /// forwards.
        /// </summary>
        /// <param name="selected">The game object to push onto the stack.</param>
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

        /// <summary>
        /// Pops the last game object off the stack, effectively navigating
        /// backwards.
        /// </summary>
        /// <returns>The game object that was popped off the stack.</returns>
        public GameObject Pop()
        {
            if (this.items.Count == 0) {
                return null;
            }

            GameObject top = null;

            // Pop off the top of the stack
            if (this.items.Count > 1 || this.allowEmptyStack)
            {
                top = this.items.Pop();

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

            return top;
        }

        private void OnBack(InputAction.CallbackContext context)
        {
            if (context.performed) {
                Pop();
            }
        }

    }

}

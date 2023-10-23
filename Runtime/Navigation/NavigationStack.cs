using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Zigurous.UI
{
    /// <summary>
    /// Manages a stack of game objects for menu navigation purposes. This is
    /// especially useful to handle backwards navigation by simply popping off
    /// the last item in the stack.
    /// </summary>
    [RequireComponent(typeof(EventSystem))]
    [AddComponentMenu("Zigurous/UI/Navigation/Navigation Stack")]
    [HelpURL("https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI/NavigationStack")]
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
        public GameObject Top => items.Count > 0 ? items.Peek() : null;

        #if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// The input action that handles backwards navigation by popping items
        /// off the stack.
        /// </summary>
        [Tooltip("The input action that handles backwards navigation by popping items off the stack.")]
        public InputAction backNavigationInput = new InputAction("MenuBackNavigation", InputActionType.Button);
        #endif

        #if ENABLE_LEGACY_INPUT_MANAGER
        /// <summary>
        /// The input button that handles backwards navigation by popping items
        /// off the stack.
        /// </summary>
        [Tooltip("The input button that handles backwards navigation by popping items off the stack.")]
        public string legacyBackNavigationInput = "Cancel";
        #endif

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
        /// Allows for all items to be popped off the stack, including the root.
        /// </summary>
        [Tooltip("Allows for all items to be popped off the stack, including the root.")]
        public bool allowEmptyStack = false;

        /// <summary>
        /// Allows for null game objects to be pushed onto the stack.
        /// </summary>
        [Tooltip("Allows for null game objects to be pushed onto the stack.")]
        public bool allowNullSelections = false;

        #if ENABLE_INPUT_SYSTEM
        private void Reset()
        {
            backNavigationInput = new InputAction("MenuBackNavigation", InputActionType.Button);
            backNavigationInput.AddBinding("<Keyboard>/escape");
            backNavigationInput.AddBinding("<Keyboard>/backspace");
            backNavigationInput.AddBinding("<Gamepad>/select");
            backNavigationInput.AddBinding("<Gamepad>/buttonEast");
        }
        #endif

        private void Awake()
        {
            items = new Stack<GameObject>(8);
            eventSystem = GetComponent<EventSystem>();

            #if ENABLE_INPUT_SYSTEM
            backNavigationInput.performed += OnBack;
            #endif
        }

        private void Start()
        {
            Push(rootGameObject);
        }

        #if ENABLE_INPUT_SYSTEM
        private void OnEnable()
        {
            backNavigationInput.Enable();
        }

        private void OnDisable()
        {
            backNavigationInput.Disable();
        }
        #endif

        #if ENABLE_LEGACY_INPUT_MANAGER
        private void Update()
        {
            try
            {
                if (!string.IsNullOrEmpty(legacyBackNavigationInput) && Input.GetButtonDown(legacyBackNavigationInput)) {
                    Pop();
                }
            }
            catch
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"[NavigationStack] Input button '{legacyBackNavigationInput}' is not setup.\nDefine the input in the Input Manager settings accessed from the menu: Edit > Project Settings");
                #endif
            }
        }
        #endif

        /// <summary>
        /// Pushes a game object onto the stack, effectively navigating
        /// forwards.
        /// </summary>
        /// <param name="selected">The game object to push onto the stack.</param>
        public void Push(GameObject selected)
        {
            if (selected != null || allowNullSelections)
            {
                if (setActiveState && selected != null) {
                    selected.SetActive(true);
                }

                eventSystem.SetSelectedGameObject(selected);
                items.Push(selected);
            }
        }

        /// <summary>
        /// Pops the last game object off the stack, effectively navigating
        /// backwards.
        /// </summary>
        /// <returns>The game object that was popped off the stack.</returns>
        public GameObject Pop()
        {
            if (items.Count == 0) {
                return null;
            }

            GameObject top = null;

            // Pop off the top of the stack
            if (items.Count > 1 || allowEmptyStack)
            {
                top = items.Pop();

                if (setActiveState && top != null) {
                    top.SetActive(false);
                }

                eventSystem.SetSelectedGameObject(null);
            }

            // Set the previous item to be active
            if (items.Count > 0)
            {
                GameObject previous = items.Peek();

                if (setActiveState && previous != null) {
                    previous.SetActive(true);
                }

                eventSystem.SetSelectedGameObject(previous);
            }

            return top;
        }

        #if ENABLE_INPUT_SYSTEM
        private void OnBack(InputAction.CallbackContext context)
        {
            if (context.performed) {
                Pop();
            }
        }
        #endif

    }

}

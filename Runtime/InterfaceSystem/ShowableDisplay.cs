using UnityEngine;
using UnityEngine.InputSystem;

namespace Zigurous.UI
{
    public abstract class ShowableDisplay : AbstractDisplay, IShowableDisplay
    {
        [Header("Settings")]

        [Tooltip("The cursor state to set when the display is shown (optional).")]
        public CursorState cursorState = null;

        [Tooltip("Whether the display should be shown when the behavior starts.")]
        public bool showOnStart = true;

        [Header("Input")]
        public InputActionReference showInput;
        public InputActionReference hideInput;
        public InputActionReference toggleInput;

        public abstract bool IsShown { get; }
        public bool IsHidden => !IsShown;

        public abstract void Show();
        public abstract void Hide();

        protected virtual void Awake()
        {
            RegisterInput(showInput, OnShowInput);
            RegisterInput(hideInput, OnHideInput);
            RegisterInput(toggleInput, OnToggleInput);
        }

        protected virtual void OnDestroy()
        {
            UnregisterInput(showInput, OnShowInput);
            UnregisterInput(hideInput, OnHideInput);
            UnregisterInput(toggleInput, OnToggleInput);
        }

        protected virtual void Start()
        {
            if (showOnStart) {
                Show();
            } else {
                Hide();
            }
        }

        public void InvokeShow(float time)
        {
            Invoke(nameof(Show), time);
        }

        public void InvokeHide(float time)
        {
            Invoke(nameof(Hide), time);
        }

        protected virtual void OnShown()
        {
            if (this.cursorState != null) {
                CursorController.PushState(this.cursorState, GetInstanceID());
            }
        }

        protected virtual void OnHidden()
        {
            if (this.cursorState != null) {
                CursorController.RemoveState(GetInstanceID());
            }
        }

        private void OnShowInput(InputAction.CallbackContext context)
        {
            if (isActiveAndEnabled) {
                this.Show();
            }
        }

        private void OnHideInput(InputAction.CallbackContext context)
        {
            if (isActiveAndEnabled) {
                this.Hide();
            }
        }

        private void OnToggleInput(InputAction.CallbackContext context)
        {
            if (isActiveAndEnabled) {
                this.ToggleShowHide();
            }
        }

    }

}

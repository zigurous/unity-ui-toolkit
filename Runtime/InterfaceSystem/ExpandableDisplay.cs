using UnityEngine;
using UnityEngine.InputSystem;

namespace Zigurous.UI
{
    public abstract class ExpandableDisplay : AbstractDisplay, IExpandableDisplay
    {
        [Header("Settings")]

        [Tooltip("The cursor state to set when the display is expanded (optional).")]
        public CursorState cursorState = null;

        [Tooltip("Whether the display should be expanded when the behavior starts.")]
        public bool expandOnStart = true;

        [Header("Input")]
        public InputActionReference expandInput;
        public InputActionReference collapseInput;
        public InputActionReference toggleInput;

        public abstract bool IsExpanded { get; }
        public bool IsCollapsed => !IsExpanded;

        public abstract void Expand();
        public abstract void Collapse();

        protected virtual void Awake()
        {
            RegisterInput(expandInput, OnExpandInput);
            RegisterInput(collapseInput, OnCollapseInput);
            RegisterInput(toggleInput, OnToggleInput);
        }

        protected virtual void OnDestroy()
        {
            UnregisterInput(expandInput, OnExpandInput);
            UnregisterInput(collapseInput, OnCollapseInput);
            UnregisterInput(toggleInput, OnToggleInput);
        }

        protected virtual void Start()
        {
            if (expandOnStart) {
                Expand();
            } else {
                Collapse();
            }
        }

        public void InvokeExpand(float time)
        {
            Invoke(nameof(Expand), time);
        }

        public void InvokeCollapse(float time)
        {
            Invoke(nameof(Collapse), time);
        }

        protected virtual void OnExpanded()
        {
            if (this.cursorState != null) {
                CursorController.PushState(this.cursorState, GetInstanceID());
            }
        }

        protected virtual void OnCollapsed()
        {
            if (this.cursorState != null) {
                CursorController.RemoveState(GetInstanceID());
            }
        }

        private void OnExpandInput(InputAction.CallbackContext context)
        {
            if (isActiveAndEnabled) {
                this.Expand();
            }
        }

        private void OnCollapseInput(InputAction.CallbackContext context)
        {
            if (isActiveAndEnabled) {
                this.Collapse();
            }
        }

        private void OnToggleInput(InputAction.CallbackContext context)
        {
            if (isActiveAndEnabled) {
                this.ToggleExpandCollapse();
            }
        }

    }

}

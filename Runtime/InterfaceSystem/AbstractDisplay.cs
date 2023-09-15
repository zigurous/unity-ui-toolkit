using UnityEngine;
using UnityEngine.InputSystem;

namespace Zigurous.UI
{
    public abstract class AbstractDisplay : MonoBehaviour, IInterfaceDisplay
    {
        protected void RegisterInput(InputActionReference input, System.Action<InputAction.CallbackContext> callback, bool enableInput = true)
        {
            if (input != null && input.action != null)
            {
                input.action.performed += callback;

                if (enableInput) {
                    input.action.Enable();
                }
            }
        }

        protected void UnregisterInput(InputActionReference input, System.Action<InputAction.CallbackContext> callback, bool disableInput = false)
        {
            if (input != null && input.action != null)
            {
                input.action.performed -= callback;

                if (disableInput) {
                    input.action.Disable();
                }
            }
        }

    }

}

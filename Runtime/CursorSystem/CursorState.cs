using UnityEngine;

namespace Zigurous.UI
{
    [CreateAssetMenu(menuName = "Zigurous/UI/Cursor State")]
    public class CursorState : ScriptableObject
    {
        public bool pointerVisible;
        public CursorLockMode lockState;
        public CustomCursor defaultCursor;
    }

    [System.Serializable]
    public struct InstancedCursorState
    {
        public CursorState state;
        public int instanceID;

        public InstancedCursorState(CursorState state, int instanceID)
        {
            this.state = state;
            this.instanceID = instanceID;
        }

    }

}

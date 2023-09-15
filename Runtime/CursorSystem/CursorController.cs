using System.Collections.Generic;
using UnityEngine;

namespace Zigurous.UI
{
    public static class CursorController
    {
        private static List<InstancedCursorState> m_Instances;

        private static CursorState m_DefaultState;
        private static CursorState DefaultState
        {
            get
            {
                if (m_DefaultState == null)
                {
                    m_DefaultState = ScriptableObject.CreateInstance<CursorState>();
                    m_DefaultState.pointerVisible = true;
                    m_DefaultState.lockState = CursorLockMode.None;
                }
                return m_DefaultState;
            }
        }

        public static CursorState CurrentState
        {
            get
            {
                if (m_Instances != null && m_Instances.Count > 0) {
                    return m_Instances[m_Instances.Count - 1].state;
                } else {
                    return DefaultState;
                }
            }
        }

        public static System.Action<CursorState> OnCursorChanged { get; set; }

        public static void Initialize(CursorState state)
        {
            if (m_Instances == null)
            {
                m_Instances = new List<InstancedCursorState>();
                Application.focusChanged += OnFocusChanged;
            }
            else
            {
                m_Instances.Clear();
            }

            PushState(state, 0);
        }

        public static void PushState(CursorState state, int instanceID)
        {
            int index = GetIndex(instanceID);

            if (index != -1)
            {
                InstancedCursorState instance = m_Instances[index];
                instance.state = state;
                m_Instances[index] = instance;
            }
            else
            {
                m_Instances.Add(new InstancedCursorState(state, instanceID));
            }

            UpdateCursorState();
        }

        public static void RemoveState(int instanceID)
        {
            int index = GetIndex(instanceID);

            if (index != -1)
            {
                m_Instances.RemoveAt(index);
                UpdateCursorState();
            }
        }

        public static void UpdateCursorState()
        {
            CursorState state = CurrentState;

            if (state != null)
            {
                Cursor.visible = state.pointerVisible;
                Cursor.lockState = state.lockState;

                if (state.defaultCursor != null) {
                    state.defaultCursor.Apply();
                }

                OnCursorChanged?.Invoke(state);
            }
        }

        private static int GetIndex(int instanceID)
        {
            int index = -1;

            for (int i = 0; i < m_Instances.Count; i++)
            {
                if (m_Instances[i].instanceID == instanceID)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private static void OnFocusChanged(bool hasFocus)
        {
            if (hasFocus) {
                CursorController.UpdateCursorState();
            }
        }

    }

}

using UnityEngine;

namespace Zigurous.UI
{
    public abstract class StyledComponent : MonoBehaviour
    {
        #if UNITY_EDITOR
        [SerializeField]
        private bool updateInEditor = true;
        #endif

        public abstract void ApplyStyles();

        protected virtual void Awake()
        {
            ApplyStyles();
        }

        protected virtual void OnValidate()
        {
            #if UNITY_EDITOR
            if (updateInEditor) {
                ApplyStyles();
            }
            #endif
        }

        protected void RequestUpdate()
        {
            if (Application.isPlaying || updateInEditor) {
                ApplyStyles();
            }
        }

    }

}

using System.Collections;
using UnityEngine;

namespace Zigurous.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupDisplay : ShowableAnimatedDisplay
    {
        [Header("Interaction")]
        public bool interactable = true;
        public bool blocksRaycasts = true;

        private CanvasGroup m_CanvasGroup;
        public CanvasGroup canvasGroup
        {
            get
            {
                if (m_CanvasGroup == null) {
                    m_CanvasGroup = GetComponent<CanvasGroup>();
                }
                return m_CanvasGroup;
            }
        }

        public override bool IsShown => canvasGroup.alpha > 0f;

        protected override IEnumerator OnRequestAnimationShow(float duration)
        {
            return canvasGroup.FadeIn(duration);
        }

        protected override IEnumerator OnRequestAnimationHide(float duration)
        {
            return canvasGroup.FadeOut(duration);
        }

        protected override void OnRequestShow()
        {
            canvasGroup.alpha = 1f;
        }

        protected override void OnRequestHide()
        {
            canvasGroup.alpha = 0f;
        }

        protected override void OnShown()
        {
            canvasGroup.interactable = interactable;
            canvasGroup.blocksRaycasts = blocksRaycasts;

            base.OnShown();
        }

        protected override void OnHidden()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            base.OnHidden();
        }

    }

}

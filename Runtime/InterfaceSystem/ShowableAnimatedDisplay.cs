using System.Collections;
using UnityEngine;

namespace Zigurous.UI
{
    public abstract class ShowableAnimatedDisplay : ShowableDisplay, IShowableAnimatedDisplay
    {
        [Header("Animation")]

        [Tooltip("Whether the display should be animated by default.")]
        public bool animated = true;

        [Tooltip("The default duration of the animation.")]
        public float animationDuration = 0.2f;

        public bool IsAnimating => m_Animation != null;

        protected Coroutine m_Animation;

        public override void Show()
        {
            if (animated)
            {
                ShowAnimated();
            }
            else
            {
                OnRequestShow();
                OnShown();
            }
        }

        public void ShowAnimated()
        {
            ShowAnimated(this.animationDuration);
        }

        public void ShowAnimated(float duration)
        {
            CancelAnimation();
            m_Animation = StartCoroutine(Animate(OnRequestAnimationShow(duration), OnShown));
        }

        public override void Hide()
        {
            if (animated)
            {
                HideAnimated();
            }
            else
            {
                OnRequestHide();
                OnHidden();
            }
        }

        public void HideAnimated()
        {
            HideAnimated(this.animationDuration);
        }

        public void HideAnimated(float duration)
        {
            CancelAnimation();
            m_Animation = StartCoroutine(Animate(OnRequestAnimationHide(duration), OnHidden));
        }

        public void InvokeShowAnimated(float time)
        {
            Invoke(nameof(ShowAnimated), time);
        }

        public void InvokeHideAnimated(float time)
        {
            Invoke(nameof(HideAnimated), time);
        }

        private void CancelAnimation()
        {
            if (m_Animation != null)
            {
                StopCoroutine(m_Animation);
                m_Animation = null;
            }
        }

        private IEnumerator Animate(IEnumerator animation, System.Action callback)
        {
            yield return animation;
            m_Animation = null;
            callback.Invoke();
        }

        protected abstract IEnumerator OnRequestAnimationShow(float duration);
        protected abstract IEnumerator OnRequestAnimationHide(float duration);

        protected abstract void OnRequestShow();
        protected abstract void OnRequestHide();

    }

}

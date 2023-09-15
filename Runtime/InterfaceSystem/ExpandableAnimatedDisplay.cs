using System.Collections;
using UnityEngine;

namespace Zigurous.UI
{
    public abstract class ExpandableAnimatedDisplay : ExpandableDisplay, IExpandableAnimatedDisplay
    {
        [Header("Animation")]

        [Tooltip("Whether the display should be animated by default.")]
        public bool animated = true;

        [Tooltip("The default duration of the animation.")]
        public float animationDuration = 0.2f;

        public bool IsAnimating => m_Animation != null;

        protected Coroutine m_Animation;

        public override void Expand()
        {
            if (animated)
            {
                ExpandAnimated();
            }
            else
            {
                OnRequestExpand();
                OnExpanded();
            }
        }

        public void ExpandAnimated()
        {
            ExpandAnimated(this.animationDuration);
        }

        public void ExpandAnimated(float duration)
        {
            CancelAnimation();
            m_Animation = StartCoroutine(Animate(OnRequestAnimationExpand(duration), OnExpanded));
        }

        public override void Collapse()
        {
            if (animated)
            {
                CollapseAnimated();
            }
            else
            {
                OnRequestCollapse();
                OnCollapsed();
            }
        }

        public void CollapseAnimated()
        {
            CollapseAnimated(this.animationDuration);
        }

        public void CollapseAnimated(float duration)
        {
            CancelAnimation();
            m_Animation = StartCoroutine(Animate(OnRequestAnimationCollapse(duration), OnCollapsed));
        }

        public void InvokeExpandAnimated(float time)
        {
            Invoke(nameof(ExpandAnimated), time);
        }

        public void InvokeCollapseAnimated(float time)
        {
            Invoke(nameof(CollapseAnimated), time);
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

        protected abstract IEnumerator OnRequestAnimationExpand(float duration);
        protected abstract IEnumerator OnRequestAnimationCollapse(float duration);

        protected abstract void OnRequestExpand();
        protected abstract void OnRequestCollapse();
    }

}

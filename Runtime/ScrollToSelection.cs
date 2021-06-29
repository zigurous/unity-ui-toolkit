using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Zigurous.UI
{
    [RequireComponent(typeof(ScrollRect))]
    [AddComponentMenu("Zigurous/UI/Navigation/Scroll To Selection")]
    public class ScrollToSelection : MonoBehaviour
    {
        public enum ScrollDirection
        {
            Vertical,
            Horizontal,
            Both,
        }

        public ScrollDirection scrollDirection;
        public float scrollSpeed = 10.0f;
        public bool manualScrolling { get; private set; }

        public ScrollRect scrollRect { get; private set; }
        public RectTransform scrollTransform { get; private set; }
        public GameObject selectedGameObject { get; private set; }
        public RectTransform selectedTransform { get; private set; }

        private void Awake()
        {
            this.scrollRect = GetComponent<ScrollRect>();
            this.scrollTransform = this.scrollRect.GetComponent<RectTransform>();
        }

        private void Update()
        {
            CheckForManualScrolling();
            SetSelectedGameObject();
            SetScrollPosition();
        }

        private void CheckForManualScrolling()
        {
            if (Mouse.current != null && Mouse.current.scroll.y.IsActuated()) {
                this.manualScrolling = true;
            }
        }

        private void SetSelectedGameObject()
        {
            EventSystem eventSystem = EventSystem.current;

            if (eventSystem == null || eventSystem.currentSelectedGameObject == null) {
                return;
            }

            if (eventSystem.currentSelectedGameObject != this.selectedGameObject &&
                eventSystem.currentSelectedGameObject.transform.parent == this.scrollRect.content)
            {
                this.selectedGameObject = eventSystem.currentSelectedGameObject;
                this.selectedTransform = this.selectedGameObject.GetComponent<RectTransform>();
                this.manualScrolling = false;
            }
        }

        private void SetScrollPosition()
        {
            if (this.selectedTransform == null || this.manualScrolling) {
                return;
            }

            switch (this.scrollDirection)
            {
                case ScrollDirection.Vertical:
                    ScrollVertical(this.selectedTransform);
                    break;

                case ScrollDirection.Horizontal:
                    ScrollHorizontal(this.selectedTransform);
                    break;

                case ScrollDirection.Both:
                    ScrollVertical(this.selectedTransform);
                    ScrollHorizontal(this.selectedTransform);
                    break;
            }
        }

        private void ScrollVertical(RectTransform selection)
        {
            // Calculate the scroll offset
            float elementHeight = selection.rect.height;
            float maskHeight = this.scrollTransform.rect.height;
            float anchorPosition = this.scrollRect.content.anchoredPosition.y;
            float selectionPosition = -selection.anchoredPosition.y - (elementHeight * (1.0f - selection.pivot.y));
            float offset = GetScrollOffset(selectionPosition, anchorPosition, elementHeight, maskHeight);

            // Move the target scroll rect
            float position = this.scrollRect.verticalNormalizedPosition;
            position = Mathf.Clamp01(position + (offset / this.scrollRect.content.rect.height) * Time.unscaledDeltaTime * this.scrollSpeed);
            this.scrollRect.verticalNormalizedPosition = position;
        }

        private void ScrollHorizontal(RectTransform selection)
        {
            // Calculate the scroll offset
            float selectionPosition = -selection.anchoredPosition.x - (selection.rect.width * (1.0f - selection.pivot.x));
            float elementWidth = selection.rect.width;
            float maskWidth = this.scrollTransform.rect.width;
            float anchorPosition = -this.scrollRect.content.anchoredPosition.x;
            float offset = -GetScrollOffset(selectionPosition, anchorPosition, elementWidth, maskWidth);

            // Move the target scroll rect
            float position = this.scrollRect.horizontalNormalizedPosition;
            position = Mathf.Clamp01(position + (offset / this.scrollRect.content.rect.width) * Time.unscaledDeltaTime * this.scrollSpeed);
            this.scrollRect.horizontalNormalizedPosition = position;
        }

        private float GetScrollOffset(float position, float anchorPosition, float targetLength, float maskLength)
        {
            if (position < anchorPosition + (targetLength * 0.5f)) {
                return (anchorPosition + maskLength) - (position - targetLength);
            }
            else if (position + targetLength > anchorPosition + maskLength) {
                return (anchorPosition + maskLength) - (position + targetLength);
            }

            return 0.0f;
        }

    }

}

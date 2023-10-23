using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.UI;

namespace Zigurous.UI
{
    /// <summary>
    /// Handles scrolling a ScrollRect component to the selected child element.
    /// This is especially useful for controller support.
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    [AddComponentMenu("Zigurous/UI/Navigation/Scroll To Selection")]
    [HelpURL("https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI/ScrollToSelection")]
    public class ScrollToSelection : MonoBehaviour
    {
        /// <summary>
        /// The ScrollRect component being scrolled (Read only).
        /// </summary>
        public ScrollRect scrollRect { get; private set; }

        /// <summary>
        /// The RectTransform component of the scroll rect (Read only).
        /// </summary>
        public RectTransform scrollTransform { get; private set; }

        /// <summary>
        /// The current selected game object (Read only).
        /// </summary>
        public GameObject selectedGameObject { get; private set; }

        /// <summary>
        /// The RectTransform of the current selected game object (Read only).
        /// </summary>
        public RectTransform selectedTransform { get; private set; }

        /// <summary>
        /// The direction to scroll the ScrollRect.
        /// </summary>
        [Tooltip("The direction to scroll the ScrollRect.")]
        public ScrollDirection scrollDirection = ScrollDirection.Vertical;

        /// <summary>
        /// How quickly the ScrollRect scrolls.
        /// </summary>
        [Tooltip("How quickly the ScrollRect scrolls.")]
        public float scrollSpeed = 10f;

        /// <summary>
        /// Whether the ScrollRect is currently being scrolled manually. This
        /// allows the user to freely scroll with the mouse even when a child
        /// element is selected (Read only).
        /// </summary>
        public bool manualScrolling { get; private set; }

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            scrollTransform = scrollRect.GetComponent<RectTransform>();
        }

        private void Update()
        {
            CheckForManualScrolling();
            SetSelectedGameObject();
            SetScrollPosition();
        }

        private void CheckForManualScrolling()
        {
            #if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                switch (scrollDirection)
                {
                    case ScrollDirection.Horizontal:
                        if (Mouse.current.scroll.x.IsActuated()) {
                            manualScrolling = true;
                        }
                        break;

                    case ScrollDirection.Vertical:
                        if (Mouse.current.scroll.y.IsActuated()) {
                            manualScrolling = true;
                        }
                        break;

                    case ScrollDirection.Both:
                        if (Mouse.current.scroll.x.IsActuated() || Mouse.current.scroll.y.IsActuated()) {
                            manualScrolling = true;
                        }
                        break;
                }
            }
            #elif ENABLE_LEGACY_INPUT_MANAGER
            switch (scrollDirection)
            {
                case ScrollDirection.Horizontal:
                    if (Input.mouseScrollDelta.x != 0f) {
                        manualScrolling = true;
                    }
                    break;

                case ScrollDirection.Vertical:
                    if (Input.mouseScrollDelta.y != 0f) {
                        manualScrolling = true;
                    }
                    break;

                case ScrollDirection.Both:
                    if (Input.mouseScrollDelta.x != 0f || Input.mouseScrollDelta.y != 0f) {
                        manualScrolling = true;
                    }
                    break;
            }
            #endif
        }

        private void SetSelectedGameObject()
        {
            EventSystem eventSystem = EventSystem.current;

            if (eventSystem == null || eventSystem.currentSelectedGameObject == null) {
                return;
            }

            if (eventSystem.currentSelectedGameObject != selectedGameObject &&
                eventSystem.currentSelectedGameObject.transform.parent == scrollRect.content)
            {
                selectedGameObject = eventSystem.currentSelectedGameObject;
                selectedTransform = selectedGameObject.GetComponent<RectTransform>();
                manualScrolling = false;
            }
        }

        private void SetScrollPosition()
        {
            if (selectedTransform == null || manualScrolling) {
                return;
            }

            switch (scrollDirection)
            {
                case ScrollDirection.Vertical:
                    ScrollVertical(selectedTransform);
                    break;

                case ScrollDirection.Horizontal:
                    ScrollHorizontal(selectedTransform);
                    break;

                case ScrollDirection.Both:
                    ScrollVertical(selectedTransform);
                    ScrollHorizontal(selectedTransform);
                    break;
            }
        }

        private void ScrollVertical(RectTransform selection)
        {
            // Calculate the scroll offset
            float elementHeight = selection.rect.height;
            float maskHeight = scrollTransform.rect.height;
            float anchorPosition = scrollRect.content.anchoredPosition.y;
            float selectionPosition = -selection.anchoredPosition.y - (elementHeight * (1f - selection.pivot.y));
            float offset = GetScrollOffset(selectionPosition, anchorPosition, elementHeight, maskHeight);

            // Move the target scroll rect
            float position = scrollRect.verticalNormalizedPosition;
            position = Mathf.Clamp01(position + (offset / scrollRect.content.rect.height) * Time.unscaledDeltaTime * scrollSpeed);
            scrollRect.verticalNormalizedPosition = position;
        }

        private void ScrollHorizontal(RectTransform selection)
        {
            // Calculate the scroll offset
            float selectionPosition = -selection.anchoredPosition.x - (selection.rect.width * (1f - selection.pivot.x));
            float elementWidth = selection.rect.width;
            float maskWidth = scrollTransform.rect.width;
            float anchorPosition = -scrollRect.content.anchoredPosition.x;
            float offset = -GetScrollOffset(selectionPosition, anchorPosition, elementWidth, maskWidth);

            // Move the target scroll rect
            float position = scrollRect.horizontalNormalizedPosition;
            position = Mathf.Clamp01(position + (offset / scrollRect.content.rect.width) * Time.unscaledDeltaTime * scrollSpeed);
            scrollRect.horizontalNormalizedPosition = position;
        }

        private float GetScrollOffset(float position, float anchorPosition, float targetLength, float maskLength)
        {
            if (position < anchorPosition + (targetLength * 0.5f)) {
                return (anchorPosition + maskLength) - (position - targetLength);
            }
            else if (position + targetLength > anchorPosition + maskLength) {
                return (anchorPosition + maskLength) - (position + targetLength);
            }

            return 0f;
        }

    }

}

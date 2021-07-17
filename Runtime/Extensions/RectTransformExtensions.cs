using UnityEngine;

namespace Zigurous.UI
{
    /// <summary>
    /// Extension methods for RectTransform.
    /// </summary>
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Sets the width of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="width">The width value to set.</param>
        public static void SetWidth(this RectTransform rect, float width) =>
            rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);

        /// <summary>
        /// Sets the height of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="height">The height value to set.</param>
        public static void SetHeight(this RectTransform rect, float height) =>
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);

        /// <summary>
        /// Sets the left offset of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="left">The left offset value to set.</param>
        public static void SetLeft(this RectTransform rect, float left) =>
            rect.offsetMin = new Vector2(left, rect.offsetMin.y);

        /// <summary>
        /// Sets the right offset of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="right">The right offset value to set.</param>
        public static void SetRight(this RectTransform rect, float right) =>
            rect.offsetMax = new Vector2(-right, rect.offsetMax.y);

        /// <summary>
        /// Sets the top offset of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="top">The top offset value to set.</param>
        public static void SetTop(this RectTransform rect, float top) =>
            rect.offsetMax = new Vector2(rect.offsetMax.x, -top);

        /// <summary>
        /// Sets the bottom offset of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="bottom">The bottom offset value to set.</param>
        public static void SetBottom(this RectTransform rect, float bottom) =>
            rect.offsetMin = new Vector2(rect.offsetMin.x, bottom);

        /// <summary>
        /// Sets the left anchor of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="minX">The left anchor value to set.</param>
        public static void SetAnchorMinX(this RectTransform rect, float minX) =>
            rect.anchorMin = new Vector2(minX, rect.anchorMin.y);

        /// <summary>
        /// Sets the bottom anchor of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="minY">The bottom anchor value to set.</param>
        public static void SetAnchorMinY(this RectTransform rect, float minY) =>
            rect.anchorMin = new Vector2(rect.anchorMin.x, minY);

        /// <summary>
        /// Sets the right anchor of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="maxX">The right anchor value to set.</param>
        public static void SetAnchorMaxX(this RectTransform rect, float maxX) =>
            rect.anchorMax = new Vector2(maxX, rect.anchorMax.y);

        /// <summary>
        /// Sets the top anchor of the rect transform to the given value.
        /// </summary>
        /// <param name="rect">The rect transform to update.</param>
        /// <param name="maxY">The top anchor value to set.</param>
        public static void SetAnchorMaxY(this RectTransform rect, float maxY) =>
            rect.anchorMax = new Vector2(rect.anchorMax.x, maxY);

    }

}

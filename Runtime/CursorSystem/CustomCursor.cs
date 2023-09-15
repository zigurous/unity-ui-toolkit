using UnityEngine;

namespace Zigurous.UI
{
    [CreateAssetMenu(menuName = "Zigurous/UI/Custom Cursor")]
    public sealed class CustomCursor : ScriptableObject
    {
        [Tooltip("The texture to use for the cursor. To use a texture, you must first import it with `Read/Write`enabled. Alternatively, you can use the default cursor import setting. If you created your cursor texture from code, it must be in RGBA32 format, have alphaIsTransparency enabled, and have no mip chain. To use the default cursor, set the texture to `Null`.")]
        public Texture2D texture;

        [Tooltip("The offset from the top left of the texture to use as the target point (must be within the bounds of the cursor).")]
        public Vector2 hotspot;

        [Tooltip("Allow this cursor to render as a hardware cursor on supported platforms, or force software cursor.")]
        public CursorMode mode = CursorMode.Auto;

        /// <summary>
        /// Sets the custom cursor image.
        /// </summary>
        public void Apply()
        {
            Cursor.SetCursor(texture, hotspot, mode);
        }

        /// <summary>
        /// Unsets the custom cursor image.
        /// </summary>
        /// <param name="revertToDefault">Applies the default cursor of the current state specified by the <see cref="CursorController"/>, otherwise unassigns the cursor image.</param>
        public void Unset(bool revertToDefault = true)
        {
            CustomCursor cursor = CursorController.CurrentState?.defaultCursor;

            if (revertToDefault && cursor != null && cursor != this) {
                cursor.Apply();
            } else {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

    }

}

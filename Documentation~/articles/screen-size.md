---
slug: "/manual/screen-size"
---

# Screen Size

Detecting changes to the screen size is very common for many different use cases in games. The **UI Toolkit** package includes the singleton [ScreenSizeListener](/api/Zigurous.UI/ScreenSizeListener) that allows you to register a callback to know when the screen size changes.

```csharp
ScreenSizeListener.Instance.resized += OnResize;

private void OnResize(float width, float height)
{
    // handle resize here
}
```

You can simply get the current screen size too. There seems to be a bug with Unity's `Screen.width` and `Screen.height` because they do not always return the correct values. Using the listener can be more reliable.

```csharp
if (ScreenSizeListener.HasInstance)
{
    // Individual properties
    float width = ScreenSizeListener.Instance.width;
    float height = ScreenSizeListener.Instance.height;

    // Single property
    Vector2Int size = ScreenSizeListener.Instance.size;
}
```

# UI Toolkit

The UI Toolkit package contains scripts and utilities for creating UI in Unity projects. The package is intended to solve common problems that arise when developing UI and menus. The package is still early in development, and more scripts will be added over time.

## Installation

Use the Unity [Package Manager](https://docs.unity3d.com/Manual/upm-ui.html) to install the UI Toolkit package.

1. Open the Package Manager in `Window > Package Manager`
2. Click the add (`+`) button in the status bar
3. Select `Add package from git URL` from the add menu
4. Enter the following Git URL in the text box and click Add:

```http
https://github.com/zigurous/unity-ui-toolkit.git
```

For more information on the Package Manager and installing packages, see the following pages:

- [Unity's Package Manager](https://docs.unity3d.com/Manual/Packages.html)
- [Installing from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

### Importing

Import the package namespace in each script or file you want to use it.

> **Note**: You may need to regenerate project files/assemblies first.

```csharp
using Zigurous.UI;
```

## Reference

### API

- [NavigationStack](https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI.NavigationStack.html)
- [ScrollToSelection](https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI.ScrollToSelection.html)
- [ScrollWithInput](https://docs.zigurous.com/com.zigurous.ui/api/Zigurous.UI.ScrollWithInput.html)

### Manual

- [Menus](https://docs.zigurous.com/com.zigurous.ui/manual/menus.html)


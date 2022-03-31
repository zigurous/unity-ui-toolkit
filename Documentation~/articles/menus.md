---
slug: "/manual/menus"
---

# Menu Tools

The **UI Toolkit** comes with a few scripts to assist in creating UI menus. There are several common problems that arise, especially in regards to menu navigation and controller-support. Generally speaking, basic navigation in menus across any input device can be achieved out-of-box using Unity's EventSystem component; however, this is not always the case.

<br/>

### [NavigationStack](/api/Zigurous.UI/NavigationStack)

This script manages a stack of game objects (menus) that you can navigate between. As you navigate to other menus or sub-menus, you push the menu's game object onto the stack. Then, when you need to navigate back to the previous menu, you simply pop the game object off the stack. The script can also automatically turn on/off the game objects as they are pushed and popped from the stack. The script also provides an InputAction to handle backwards navigation.

<hr/>

### [ScrollToSelection](/api/Zigurous.UI/ScrollToSelection)

This script works in conjunction with the EventSystem to handle scrolling a ScrollRect component to the selected game object. For example, when using a controller you usually do not freely scroll menus that display a list of buttons, such as a level select menu. Rather, you simply navigate up and down to the next or previous button, respectively. The ScrollRect will automatically be scrolled to whichever game object is selected.

<hr/>

### [ScrollWithInput](/api/Zigurous.UI/ScrollWithInput)

For other menus that use a ScrollRect component, you may just want to be able to freely scroll up and down. This may not be supported out of the box for controllers, so this script simply exposes an InputAction that you can bind controls to in order to scroll the menu. There is actually nothing specific to controllers in this script, so you can also use it with any kind of input, but generally mouse+keyboard already works with ScrollRect.

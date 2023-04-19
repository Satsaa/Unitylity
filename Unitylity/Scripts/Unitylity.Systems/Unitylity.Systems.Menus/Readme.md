# Menus

## Purpose

Provides a way to handle complex stacks of menus using prefabs. 

## Menus

Singleton which handles the Menu stack and contains functions to show/hide Menus. Menus keeps a stack of the currently shown menus. The stack can be modified at the top, but it cannot be modified from the middle.

### API

```cs
// Create an instance of source Menu if needed and shows it.
public static Menu Show(Menu source, [Action<Menu> initializer], [bool animate]);
// Hides the top-most Menu
public static void Pop();
// Hides the top-most Menu that is target or instance of it
public static bool Hide(Menu target);
// Hides the Menu that is positioned at the index in the menu stack
public static void Hide(int index);
```

## Menu

Component for Menu prefabs. Contains multiple settings to modify how the Menu item behaves in the stack.


#### Base settings:
```cs

```

# Menus

## Purpose

Provides a way to handle complex stacks of menus using prefabs. 

## Menus

Singleton which handles the Menu stack and contains functions to show/hide Menus. Menus keeps a stack of the currently shown menus. The stack can be modified at the top, but it cannot be modified from the middle.

### API

```cs
// Shows a Menu representing the source Menu
Menu Show(Menu source, bool animate = true);

// Removes the last/newest Menu
void Pop(bool animate = true);

// Pops until the a root Menu with the same group is removed (only if one is found)
void RemoveRoot(Menu source, bool animate = true);
// Pops until any root Menu is removed (only if one is found)
void RemoveRoot(bool animate = true);
// Pops until any root Menu of the same group is at the top (only if one is found)
void ExposeRoot(Menu source, bool animate = true);
// Pops until any root Menu is at the top (only if one is found)
void ExposeRoot(bool animate = true);

```

## Menu

Component for Menu prefabs. Contains multiple settings to modify how the Menu item behaves in the stack.


#### Base settings:
```cs
// Menus with the same group will replace each other.
string group;
// Root Menus will pop previous Menus until a root Menu with the same replace group is popped (only if there is one).
bool isGroupRoot;
// Keep one instance of this for later use.
bool cache;
// Menu before this will not be hidden.
bool showPrevious;
// This object will be automatically selected when the Menu is at top.
GameObject select;
```

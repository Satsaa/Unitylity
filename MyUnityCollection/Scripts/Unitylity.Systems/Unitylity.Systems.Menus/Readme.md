# Menus

## Purpose

Provides a way to handle complex stacks of menus using prefabs. 

## Menus

Singleton which handles the Menu stack and contains functions to show/hide Menus. 

## Menu

Component for Menu prefabs. Contains multiple settings to modify how the Menu item behaves in the stack.


#### Base settings:
```cs
"Used to compare Menu types. Many settings define their behaviour by comparing types of Menus"
string type

"Will this Menu close the previous chain of Menus that are replaceable?"
bool replace

"Will this Menu be closed when a Replacing Menu is added after this?"
bool replaceable

"Disallow multiple consecutive Menus of this type? The newest one is used."
bool collapse

"Reuse this Menu for upcoming Menus of the same type? No per-menu data is stored."
bool reuse

"Don't destroy the last instance of a Menu of this type, complementing the reuse toggle."
bool persist

"Don't hide the Menu when it is not at the top?"
bool alwaysVisible

"Does the close key defined by Menus close this menu?"
bool allowCloseKey
```

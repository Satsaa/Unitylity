# Interaction

## Purpose

This system is for making it easy to create interactable buttons, and allow objects to be dragged/moved by the user. A familiar example of this might be Skyrim.

## Interactor

The Interactor is a raycasting machine usually located directly in the middle of the player Camera and pointing forward. When a button is pressed/held/released "interaction" occurs and a UnityEvent is triggered. 

## Interactable

When an Interactor interacts with this object onActivate/onActive/onDeactivate is triggered. You can for example make these events call the functions of Movable to allow moving that object.

## Movable and Throwable

Movable and Throwable are reference implementations for... movable and throwable objects
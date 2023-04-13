# Input

## Purpose

The Unitylity.Systems.Input contains Components for making use of the "New Input System" easier and more *modular*. This system does not currently allow for stopping the propagation (using) of events caused by e.g. button presses.

## XInput

A XInput Component exists for the corresponding XControl, so an ButtonInput exists for ButtonControl.

XInput requires a control to be set for it to function. When said control changes state the corresponding UnityEvents are fired (onChange is always fired, onPress when pressed and so on).
# Popups

## Purpose

This provides a way to create message box like popups with pressable options. Prefabs can be used to create presets that get built into popups during runtime.

## Popups

Singleton which provides the functions for showing Popups and handles the external behaviour of Popups.

## Popup

Base Component for popup prefabs.

## PopupOption

Base Component for options shown in Popups (e.g. yes/cancel/no/skip).

## PopupPreset

ScriptableObject containing the info for creating a specific combination of the Popup prefab and PopupOption prefabs.

![RenderObject](/Res/Popups/PopupExample.png)

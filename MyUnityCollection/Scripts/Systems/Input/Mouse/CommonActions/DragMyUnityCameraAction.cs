

namespace Muc.Input.Mouse {

  using System;
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;

  using UnityEngine;

  using Muc.Camera;
  using Muc.Types.Extensions;
  using Muc.Inspector;
  using Muc.Types;

  [RequireComponent(typeof(MouseActionHandler))]
  [RequireComponent(typeof(MyUnityCameraDrag))]
  public class DragMyUnityCameraAction : MonoBehaviour {

    private enum ActivationType {
      targetIsNull,
      targetIsNotNull,
      targetIsTag,
      targetIsLayer,
      targetIsAnyOf,
      targetIsNotAnyOf,
      targetIsNotSelected,
      custom
    }

    [SerializeField]
    private ActivationType activationType = 0;


    // Tag
    [SerializeField, DrawIf(nameof(activationType), ActivationType.targetIsTag)]
    private string targetTag = "Draggable";

    // Layer
    [SerializeField, DrawIf(nameof(activationType), ActivationType.targetIsLayer)]
    private LayerMask targetLayer = LayerMask.GetMask("Default");

    // Any of or not any of
    [SerializeField/* , DrawIf(nameof(activationType), ActivationType.targetIsAnyOf, ActivationType.targetIsNotAnyOf) */]
    private List<GameObject> targets = null;

    // Custom
    [SerializeField, DrawIf(nameof(activationType), ActivationType.custom)]
    private ScriptablePredicate customPredicate = null;


    void OnValidate() {
      if (!GetComponent<SelectionHandler>() && activationType == ActivationType.targetIsNotSelected)
        Debug.LogWarning($"You have set {nameof(activationType)} to {ActivationType.targetIsNotSelected} which requires a {nameof(SelectionHandler)} Component on the GameObject!", this);
    }

    void Start() {

      var selection = GetComponent<SelectionHandler>();
      var handler = GetComponent<MouseActionHandler>();
      var dragger = GetComponent<MyUnityCameraDrag>();

      Predicate<GameObject> predicate = null;

      switch (activationType) {
        case ActivationType.targetIsNull:
          predicate = g => !g;
          break;
        case ActivationType.targetIsNotNull:
          predicate = g => g;
          break;
        case ActivationType.targetIsTag:
          predicate = g => g.tag == targetTag;
          break;
        case ActivationType.targetIsLayer:
          predicate = g => g.layer == targetLayer;
          break;
        case ActivationType.targetIsAnyOf:
          predicate = g => targets.Contains(g);
          break;
        case ActivationType.targetIsNotAnyOf:
          predicate = g => !targets.Contains(g);
          break;
        case ActivationType.targetIsNotSelected:
          predicate = g => !selection.Contains(g);
          break;
        case ActivationType.custom:
          predicate = customPredicate.predicate;
          break;
      }

      // Drag and move MyUnityCamera using MyUnityCameraDrag methods
      handler.AddMouseHotkey(
          new DragAction(
            name: "Move Camera Along Plane",
            specifiers: HotkeySpecifier.Persistent | HotkeySpecifier.Alt,
            predicate: predicate,
            noPromote: false,

            start: (x, vec) => {
              dragger.Init();
            },

            drag: (x, v) => {
              dragger.UpdateDrag();
            },

            end: (x, vec) => {

            }

          )
        );

    }
  }
}


namespace Muc.Input.Mouse {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using System.Linq;

  using UnityEngine;

  using Muc.Collections;
  using Muc.Types.Extensions;

  [RequireComponent(typeof(Camera))]
  public class MouseActionHandler : MonoBehaviour {

    [Tooltip("LayerMask used with RayCast when clicking the primary mouse button")]
    public LayerMask defaultMask;

    [Tooltip("Minimum pixel distance dragged before registering drag. Prevents accidental drags.")]
    public float minDragDist = 2;

    [Tooltip("The primary button. Mostly used for selecting targets")]
    public KeyCode primaryKey = KeyCode.Mouse0;
    [Tooltip("The secondary button. Mostly used for move targeting")]
    public KeyCode secondaryKey = KeyCode.Mouse1;

    public KeyCode controlKey = KeyCode.LeftControl;
    public KeyCode altKey = KeyCode.LeftAlt;
    public KeyCode shiftKey = KeyCode.LeftShift;


    public IReadOnlyList<MouseAction> actions => _actions;
    private OrderedList<MouseAction> _actions = new OrderedList<MouseAction>((a, b) => a.priority - b.priority);

    private DragAction dragAction = null;
    private ClickAction dragClickActionFallback = null;
    private GameObject dragTarget;
    private bool dragIniting;
    private float dragStartDist;
    private Vector2 dragInitScreenPos;


    private new Camera camera;


    public void Awake() {
      camera = GetComponent<Camera>();
    }

    // Call from component
    public void Update() {
      if (HandleDrag()) return;
      HandleActions(WhereActive(_actions));
    }

    private bool HandleDrag() {
      if (dragAction is null) return false;

      if (dragIniting) {

        // Cancel if releases before starting drag
        if (!Input.GetKey(dragAction.specifiers.HasFlag(HotkeySpecifier.Secondary) ? secondaryKey : primaryKey)) {
          dragAction = null;

          if (dragClickActionFallback && dragClickActionFallback.validate(dragTarget)) {
            ExecuteClickAction(dragClickActionFallback, dragTarget);
          }
          return false;
        }

        // Check if enough movement
        var dragDist = Vector2.Distance(dragInitScreenPos, Input.mousePosition);
        if (dragDist < minDragDist) return true;

        dragIniting = false;
        dragAction.start(dragTarget, GetDragPosition());
      }

      // End if released
      if (!Input.GetKey(dragAction.specifiers.HasFlag(HotkeySpecifier.Secondary) ? secondaryKey : primaryKey)) {
        dragAction.end(dragTarget, GetDragPosition());
        if (!dragAction.specifiers.HasFlag(HotkeySpecifier.Persistent)) _actions.Remove(dragAction);
        dragAction = null;
        return false;
      }

      dragAction.drag(dragTarget, GetDragPosition());


      return true;

      Vector3 GetDragPosition() => camera.transform.position + camera.ScreenPointToRay(Input.mousePosition).direction * dragStartDist;
    }


    public void HandleActions(IEnumerable<MouseAction> actions) {

      (ClickAction a, GameObject g) clickActionData = (null, null);
      (DragAction a, GameObject g) dragActionData = (null, null);

      // Find target
      GameObject target = null;
      GameObject promoTarget = null;
      if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var hit)) {
        target = hit.collider.gameObject;
      }

      // Cache keys
      var primary = Input.GetKeyDown(primaryKey);
      var secondary = Input.GetKeyDown(secondaryKey);


      foreach (var action in actions) {

        if (dragActionData.a && clickActionData.a) break;

        if (
          (primary && !action.specifiers.HasFlag(HotkeySpecifier.Secondary)) ||
          (secondary && action.specifiers.HasFlag(HotkeySpecifier.Secondary))
        ) {

          var finalTarget = (action.promote && promoTarget) ? promoTarget : target;

          if (action.validate(finalTarget)) {

            // Handle different MouseAction types
            switch (action) {

              case DragAction dragAction:
                dragActionData = (dragAction, finalTarget);
                break;

              case ClickAction clickAction:
                clickActionData = (clickAction, finalTarget);
                break;
            }

            break;
          }

        }

      }

      if (dragActionData.a) InitDragAction(dragActionData.a, dragActionData.g, clickActionData.a);
      else if (clickActionData.a) ExecuteClickAction(clickActionData.a, clickActionData.g);

    }

    private void InitDragAction(DragAction dragAction, GameObject target, ClickAction mouseActionFallback) {
      dragIniting = true;
      this.dragAction = dragAction;
      dragInitScreenPos = Input.mousePosition;
      dragStartDist = target is null ? 1 : Vector3.Distance(camera.transform.position, target.transform.position);
      dragTarget = target;
      dragClickActionFallback = mouseActionFallback;
    }

    private void ExecuteClickAction(ClickAction clickAction, GameObject target) {
      clickAction.action(target);
      if (!clickAction.specifiers.HasFlag(HotkeySpecifier.Persistent)) _actions.Remove(clickAction);
    }

    public IEnumerable<MouseAction> WhereActive() => WhereActive(_actions);
    public IEnumerable<T> WhereActive<T>(IList<T> actions) where T : MouseAction {

      var control = Input.GetKey(controlKey);
      var alt = Input.GetKey(altKey);
      var shift = Input.GetKey(shiftKey);

      // Gets highest priority MouseActions and filter for modifiers
      var prevPoints = int.MinValue;
      for (int i = actions.Count - 1; i >= 0; i--) {
        var mit = actions[i];
        if (mit.priority < prevPoints) break;

        prevPoints = mit.priority;

        var spec = mit.specifiers;

        if (!spec.HasFlag(HotkeySpecifier.AllowControl) && control != spec.HasFlag(HotkeySpecifier.Control)) continue;
        if (!spec.HasFlag(HotkeySpecifier.AllowAlt) && alt != spec.HasFlag(HotkeySpecifier.Alt)) continue;
        if (!spec.HasFlag(HotkeySpecifier.AllowShift) && shift != spec.HasFlag(HotkeySpecifier.Shift)) continue;

        yield return actions[i];
      }

    }

    public void AddMouseHotkey(MouseAction mit) {
      _actions.Add(mit);
    }

  }
}
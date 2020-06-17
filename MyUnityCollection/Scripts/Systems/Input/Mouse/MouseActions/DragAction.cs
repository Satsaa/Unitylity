


namespace Muc.Input.Mouse {

  using System;
  using UnityEngine;

  using DragAction = System.Action<UnityEngine.GameObject, UnityEngine.Vector3>;


  public class DragAction : MouseAction {

    public Action<GameObject, Vector3> start;
    public Action<GameObject, Vector3> drag;
    public Action<GameObject, Vector3> end;


    public DragAction(string name, HotkeySpecifier specifiers, Predicate<GameObject> predicate, bool noPromote, Action<GameObject, Vector3> start, Action<GameObject, Vector3> drag, Action<GameObject, Vector3> end)
      : base(name, specifiers, predicate, noPromote) {

      this.start = start;
      this.drag = drag;
      this.end = end;
    }

    public DragAction(string name, HotkeySpecifier specifiers, Predicate<GameObject> predicate, Action<GameObject, Vector3> start, Action<GameObject, Vector3> drag, Action<GameObject, Vector3> end)
      : this(name, specifiers, predicate, false, start, drag, end) { }

  }

}
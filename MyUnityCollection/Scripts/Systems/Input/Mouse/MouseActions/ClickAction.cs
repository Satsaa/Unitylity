


namespace Muc.Input.Mouse {

  using System;
  using UnityEngine;

  [Serializable]
  public class ClickAction : MouseAction {


    /// <summary> Will be called automatically if the user activates a valid target </summary>
    public Action<GameObject> action;

    public ClickAction(string name, HotkeySpecifier specifiers, Predicate<GameObject> predicate, Action<GameObject> action)
      : this(name, specifiers, predicate, false, action) { }

    public ClickAction(string name, HotkeySpecifier specifiers, Predicate<GameObject> predicate, bool noPromote, Action<GameObject> action)
      : base(name, specifiers, predicate, noPromote) {

      this.action = action;
    }

  }

}
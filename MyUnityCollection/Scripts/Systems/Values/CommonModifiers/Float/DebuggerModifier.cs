

namespace Muc.Systems.Values {
  using System;
  using UnityEngine;


  public class DebuggerModifier : Modifier<float> {

    public void Log(string message) {
      Debug.Log($"{nameof(DebuggerModifier)}: {message}");
    }


    public override bool enabled {
      get {
        Log($"{nameof(enabled)}.get()");
        return base.enabled;
      }
      set {
        Log($"{nameof(enabled)}.set({value})");
        base.enabled = value;
      }
    }

    public override Handler onGet => ((float current) => { Log($"{nameof(onGet)}({current})"); return current; });
    public override Handler onSet => ((float current) => { Log($"{nameof(onSet)}({current})"); return current; });
    public override Handler onAdd => ((float current) => { Log($"{nameof(onAdd)}({current})"); return current; });
    public override Handler onSub => ((float current) => { Log($"{nameof(onSub)}({current})"); return current; });

    public override bool CanBeAdded(Value<float> value) {
      var res = base.CanBeAdded(value);
      Log($"{nameof(CanBeAdded)}({value.GetType().Name}) => {res}");
      return res;
    }

    public override bool CanBeRemoved(Value<float> value) {
      var res = base.CanBeRemoved(value);
      Log($"{nameof(CanBeRemoved)}({value.GetType().Name}) => {res}");
      return res;
    }


    public override void OnModifierAdd(Value<float> value) {
      Log($"{nameof(OnModifierAdd)}({value.GetType().Name})");
      base.OnModifierAdd(value);
    }

    public override void OnModifierRemove(Value<float> value) {
      Log($"{nameof(OnModifierRemove)}({value.GetType().Name})");
      base.OnModifierRemove(value);
    }
  }
}
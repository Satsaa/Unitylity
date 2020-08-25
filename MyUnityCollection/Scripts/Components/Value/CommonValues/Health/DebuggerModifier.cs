

namespace Muc.Components.Values {
  using System;
  using UnityEngine;


  public class DebuggerModifier : HealthModifier {

    public void Log(string message) {
      Debug.Log($"{nameof(DebuggerModifier)} -> {message}");
    }


    public override bool enabled { get => base.enabled; set => base.enabled = value; }

    public override Handler onGet => ((float current) => { Log(nameof(onGet)); return current; });

    public override Handler onSet => ((float current) => { Log(nameof(onSet)); return current; });

    public override Handler onAdd => ((float current) => { Log(nameof(onAdd)); return current; });

    public override Handler onSub => ((float current) => { Log(nameof(onSub)); return current; });

    public override bool CanBeAdded(Health value) {
      Log(nameof(CanBeAdded));
      return base.CanBeAdded(value);
    }

    public override bool CanBeRemoved(Health value) {
      Log(nameof(CanBeRemoved));
      return base.CanBeRemoved(value);
    }


    public override void OnModifierAdd(Health value) {
      Log(nameof(OnModifierAdd));
      base.OnModifierAdd(value);
    }

    public override void OnModifierRemove(Health value) {
      Log(nameof(OnModifierRemove));
      base.OnModifierRemove(value);
    }
  }
}
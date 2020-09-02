

namespace Muc.Systems.Values {

  using System;
  using System.Collections.Generic;

  using UnityEngine;
  using UnityEngine.Events;


  public class Health : LimitedValue<float> {

    [Min(0)]
    public float max = 100;
    public bool enforceMax = true;

    protected override float defaultValue => max;
    protected override List<object> defaultModifiers => new List<object>() { };

    protected override float AddValues(float a, float b) => a + b;
    protected override float SubtractValues(float a, float b) => a - b;


    public void EnforceLimitIfNeed() {
      if (!enforceMax) return;
      var val = base.Get();
      if (val > max) {
        value -= val - max;
      }
    }

    public override float Get() {
      EnforceLimitIfNeed();
      return value;
    }
    public override float Set(float newValue) {
      var res = base.Set(newValue);
      EnforceLimitIfNeed();
      return res;
    }

    public override float Add(float addition) {
      EnforceLimitIfNeed();
      return base.Add(addition);
    }
    public override float Sub(float subtraction) {
      EnforceLimitIfNeed();
      return base.Sub(subtraction);
    }

  }
}
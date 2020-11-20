

namespace Muc.Systems.Values {

  using System;
  using System.Collections.Generic;

  using UnityEngine;
  using UnityEngine.Events;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
  [AddComponentMenu("")]
#else
  [AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Values) + "/" + nameof(Health))]
#endif
  public class Health : LimitedValue<float> {

    [field: SerializeField, Min(0)]
    public override float max { get; set; } = 100;

    [field: SerializeField]
    public override bool enforceMax { get; set; } = true;

    protected override float defaultValue => max;
    protected override List<object> defaultModifiers => new List<object>();

    protected override float AddValues(float a, float b) => a + b;
    protected override float SubtractValues(float a, float b) => a - b;
  }
}
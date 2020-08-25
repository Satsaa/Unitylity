

namespace Muc.Components.Values {
  using System;
  using UnityEngine;


  public class DamageMultiplier : HealthModifier {

    [field: SerializeField]
    float multiplier { get; set; } = 1;

    public override Handler onSub => OnSub;
    protected float OnSub(float current) {
      return current * multiplier;
    }
  }
}
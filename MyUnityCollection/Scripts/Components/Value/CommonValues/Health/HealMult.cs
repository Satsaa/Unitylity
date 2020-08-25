

namespace Muc.Components.Values {
  using System;
  using UnityEngine;


  public class HealMult : HealthModifier {

    [field: SerializeField]
    float multiplier { get; set; } = 1;

    public override Handler onAdd => OnAdd;
    protected float OnAdd(float current) {
      return current * multiplier;
    }
  }
}
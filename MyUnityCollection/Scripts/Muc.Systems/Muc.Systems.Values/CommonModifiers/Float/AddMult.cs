

namespace Muc.Systems.Values {
  using System;
  using UnityEngine;


  public class AddMult : Modifier<float> {

    [field: SerializeField]
    float multiplier { get; set; } = 1;

    public override Handler onAdd => OnAdd;
    protected float OnAdd(float current) {
      return current * multiplier;
    }
  }
}
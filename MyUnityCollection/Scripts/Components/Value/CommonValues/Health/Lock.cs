
using UnityEngine;
using Muc.Components.Values;
using System;

public class Lock : HealthModifier {

  public override Handler onSet => OnSet;
  protected float OnSet(float arg1) => target.GetRaw();

  public override Handler onAdd => OnArithmetic;
  public override Handler onSub => OnArithmetic;

  protected float OnArithmetic(float arg1) {
    Ignore();
    return 0;
  }

}
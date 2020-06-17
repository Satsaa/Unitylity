


using System.Collections.Generic;
using Muc.Inspector;
using UnityEngine;
using UnityEngine.Events;

namespace Muc.Components.Values {

  public class Stamina : Value<float, Stamina> {

    private const int DEFAULT_VALUE = 100;

    protected override float defaultValue => DEFAULT_VALUE;
    public float max = DEFAULT_VALUE;

    public UnityEvent<Stamina> onDeath;

    protected override float AddRawToValue(float addition) => value += addition;

    public override void AddToValue(float value) {
      var prevVal = this.value;
      base.AddToValue(value);
      if (this.value <= 0 && prevVal > 0) {
        onDeath.Invoke(this);
      }
    }
  }

  public abstract class StaminaModifier : Modifier<float, Stamina> { }
}
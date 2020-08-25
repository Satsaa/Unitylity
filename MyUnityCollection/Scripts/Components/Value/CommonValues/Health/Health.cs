

namespace Muc.Components.Values {

  using System;

  using UnityEngine;
  using UnityEngine.Events;


  public class Health : ArithmeticValue<float, Health> {

    public float max = 100;
    protected override float defaultValue => max;

    protected override float value {
      get => _value;
      set {
        _value = value;
        dead = _value <= 0;
      }
    }

    public bool dead {
      get => _dead;
      set {
        if (_dead != value) {
          _dead = value;
          if (_dead) {
            onDeath.Invoke(this);
          } else {
            onRevive.Invoke(this);
          }
        }
      }
    }
    [SerializeField]
    private bool _dead = false;

    public UnityEvent<Health> onDeath;
    public UnityEvent<Health> onRevive;

    protected override float AddValues(float a, float b) => a + b;
    protected override float SubtractValues(float a, float b) => a - b;

    public override float Add(float value) {
      var res = base.Add(value);
      dead = value <= 0;
      return res;
    }

    public override float Set(float value) {
      var res = base.Set(value);
      dead = value <= 0;
      return res;
    }
  }


  public abstract class HealthModifier : Modifier<float, Health> { }
}
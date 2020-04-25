

namespace ValueComponents {

  using UnityEngine.Events;


  public class Health : Value<float, Health> {

    private const int DEFAULT_VALUE = 100;

    protected override float defaultValue => DEFAULT_VALUE;
    public float max = DEFAULT_VALUE;

    public UnityEvent<Health> onDeath;

    protected override float AddRawToValue(float addition) => value += addition;

    public override void AddToValue(float value) {
      var prevVal = this.value;
      base.AddToValue(value);
      if (this.value <= 0 && prevVal > 0) {
        onDeath.Invoke(this);
      }
    }
  }

  public abstract class HealthModifier : Modifier<float, Health> { }
}
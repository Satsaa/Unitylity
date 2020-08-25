

namespace Muc.Components.Values {

  using UnityEngine.Events;


  public class Stamina : ArithmeticValue<float, Stamina> {

    public float max = 100;
    protected override float defaultValue => max;

    protected override float AddValues(float a, float b) => a + b;
    protected override float SubtractValues(float a, float b) => a - b;
  }

  public abstract class StaminaModifier : Modifier<float, Stamina> { }
}
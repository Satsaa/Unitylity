

namespace ValueComponents {

  using UnityEngine;


  public class DamageMultiplier : HealthModifier {

    [field: SerializeField]
    float multiplier { get; set; }

    public override float Modify(float current, Health value) {
      if (current < 0) return current * multiplier;
      return current;
    }
  }
}
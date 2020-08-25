

namespace Muc.Components.Values {

  using System;
  using System.Collections.Generic;

  using UnityEngine;
  using UnityEngine.Events;
  using Random = UnityEngine.Random;

  using Muc.Types.Extensions;

  public class Crits : HealthModifier {

    enum CritStacking {
      [Tooltip("The highest multiplier of the triggered crits is used")]
      Highest,
      [Tooltip("Multipliers of triggered crits are combined using addition")]
      Additive,
      [Tooltip("Multipliers of triggered crits are combined using multiplication")]
      Multiplicative,
    }

    [field: SerializeField]
    CritStacking critStacking { get; set; } = CritStacking.Additive;

    [field: SerializeField]
    List<Crit> crits { get; set; } = new List<Crit>();


    [Serializable]
    public class Crit {
      [field: SerializeField, Range(0, 1)]
      public float chance { get; set; }
      [field: SerializeField, Min(0)]
      public float multiplier { get; private set; }
    }

    public override Handler onSub => OnSub;
    protected float OnSub(float current) {
      if (crits.Count == 0) return current;

      float mult = 1;
      var rand = Random.value;

      switch (critStacking) {

        case CritStacking.Highest:
          foreach (var crit in crits) {
            if (mult < crit.multiplier) {
              if (rand <= crit.chance) {
                mult = Mathf.Max(mult, crit.multiplier);
              }
            }
          }
          break;

        case CritStacking.Additive:
          bool critted = false;
          foreach (var crit in crits) {
            if (rand <= crit.chance) {
              if (critted) {
                mult += crit.multiplier;
              } else {
                critted = true;
                mult = crit.multiplier;
              }
            }
          }
          if (!critted) mult = current * (mult - 1);
          break;

        case CritStacking.Multiplicative:
          foreach (var crit in crits) {
            if (rand <= crit.chance) {
              mult *= crit.multiplier;
            }
          }
          break;

      }

      return current * mult;
    }
  }
}
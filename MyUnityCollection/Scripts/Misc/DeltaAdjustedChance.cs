

using UnityEngine;

namespace MUC.Misc {

  public static partial class Random {

    // https://answers.unity.com/questions/1353041/deltatime-dependent-random-wander-math-problem.html

    /// <summary>
    /// Returns `chance` adjusted to deltaTime. 
    /// When called from Update or FixedUpdate,
    /// Random.value should be lower than this at `chance` probability  
    /// </summary>
    public static float GetDeltaAdjustedChance(float chance) => GetDeltaAdjustedChance(chance, Time.deltaTime);
    /// <summary>
    /// Returns `chance` adjusted to deltaTime. 
    /// When called from Update or FixedUpdate,
    /// Random.value should be lower than this at `chance` probability  
    /// </summary>
    public static float GetDeltaAdjustedChance(float chance, float deltaTime) {
      return 1 - Mathf.Pow(1 - chance, deltaTime);
    }

    /// <summary>
    /// Samples `chance` in such way that true is returned with `chance`
    /// probability each second if called from Update or FixedUpdate. 
    /// True can be returned multiple times a second  
    /// </summary>
    public static bool DeltaAdjustedChance(float chance) => DeltaAdjustedChance(chance, Time.deltaTime);
    /// <summary>
    /// Samples `chance` in such way that true is returned with `chance`
    /// probability each second if called from Update or FixedUpdate. 
    /// True can be returned multiple times a second  
    /// </summary>
    public static bool DeltaAdjustedChance(float chance, float deltaTime) {
      return UnityEngine.Random.value <= 1 - Mathf.Pow(1 - chance, deltaTime);
    }

  }
}

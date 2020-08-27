

namespace Muc.Components.Values {

  using System;
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;
  using System.Reflection;

  using UnityEngine;
  using UnityEngine.Events;

  using Muc.Types.Extensions;


  /// <summary>
  /// A value container which allows adding Modifiers which change the way get, set, add or sub (subtraction) operations are handled.
  /// </summary>
  /// <typeparam name="T">The type of the contained value</typeparam>
  public abstract class LimitedValue<T> : ArithmeticValue<T> {
    // !!! applies a limit which takes into account Get Modifiers
  }
}


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
  /// A value container which allows adding Modifiers which change the way add, set or get operations are handled.  
  /// Define This as the type you are currently declaring. E.G. `class MyCustomValue : Value&lt;float, MyCustomValue&gt; { ... }`  
  /// </summary>
  /// <typeparam name="T">The contained value type</typeparam>
  /// <typeparam name="This">The type of this class</typeparam>
  public abstract class ArithmeticValue<T, This> : Value<T, This> where This : ArithmeticValue<T, This> {


    /// <summary> Standard addition operation </summary>
    protected abstract T AddValues(T a, T b);

    /// <summary> Standard subtraction operation </summary>
    protected abstract T SubtractValues(T a, T b);


    /// <summary> Adds addition, after it is modified, to the value. It is not recommended to use this function inside Modifiers! </summary>
    public virtual T Add(T addition) {
      foreach (var handler in addHandlers) {
        addition = handler(addition);
        if (HadPostHandlerActions()) {
          if (WasSkipped()) break;
        }
      }
      if (HadOnCompleteActions()) {
        DoOnCompletes();
        if (WasIgnored()) return value;
      }
      return value = AddValues(value, addition);
    }

    /// <summary> Subtracts subtraction, after it is modified, from the value. It is not recommended to use this function inside Modifiers! </summary>
    public virtual T Sub(T subtraction) {
      foreach (var handler in subHandlers) {
        subtraction = handler(subtraction);
        if (HadPostHandlerActions()) {
          if (WasSkipped()) break;
        }
      }
      if (HadOnCompleteActions()) {
        DoOnCompletes();
        if (WasIgnored()) return value;
      }
      return value = SubtractValues(value, subtraction);
    }

  }
}


namespace Muc.Systems.Values {

  using System;
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;
  using System.Reflection;

  using UnityEngine;
  using UnityEngine.Events;

  using Muc.Extensions;

  public abstract class Modifier<T> {

    public readonly Type valueType = typeof(T);

    public virtual bool enabled {
      get => _enabled;
      set {
        if (value == _enabled) return;
        _enabled = value;
        target.RefreshUsedHandlerLists(this);
      }
    }
    [SerializeField]
    protected bool _enabled = true;

    /// <summary> Current Value executing a modifier function. </summary>
    protected internal Value<T> target;

    /// <summary>
    /// Called when this Modifier is being added to a Value.  
    /// </summary>
    /// <param name="value">The target Value</param>
    /// <returns>True if this Modifier can be added to value</returns>
    public virtual bool CanBeAdded(Value<T> value) => true;

    /// <summary>
    /// Called when this Modifier is being added to a Value.  
    /// Return false if this Modifier is not valid for removal.
    /// </summary>
    /// <param name="value">The target Value</param>
    /// <returns>True if this Modifier can be removed from value</returns>
    public virtual bool CanBeRemoved(Value<T> value) => true;


    /// <summary>
    /// Called when this Modifier is added to a Value.
    /// </summary>
    public virtual void OnModifierAdd(Value<T> value) { }

    /// <summary>
    /// Called when this Modifier is removed from a Value.
    /// </summary>
    public virtual void OnModifierRemove(Value<T> value) { }

    public delegate T Handler(T current);

    public virtual Handler onGet => null;
    public virtual Handler onSet => null;

    public virtual Handler onAdd => null;
    public virtual Handler onSub => null;

    /// <summary> Executes action after the execution of Handlers finishes, even if cancelled. </summary>
    protected void OnComplete(Action action) => target.OnComplete(action);
    /// <summary> Ignores the modified value after completion. </summary>
    protected void Ignore() => target.Ignore();

    /// <summary> Skips the rest of the Handlers. </summary>
    protected void Skip() => target.Skip();

  }
}
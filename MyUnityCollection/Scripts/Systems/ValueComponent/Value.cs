

namespace ValueComponents {

  using System;
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;
  using System.Reflection;

  using UnityEngine;
  using UnityEngine.Events;

  using Muc.Types.Extensions;


  public abstract class Modifier<T, TValue> where TValue : Value<T, TValue> {
    public bool enabled = true;
    public abstract T Modify(T current, TValue value);
  }

  public interface ICustomModifierDrawer {
    void Draw();
  }

  /// <summary>
  /// The type parameter `This` should be type you are declaring.  
  /// E.G. class `MyHealth : Value&lt;float, MyHealth&gt; { ... }` 
  /// </summary>
  public abstract class Value<T, This> : MonoBehaviour,
                                         IReadOnlyList<Modifier<T, This>>,
                                         IReadOnlyCollection<Modifier<T, This>>,
                                         IEnumerable<Modifier<T, This>>,
                                         ICollection<Modifier<T, This>>,
                                         IList<Modifier<T, This>>
                                         where This : Value<T, This> {


    [SerializeField] protected ValueData vd;

    protected virtual T defaultValue { get; }
    [SerializeField] protected T _value;
    public virtual T value {
      get => _value;
      protected set {
        if (_value.Equals(value)) {
          var old = _value;
          _value = value;
          onChange.Invoke(old, value);
        }
      }
    }

    // Old value, new value
    public UnityEvent<T, T> onChange;


    private readonly List<Modifier<T, This>> modifiers = new List<Modifier<T, This>>();


    protected virtual void Reset() {
      // Add ValueData from other Value Components
      _value = defaultValue;
      if (!vd) {
        foreach (var mono in FindObjectsOfType<MonoBehaviour>()) {
          if (mono == this) continue;
          if (mono.GetType().IsGenericTypeOf(typeof(Value<,>))) {
            var type = mono.GetType();
            var field = type.GetField(nameof(vd), BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) continue;
            var val = field.GetValue(mono);
            vd = val as ValueData;
            // Continue if vd is still null
            if (vd) break;
          }
        }
      }
    }

    public virtual void AddToValue(T value) {
      var res = value;
      foreach (var modifier in modifiers) {
        res = modifier.Modify(res, (This)this);
      }
      this._value = AddRawToValue(res);
    }

    protected abstract T AddRawToValue(T addition);

    protected virtual void AddModifier<TModifier>() where TModifier : Modifier<T, This>, new()
      => AddModifier(new TModifier());

    protected virtual void AddModifier(Modifier<T, This> modifier) {
      var types = vd.GetModifiers<This>();
      var priority = types.IndexOf(modifier.GetType());
      if (priority == -1) {
        modifiers.Add(modifier);
        Debug.LogWarning($"{modifier.GetType().FullName} was not found in the modifier type list. It was added at the end of the list.");
        return;
      }

      for (int i = 0; i < modifiers.Count; i++) {
        var otherModifier = modifiers[i];
        var otherPriority = types.IndexOf(otherModifier.GetType());
        if (otherPriority < priority) {
          modifiers.Insert(i, modifier);
          return;
        }
      }
      modifiers.Add(modifier);
    }

    protected virtual void OnRemoveModifier(Modifier<T, This> modifier) {

    }


    #region Interfaces implementation

    // Props
    public int Count => modifiers.Count;
    public bool IsReadOnly => ((ICollection<Modifier<T, This>>)modifiers).IsReadOnly;

    // Accessor
    public Modifier<T, This> this[int index] {
      get => modifiers[index];
      set {
        OnRemoveModifier(modifiers[index]);
        AddModifier(value);
      }
    }

    // Enumerate
    public IEnumerator<Modifier<T, This>> GetEnumerator() => modifiers.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => modifiers.GetEnumerator();

    // Misc
    public void CopyTo(Modifier<T, This>[] array, int arrayIndex) => modifiers.CopyTo(array, arrayIndex);

    // Contents
    public bool Contains(Modifier<T, This> modifier) => modifiers.Contains(modifier);
    public int IndexOf(Modifier<T, This> modifier) => modifiers.IndexOf(modifier);


    // Add
    public void Add(Modifier<T, This> modifier) => AddModifier(modifier);
    public void Insert(int index, Modifier<T, This> modifier) => AddModifier(modifier);

    // Remove
    public bool Remove(Modifier<T, This> modifier) {
      OnRemoveModifier(modifier);
      return modifiers.Remove(modifier);
    }
    public void RemoveAt(int index) {
      var modifier = modifiers[index];
      OnRemoveModifier(modifier);
      modifiers.RemoveAt(index);
    }
    public void Clear() {
      while (modifiers.Count > 0) {
        OnRemoveModifier(modifiers.Last());
        modifiers.RemoveAt(modifiers.Count - 1);
      }
    }


    #endregion
  }
}
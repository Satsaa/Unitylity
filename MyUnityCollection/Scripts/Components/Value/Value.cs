

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
  public abstract class Value<T, This> : MonoBehaviour,
                                         ISerializationCallbackReceiver,
                                         IReadOnlyCollection<Modifier<T, This>>,
                                         IEnumerable<Modifier<T, This>>
                                         where This : Value<T, This> {


    [SerializeField]
    protected ValueData valueData;

    protected virtual T defaultValue { get; }
    protected virtual T value { get; set; }
    [SerializeField]
    protected T _value;

    [field: SerializeReference]
    protected List<object> modifiers = new List<object>() { };

    protected List<Modifier<T, This>.Handler> getHandlers = null;
    protected List<Modifier<T, This>.Handler> setHandlers = null;
    protected List<Modifier<T, This>.Handler> addHandlers = null;
    protected List<Modifier<T, This>.Handler> subHandlers = null;



    protected virtual void Reset() {
      _value = defaultValue;
      // Automatically add ValueData from other Value Components
      if (!valueData) {
        foreach (var mono in FindObjectsOfType<MonoBehaviour>()) {
          if (mono == this) continue;
          if (mono.GetType().IsGenericTypeOf(typeof(Value<,>))) {
            var type = mono.GetType();
            var field = type.GetField(nameof(valueData), BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) continue;
            var val = field.GetValue(mono);
            valueData = val as ValueData;
            if (valueData) break;
          }
        }
      }
    }

    protected void Start() {
      // This will populate handler arrays that are null
      RefreshHandlerLists(false, false, false, false);
    }


    public T GetRaw() => value;

    /// <summary> Gets the value, after modifications. It is not recommended to use this function inside Modifiers! </summary>
    public virtual T Get() {
      var result = value;
      foreach (var handler in getHandlers) {
        result = handler(result);
        if (HadPostHandlerActions()) {
          if (WasSkipped()) break;
        }
      }
      if (HadOnCompleteActions()) {
        DoOnCompletes();
        if (WasIgnored()) return value;
      }
      return result;
    }

    /// <summary> Sets newValue, after it is modified, as the value. It is not recommended to use this function inside Modifiers! </summary>
    public virtual T Set(T newValue) {
      foreach (var handler in setHandlers) {
        newValue = handler(newValue);
        if (HadPostHandlerActions()) {
          if (WasSkipped()) break;
        }
      }
      if (HadOnCompleteActions()) {
        DoOnCompletes();
        if (WasIgnored()) return value;
      }
      return value = newValue;
    }


    public virtual bool AddModifier<TModifier>() where TModifier : Modifier<T, This>, new()
      => AddModifier(new TModifier());

    public virtual bool AddModifier(Modifier<T, This> modifier) {

      if (modifier.target) throw new AlreadyAssignedException();
      var thisVal = (This)this;
      if (!modifier.CanBeAdded(thisVal)) return false;
      modifier.target = thisVal;
      try {
        modifier.OnModifierAdd(thisVal);
      } catch {
        Debug.LogError($"Adding of {nameof(Modifier<T, This>)} {modifier.GetType().FullName} was cancelled because an error was thrown during {nameof(modifier.OnModifierAdd)}.");
        modifier.target = null;
        throw;
      }

      var types = valueData.GetModifiers<This>();
      var priority = types.IndexOf(modifier.GetType());

      if (priority == -1) {
        Debug.LogWarning($"No priority value was found for {modifier.GetType().FullName}. Added at the end of the Modifier list.");
        modifiers.Add(modifier);
        goto added;
      }

      for (int i = 0; i < modifiers.Count; i++) {
        var other = modifiers[i];
        var otherPrio = types.IndexOf(other.GetType());
        if (otherPrio < priority) {
          modifiers.Insert(i, modifier);
          goto added;
        }
      }
      modifiers.Add(modifier);
    added:

      RefreshUsedHandlerLists(modifier);
      return true;
    }

    internal virtual bool RemoveModifier(Modifier<T, This> modifier) {
      var thisVal = (This)this;
      if (!modifier.CanBeRemoved(thisVal)) return false;
      modifiers.Remove(modifier);
      modifier.OnModifierRemove(thisVal);
      RefreshUsedHandlerLists(modifier);
      modifier.target = null;
      return true;
    }


    public virtual void RefreshHandlerLists(bool set = true, bool get = true, bool add = true, bool sub = true) {
      if (getHandlers == null) { get = true; getHandlers = new List<Modifier<T, This>.Handler>(); }
      if (setHandlers == null) { set = true; setHandlers = new List<Modifier<T, This>.Handler>(); }
      if (addHandlers == null) { add = true; addHandlers = new List<Modifier<T, This>.Handler>(); }
      if (subHandlers == null) { sub = true; subHandlers = new List<Modifier<T, This>.Handler>(); }
      if (get) getHandlers.Clear();
      if (set) setHandlers.Clear();
      if (add) addHandlers.Clear();
      if (sub) subHandlers.Clear();
      foreach (var mod in this) {
        if (get && mod.enabled && mod.onGet != null) getHandlers.Add(mod.onGet);
        if (set && mod.enabled && mod.onSet != null) setHandlers.Add(mod.onSet);
        if (add && mod.enabled && mod.onAdd != null) addHandlers.Add(mod.onAdd);
        if (sub && mod.enabled && mod.onSub != null) subHandlers.Add(mod.onSub);
      }
    }

    public virtual void RefreshUsedHandlerLists(Modifier<T, This> modifier) {
      var doGet = modifier.onGet != null && modifier.enabled;
      var doSet = modifier.onSet != null && modifier.enabled;
      var doAdd = modifier.onAdd != null && modifier.enabled;
      var doSub = modifier.onSub != null && modifier.enabled;
      RefreshHandlerLists(doSet, doGet, doAdd, doSub);
    }



    #region OnCompleteActions

    private readonly OnCompleteActions ocAct = new OnCompleteActions();
    private class OnCompleteActions {
      internal bool required;
      internal bool ignore;
      internal List<Action> onComplete = new List<Action>();
    }

    protected bool HadOnCompleteActions() => ocAct.required != (ocAct.required = false);

    protected bool WasIgnored() => ocAct.ignore != (ocAct.ignore = false);
    internal void Ignore() {
      ocAct.required = true;
      ocAct.ignore = true;
    }

    internal void OnComplete(Action action) {
      ocAct.required = true;
      ocAct.onComplete.Add(action);
    }

    protected void DoOnCompletes() {
      foreach (var action in ocAct.onComplete) action();
      ocAct.onComplete.Clear();
    }

    #endregion


    #region PostHandlerActions

    private readonly PostHandlerActions phAct = new PostHandlerActions();
    private class PostHandlerActions {
      internal bool required;
      internal bool skip;
    }

    protected bool HadPostHandlerActions() => phAct.required != (phAct.required = false);


    protected bool WasSkipped() => phAct.skip != (phAct.skip = false);
    internal void Skip() {
      phAct.required = true;
      phAct.skip = true;
    }

    #endregion



    #region Interfaces implementation

    public int Count => modifiers.Count;

    public IEnumerator<Modifier<T, This>> GetEnumerator() {
      foreach (var modifier in modifiers) {
        yield return (Modifier<T, This>)modifier;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => modifiers.GetEnumerator();

    void ISerializationCallbackReceiver.OnBeforeSerialize() {
    }
    void ISerializationCallbackReceiver.OnAfterDeserialize() {
      modifiers.RemoveAll(m => m is null);
      RefreshHandlerLists();
    }

    #endregion
  }
}
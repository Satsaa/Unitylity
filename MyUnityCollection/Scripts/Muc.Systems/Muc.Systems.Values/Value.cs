

namespace Muc.Systems.Values {

  using System;
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;
  using System.Reflection;

  using UnityEngine;
  using UnityEngine.Events;

  using Muc.Extensions;
  using Muc.Editor;


  /// <summary>
  /// A value container which allows adding Modifiers which change the way set or get operations are handled.
  /// </summary>
  /// <typeparam name="T">The type of the contained value</typeparam>
  [ExecuteAlways]
  public abstract class Value<T> : MonoBehaviour, ISerializationCallbackReceiver {

    public readonly Type valueType = typeof(T);

    public ValueSettings valueSettings { get => _valueSettings; internal set => _valueSettings = value; }
    [SerializeField] ValueSettings _valueSettings;

    [field: SerializeField, HideInInspector]
    public string orderGuid { get; protected internal set; } = Guid.NewGuid().ToString("N");

    protected virtual T defaultValue { get; }
    protected virtual T value { get => _value; set => _value = value; }
    [SerializeField] protected T _value;

    protected virtual List<object> defaultModifiers => new List<object>();
    [SerializeReference]
    protected List<object> modifiers;

    protected List<Modifier<T>.Handler> getHandlers = null;
    protected List<Modifier<T>.Handler> setHandlers = null;
    protected List<Modifier<T>.Handler> addHandlers = null;
    protected List<Modifier<T>.Handler> subHandlers = null;


    protected virtual void Reset() {
      value = defaultValue;
      modifiers = defaultModifiers;
      if (valueSettings is null) valueSettings = ValueSettings.GetDefaultInstance();
    }

#if UNITY_EDITOR
    private void Update() => RefreshOrdersIfGuid();
#endif

    protected void Start() {
      if (Application.IsPlaying(gameObject)) RefreshOrdersIfGuid();
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
      value = newValue;
      return value;
    }


    public virtual bool AddModifier<TModifier>() where TModifier : Modifier<T>, new()
      => AddModifier(new TModifier());

    public virtual bool AddModifier(Modifier<T> modifier) {

      if (modifier.target) return false;
      if (!CanBeAdded(modifier)) return false;
      if (!modifier.CanBeAdded(this)) return false;
      modifier.target = this;
      try {
        modifier.OnModifierAdd(this);
      } catch {
        Debug.LogError($"Adding of {nameof(Modifier<T>)} {modifier.GetType().FullName} was cancelled because an error was thrown during {nameof(modifier.OnModifierAdd)}.");
        modifier.target = null;
        throw;
      }

      var types = valueSettings.GetModifiers<T>();
      var priority = types.IndexOf(modifier.GetType());

      if (priority == -1) {
        Debug.LogWarning($"No priority value was found for {modifier.GetType().FullName}. Added at the start of the Modifier list.");
        modifiers.Insert(0, modifier);
        goto added;
      }

      for (int i = 0; i < modifiers.Count; i++) {
        var other = modifiers[i];
        var otherPrio = types.IndexOf(other.GetType());
        if (otherPrio > priority) {
          modifiers.Insert(i, modifier);
          goto added;
        }
      }
      modifiers.Add(modifier);
    added:

      RefreshUsedHandlerLists(modifier);
      return true;
    }

    internal virtual bool RemoveModifier(Modifier<T> modifier) {
      if (!CanBeRemoved(modifier)) return false;
      if (!modifier.CanBeRemoved(this)) return false;
      modifiers.Remove(modifier);
      modifier.OnModifierRemove(this);
      RefreshUsedHandlerLists(modifier);
      modifier.target = null;
      return true;
    }

    public virtual bool CanBeAdded(Modifier<T> modifier) => true;
    public virtual bool CanBeRemoved(Modifier<T> modifier) => true;


    protected internal virtual void RefreshHandlerLists(bool set = true, bool get = true, bool add = true, bool sub = true) {
      if (getHandlers == null) { get = true; getHandlers = new List<Modifier<T>.Handler>(); }
      if (setHandlers == null) { set = true; setHandlers = new List<Modifier<T>.Handler>(); }
      if (addHandlers == null) { add = true; addHandlers = new List<Modifier<T>.Handler>(); }
      if (subHandlers == null) { sub = true; subHandlers = new List<Modifier<T>.Handler>(); }
      if (get) getHandlers.Clear();
      if (set) setHandlers.Clear();
      if (add) addHandlers.Clear();
      if (sub) subHandlers.Clear();
      foreach (var mod in GetModifiers()) {
        if (get && mod.enabled && mod.onGet != null) getHandlers.Add(mod.onGet);
        if (set && mod.enabled && mod.onSet != null) setHandlers.Add(mod.onSet);
        if (add && mod.enabled && mod.onAdd != null) addHandlers.Add(mod.onAdd);
        if (sub && mod.enabled && mod.onSub != null) subHandlers.Add(mod.onSub);
      }
    }

    protected internal virtual void RefreshUsedHandlerLists(Modifier<T> modifier) {
      var doGet = modifier.onGet != null && modifier.enabled;
      var doSet = modifier.onSet != null && modifier.enabled;
      var doAdd = modifier.onAdd != null && modifier.enabled;
      var doSub = modifier.onSub != null && modifier.enabled;
      RefreshHandlerLists(doSet, doGet, doAdd, doSub);
    }

    internal void RefreshOrdersIfGuid() {
      if (!valueSettings || orderGuid == valueSettings.orderGuid) return;
      var types = valueSettings.GetModifiers<T>();

      modifiers.Sort((a, b) => {
        var aIndex = types.IndexOf(a.GetType());
        var bIndex = types.IndexOf(b.GetType());
        return aIndex.CompareTo(bIndex);
      });
      orderGuid = valueSettings.orderGuid;
    }

    public IEnumerable<Modifier<T>> GetModifiers() {
      foreach (var modifier in modifiers) {
        yield return (Modifier<T>)modifier;
      }
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

    void ISerializationCallbackReceiver.OnBeforeSerialize() { }
    void ISerializationCallbackReceiver.OnAfterDeserialize() {
      modifiers.RemoveAll(m => m is null);
      RefreshHandlerLists();
    }

    #endregion
  }
}

namespace Muc.Collections {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class PrioritizedModifier<T> {
    /// <summary>
    /// First parameter passed to function is the modified value and second is the original value. 
    /// Original value is a simple copy of the variable, so a reference type may not keep it's original state!
    /// </summary>
    public PrioritizedModifier(Func<T, T, T> function, float priority = 0) {
      this.function = function;
      this.priority = priority;
    }
    public Func<T, T, T> function;
    public readonly float priority = 0;
  }

  public class PrioritizedModifierList<T> : IEnumerable<PrioritizedModifier<T>>, IEnumerable {

    #region Collection methods
    public PrioritizedModifier<T> this[int index] { get => modifiers[index]; set => modifiers[index] = value; }

    public int Count { get => modifiers.Count; }
    public int Capacity { get => modifiers.Capacity; set => modifiers.Capacity = value; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<PrioritizedModifier<T>> GetEnumerator() => modifiers.GetEnumerator();

    public void Clear() => modifiers.Clear();
    public bool Contains(PrioritizedModifier<T> item) => modifiers.Contains(item);
    public bool Exists(Predicate<PrioritizedModifier<T>> match) => modifiers.Exists(match);
    public int FindIndex(int startIndex, int count, Predicate<PrioritizedModifier<T>> match) => FindIndex(startIndex, count, match);
    public int FindIndex(int startIndex, Predicate<PrioritizedModifier<T>> match) => FindIndex(startIndex, match);
    public int FindIndex(Predicate<PrioritizedModifier<T>> match) => FindIndex(match);
    public PrioritizedModifier<T> FindLast(Predicate<PrioritizedModifier<T>> match) => FindLast(match);
    public int FindLastIndex(int startIndex, int count, Predicate<PrioritizedModifier<T>> match) => FindLastIndex(startIndex, count, match);
    public int FindLastIndex(int startIndex, Predicate<PrioritizedModifier<T>> match) => FindLastIndex(startIndex, match);
    public int FindLastIndex(Predicate<PrioritizedModifier<T>> match) => FindLastIndex(match);
    public List<PrioritizedModifier<T>> GetRange(int index, int count) => modifiers.GetRange(index, count);
    public int IndexOf(PrioritizedModifier<T> item, int index, int count) => modifiers.IndexOf(item, index, count);
    public int IndexOf(PrioritizedModifier<T> item, int index) => modifiers.IndexOf(item, index);
    public int IndexOf(PrioritizedModifier<T> item) => modifiers.IndexOf(item);
    public int LastIndexOf(PrioritizedModifier<T> item) => modifiers.LastIndexOf(item);
    public int LastIndexOf(PrioritizedModifier<T> item, int index) => modifiers.LastIndexOf(item, index);
    public int LastIndexOf(PrioritizedModifier<T> item, int index, int count) => modifiers.LastIndexOf(item);
    public bool Remove(PrioritizedModifier<T> item) => modifiers.Remove(item);
    public int RemoveAll(Predicate<PrioritizedModifier<T>> match) => modifiers.RemoveAll(match);
    public void RemoveAt(int index) => modifiers.RemoveAt(index);
    public void RemoveRange(int index, int count) => modifiers.RemoveRange(index, count);
    public PrioritizedModifier<T>[] ToArray() => modifiers.ToArray();
    public void TrimExcess() => modifiers.TrimExcess();
    public bool TrueForAll(Predicate<PrioritizedModifier<T>> match) => modifiers.TrueForAll(match);
    public int BinarySearch(PrioritizedModifier<T> item) => modifiers.BinarySearch(item);
    public int BinarySearch(PrioritizedModifier<T> item, IComparer<PrioritizedModifier<T>> comparer) => modifiers.BinarySearch(item, comparer);
    public int BinarySearch(int index, int count, PrioritizedModifier<T> item, IComparer<PrioritizedModifier<T>> comparer) => modifiers.BinarySearch(index, count, item, comparer);
    #endregion


    public PrioritizedModifierList() {
      modifiers = new List<PrioritizedModifier<T>>();
    }
    public PrioritizedModifierList(IEnumerable<PrioritizedModifier<T>> collection) {
      modifiers = new List<PrioritizedModifier<T>>(collection);
    }

    protected List<PrioritizedModifier<T>> modifiers;

    public T Apply(T value) {
      var orig = value;
      foreach (var filter in modifiers) value = filter.function(value, orig);
      return value;
    }

    public void AddRange(IEnumerable<PrioritizedModifier<T>> filters) { foreach (var filter in filters) Add(filter); }

    /// <summary>
    /// First parameter passed to function is the modified value and second is the original value. 
    /// Original value is a simple copy of the variable, so a reference type may not keep it's original state!
    /// </summary>
    public void Add(Func<T, T, T> function, float priority = 0) => Add(new PrioritizedModifier<T>(function, priority));
    public void Add(PrioritizedModifier<T> filter) {
      for (int i = 0; i < modifiers.Count; i++) {
        var other = modifiers[i];
        if (other.priority <= filter.priority) {
          modifiers.Insert(i, filter);
          return;
        }
      }
      modifiers.Add(filter);
    }
  }

}
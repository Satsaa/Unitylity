using System.Collections.Generic;
using System.Collections;
using System;

namespace MUC.Collections {

  public class Modifier<T> {
    /// <summary>
    /// First parameter passed to function is the modified value and second is the original value. 
    /// Original value is a simple copy of the variable, so a reference type may not keep it's original state!
    /// </summary>
    public Modifier(Func<T, T, T> function, float priority = 0) {
      this.function = function;
      this.priority = priority;
    }
    public Func<T, T, T> function;
    public readonly float priority = 0;
  }

  public class ModifierList<T> : IEnumerable<Modifier<T>>, IEnumerable {

    #region Collection methods
    public Modifier<T> this[int index] { get => filters[index]; set => filters[index] = value; }

    public int Count { get => filters.Count; }
    public int Capacity { get => filters.Capacity; set => filters.Capacity = value; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<Modifier<T>> GetEnumerator() => filters.GetEnumerator();

    public void Clear() => filters.Clear();
    public bool Contains(Modifier<T> item) => filters.Contains(item);
    public bool Exists(Predicate<Modifier<T>> match) => filters.Exists(match);
    public int FindIndex(int startIndex, int count, Predicate<Modifier<T>> match) => FindIndex(startIndex, count, match);
    public int FindIndex(int startIndex, Predicate<Modifier<T>> match) => FindIndex(startIndex, match);
    public int FindIndex(Predicate<Modifier<T>> match) => FindIndex(match);
    public Modifier<T> FindLast(Predicate<Modifier<T>> match) => FindLast(match);
    public int FindLastIndex(int startIndex, int count, Predicate<Modifier<T>> match) => FindLastIndex(startIndex, count, match);
    public int FindLastIndex(int startIndex, Predicate<Modifier<T>> match) => FindLastIndex(startIndex, match);
    public int FindLastIndex(Predicate<Modifier<T>> match) => FindLastIndex(match);
    public List<Modifier<T>> GetRange(int index, int count) => filters.GetRange(index, count);
    public int IndexOf(Modifier<T> item, int index, int count) => filters.IndexOf(item, index, count);
    public int IndexOf(Modifier<T> item, int index) => filters.IndexOf(item, index);
    public int IndexOf(Modifier<T> item) => filters.IndexOf(item);
    public int LastIndexOf(Modifier<T> item) => filters.LastIndexOf(item);
    public int LastIndexOf(Modifier<T> item, int index) => filters.LastIndexOf(item, index);
    public int LastIndexOf(Modifier<T> item, int index, int count) => filters.LastIndexOf(item);
    public bool Remove(Modifier<T> item) => filters.Remove(item);
    public int RemoveAll(Predicate<Modifier<T>> match) => filters.RemoveAll(match);
    public void RemoveAt(int index) => filters.RemoveAt(index);
    public void RemoveRange(int index, int count) => filters.RemoveRange(index, count);
    public Modifier<T>[] ToArray() => filters.ToArray();
    public void TrimExcess() => filters.TrimExcess();
    public bool TrueForAll(Predicate<Modifier<T>> match) => filters.TrueForAll(match);
    public int BinarySearch(Modifier<T> item) => filters.BinarySearch(item);
    public int BinarySearch(Modifier<T> item, IComparer<Modifier<T>> comparer) => filters.BinarySearch(item, comparer);
    public int BinarySearch(int index, int count, Modifier<T> item, IComparer<Modifier<T>> comparer) => filters.BinarySearch(index, count, item, comparer);
    #endregion


    protected List<Modifier<T>> filters = new List<Modifier<T>>();

    public T Apply(T value) {
      var orig = value;
      foreach (var filter in filters) value = filter.function(value, orig);
      return value;
    }

    public void AddRange(IEnumerable<Modifier<T>> filters) { foreach (var filter in filters) Add(filter); }

    /// <summary>
    /// First parameter passed to function is the modified value and second is the original value. 
    /// Original value is a simple copy of the variable, so a reference type may not keep it's original state!
    /// </summary>
    public void Add(Func<T, T, T> function, float priority = 0) => Add(new Modifier<T>(function, priority));
    public void Add(Modifier<T> filter) {
      for (int i = 0; i < filters.Count; i++) {
        var other = filters[i];
        if (other.priority <= filter.priority) {
          filters.Insert(i, filter);
          return;
        }
      }
      filters.Add(filter);
    }
  }

}
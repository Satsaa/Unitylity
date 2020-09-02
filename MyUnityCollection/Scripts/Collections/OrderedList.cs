

namespace Muc.Collections {

  using System;
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;

  /// <summary>
  /// List in which new items are sorted based on a provided Comparison function
  /// </summary>
  public class OrderedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList {


    #region Implementation

    protected readonly List<T> items;

    protected readonly IComparer<T> comparer;


    public T this[int index] {
      get => items[index];
      set => ReplaceAt(index, value);
    }


    #region Ctor

    protected OrderedList(IComparer<T> comparer, List<T> items) {
      if (items == null) throw new NullReferenceException("The items cannot be null!");
      this.items = items;
      if (comparer == null) throw new NullReferenceException("The comparison provider cannot be null!");
      this.comparer = comparer;
    }

    // IComparer overloads
    public OrderedList(IComparer<T> comparer) : this(comparer, new List<T>()) { }
    public OrderedList(IEnumerable<T> collection, IComparer<T> comparer) : this(comparer, new List<T>(collection)) { }
    public OrderedList(int capacity, IComparer<T> comparer) : this(comparer, new List<T>(capacity)) { }

    // Comparison overloads
    public OrderedList(Comparison<T> comparison) : this(new Comparer(comparison)) { }
    public OrderedList(IEnumerable<T> collection, Comparison<T> comparison) : this(collection, new Comparer(comparison)) { }
    public OrderedList(int capacity, Comparison<T> comparison) : this(capacity, new Comparer(comparison)) { }

    #endregion


    private class Comparer : IComparer<T> {
      private readonly Comparison<T> comparison;
      public Comparer(Comparison<T> comparison) => this.comparison = comparison;
      public int Compare(T x, T y) => comparison(x, y);
    }

    public void Add(T item) {

      int i = 0;
      if (items.Count != 0) {
        while (comparer.Compare(item, items[i]) >= 0 && ++i < items.Count) ;
      }

      items.Insert(i, item);
    }

    /// <summary> Not optimized </summary>
    public void AddRange(IEnumerable<T> collection) {
      foreach (var item in collection)
        Add(item);
    }


    protected void ReplaceAt(int index, T item) {

      int i = 0;
      while (comparer.Compare(item, items[i]) >= 0 && ++i < items.Count) ;

      if (i == index || i == index + 1) {
        items[index] = item;
        return;
      }

      if (i < index) {
        items.RemoveAt(index);
        items[i] = item;
      } else {
        items.Insert(i, item);
        items.RemoveAt(index);
      }
    }


    public void ReSort() => items.Sort(comparer);

    #endregion



    #region Explicit interface implementations

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

    bool ICollection<T>.IsReadOnly => ((ICollection<T>)items).IsReadOnly;
    void ICollection.CopyTo(Array array, int index) => ((ICollection)items).CopyTo(array, index);
    bool ICollection.IsSynchronized => ((ICollection)items).IsSynchronized;
    object ICollection.SyncRoot => ((ICollection)items).SyncRoot;

    bool IList.IsReadOnly => ((IList)items).IsReadOnly;
    bool IList.IsFixedSize => ((IList)items).IsFixedSize;
    object IList.this[int index] { get => ((IList)items)[index]; set => ((IList)items)[index] = value; }
    void IList<T>.Insert(int index, T item) => ((IList<T>)items).Insert(index, item);
    int IList.Add(object value) => ((IList)items).Add(value);
    bool IList.Contains(object value) => ((IList)items).Contains(value);
    int IList.IndexOf(object value) => ((IList)items).IndexOf(value);
    void IList.Insert(int index, object value) => ((IList)items).Insert(index, value);
    void IList.Remove(object value) => ((IList)items).Remove(value);

    #endregion



    #region List like members

    public int Count => items.Count;

    public OrderedList<T> Sort(Comparison<T> comparison) => new OrderedList<T>(items, comparison);
    public OrderedList<T> Sort(int index, int count, IComparer<T> comparer) => new OrderedList<T>(comparer, items.GetRange(index, count));
    public OrderedList<T> Sort(IComparer<T> comparer) => new OrderedList<T>(comparer, items);

    public ReadOnlyCollection<T> AsReadOnly() => items.AsReadOnly();
    public int BinarySearch(T item) => items.BinarySearch(item);
    public void Clear() => items.Clear();
    public bool Contains(T item) => items.Contains(item);
    public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => items.ConvertAll<TOutput>(converter);
    public void CopyTo(int index, T[] array, int arrayIndex, int count) => items.CopyTo(index, array, arrayIndex, count);
    public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);
    public void CopyTo(T[] array) => items.CopyTo(array);
    public bool Exists(Predicate<T> match) => items.Exists(match);
    public T Find(Predicate<T> match) => items.Find(match);
    public List<T> FindAll(Predicate<T> match) => items.FindAll(match);
    public int FindIndex(int startIndex, int count, Predicate<T> match) => items.FindIndex(startIndex, count, match);
    public int FindIndex(int startIndex, Predicate<T> match) => items.FindIndex(startIndex, match);
    public int FindIndex(Predicate<T> match) => items.FindIndex(match);
    public T FindLast(Predicate<T> match) => items.FindLast(match);
    public int FindLastIndex(int startIndex, int count, Predicate<T> match) => items.FindLastIndex(startIndex, count, match);
    public int FindLastIndex(int startIndex, Predicate<T> match) => items.FindLastIndex(startIndex, match);
    public int FindLastIndex(Predicate<T> match) => items.FindLastIndex(match);
    public void ForEach(Action<T> action) => items.ForEach(action);
    public List<T>.Enumerator GetEnumerator() => items.GetEnumerator();
    public List<T> GetRange(int index, int count) => items.GetRange(index, count);
    public int IndexOf(T item, int index, int count) => items.IndexOf(item, index, count);
    public int IndexOf(T item, int index) => items.IndexOf(item, index);
    public int IndexOf(T item) => items.IndexOf(item);

    public int LastIndexOf(T item) => items.LastIndexOf(item);
    public int LastIndexOf(T item, int index) => items.LastIndexOf(item, index);
    public int LastIndexOf(T item, int index, int count) => items.LastIndexOf(item, index, count);
    public bool Remove(T item) => items.Remove(item);
    public int RemoveAll(Predicate<T> match) => items.RemoveAll(match);
    public void RemoveAt(int index) => items.RemoveAt(index);
    public void RemoveRange(int index, int count) => items.RemoveRange(index, count);

    public T[] ToArray() => items.ToArray();
    public List<T> ToList() => items.ToList();
    public void TrimExcess() => items.TrimExcess();
    public bool TrueForAll(Predicate<T> match) => items.TrueForAll(match);

    #endregion

  }
}
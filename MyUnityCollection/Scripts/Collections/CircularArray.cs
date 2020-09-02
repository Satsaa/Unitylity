

namespace Muc.Collections {

  using System;
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;

  using Muc.Math;

  /// <summary>
  /// Represents a first-in, first-out fixed size collection of items. 
  /// </summary>
  public class CircularArray<T> : IEnumerable<T>, IEnumerable, ICloneable, IReadOnlyCollection<T>, IReadOnlyList<T> {

    public T this[int index] {
      get => data[head - index];
      set => data[head - index] = value;
    }

    public int Length { get => data.Length; set => Resize(value); }

    private T[] data;
    private CircularInt head;

    public CircularArray(IEnumerable<T> collection) {
      data = collection.ToArray();
      head = new CircularInt(0, data.Length);
    }

    public CircularArray(int length) {
      data = new T[length];
      head = new CircularInt(0, length);
    }

    public void Add(T item) {
      data[++head] = item;
    }

    /// <summary> Set all items to the default value of the Type of the items </summary>
    public void Clear() {
      for (int i = 0; i < data.Length; i++) {
        data[i] = default(T);
      }
    }

    /// <summary> Resizes the buffer </summary>
    public void Resize(int length) {
      var old = data;
      data = this.ToArray();
      Array.Resize(ref data, length);
      data.Reverse();
      head = new CircularInt(length - 1, length);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator() {
      for (int i = 0; i < head.ceiling; i++) {
        yield return data[head - i];
      }
    }

    public new string ToString() => string.Join(", ", this.ToArray());

    public object Clone() {
      return this.ToArray();
    }

    int IReadOnlyCollection<T>.Count => ((IReadOnlyCollection<T>)data).Count;

  }

}
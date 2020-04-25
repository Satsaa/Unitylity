using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

using Muc.Types;

namespace Muc.Collections {

  /// <summary>
  /// Represents a first-in, first-out fixed size list of items. 
  /// </summary>
  public class CircularBuffer<T> : IEnumerable<T>, IEnumerable {

    public T this[int index] {
      get => data[head - index];
      set => data[head - index] = value;
    }

    public int Length { get => data.Length; set => Resize(value); }

    private T[] data;
    private CircularInt head;

    public CircularBuffer(int length) {
      data = new T[length];
      head = new CircularInt(0, length);

      var a = new List<int>();
      var b = a.AsReadOnly();
    }

    public void Add(T item) {
      data[++head] = item;
    }

    /// <summary> Set all elements to the default value of element type </summary>
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
      for (int i = 0; i < head.ceil; i++) {
        yield return data[head - i];
      }
    }

    public new string ToString() => string.Join(", ", this.ToArray());

  }

}
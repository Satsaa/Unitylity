using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

using MUC.Types;

namespace MUC.Collections {

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
      // 56734  // Content

      data = this.ToArray();
      // 76543
      // Element order is now new -> old

      Array.Resize(ref data, length);
      // 7654300000 // length 10
      // 76         // length 2

      data.Reverse();
      // 0000034567

      head = new CircularInt(length - 1, length);
      // 0000034567
      // ^write   ^read
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator() {
      for (int i = 0; i < head.ceil; i++) {
        yield return data[head - i];
      }
    }

    public ReadOnlyCollection<T> AsReadOnly() => new ReadOnlyCollection<T>(this.ToArray());

    public new string ToString() => string.Join(", ", this.ToArray());

  }

}
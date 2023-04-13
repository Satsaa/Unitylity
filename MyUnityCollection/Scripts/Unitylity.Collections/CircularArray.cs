
namespace Unitylity.Collections {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Unitylity.Numerics;
	using UnityEngine;

	/// <summary>
	/// Represents a first-in, first-out fixed size collection of items.
	/// </summary>
	[Serializable]
	public class CircularArray<T> : IEnumerable<T>, IEnumerable, ICloneable, IReadOnlyCollection<T>, IReadOnlyList<T> {

		public T this[int index] {
			get => data[head - index];
			set => data[head - index] = value;
		}

		public int Length { get => data.Length; set => Resize(value); }

		[SerializeField] private T[] data;
		[SerializeField] private CircularInt head;

		public CircularArray(IEnumerable<T> collection) {
			data = collection.ToArray();
			head = new CircularInt(data.Length - 1, data.Length);
		}

		public CircularArray(int length) {
			data = new T[length];
			head = new CircularInt(length - 1, length);
		}

		public void Add(T item) {
			data[++head] = item;
		}

		/// <summary> Set all items to the default value of the Type of the items </summary>
		public void Clear() {
			for (int i = 0; i < data.Length; i++) {
				data[i] = default;
			}
		}

		/// <summary> Resizes the array </summary>
		public void Resize(int length) {
			if (length == Length) return;

			var array = this.ToArray();
			Array.Resize(ref array, length);
			Array.Reverse(array);
			head = new CircularInt(length - 1, length);
			data = array;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<T> GetEnumerator() {
			for (int i = 0; i < Length; i++) {
				yield return this[i];
			}
		}

		public int IndexOf(T item) {
			for (int i = 0; i < Length; i++)
				if (this[i].Equals(item)) return i;
			return -1;
		}

		public override string ToString() => string.Join(", ", this);

		public void CopyTo(T[] array, int index) {
			if (array.Length < Length) throw new ArgumentException("Destination array was not long enough. Check destIndex and length, and the array's lower bounds");
			if (index >= array.Length || index < 0) throw new ArgumentOutOfRangeException("Index must be positive and less than the array length.");

			var circIndex = new CircularInt(index, array.Length);
			for (int i = 0; i < Length; i++) {
				array[circIndex + i] = this[i];
			}

		}
		object ICloneable.Clone() => Clone();
		public CircularArray<T> Clone() => new(this);

		public bool Contains(T item) => data.Contains(item);

		int IReadOnlyCollection<T>.Count => data.Length;

	}

}
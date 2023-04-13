
namespace Unitylity.Collections {

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using UnityEngine;

	[Serializable]
	public class SerializedStack<T> : IEnumerable<T>, IReadOnlyCollection<T>, System.Collections.ICollection {

		[SerializeField] List<T> list;
		private int version;

		public SerializedStack() {
			list = new List<T>();
		}

		public SerializedStack(int capacity) {
			list = new List<T>(capacity);
		}

		public SerializedStack(IEnumerable<T> collection) {
			list = new List<T>(collection);
		}

		public int Count => list.Count;


		public Enumerator GetEnumerator() => new(this);
		IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => new Enumerator(this);

		public void Clear() => list.Clear();
		public bool Contains(T item) => list.Contains(item);
		public T Peek() => list.Last();

		public T Pop() {
			var res = list.Last();
			list.RemoveAt(Count - 1);
			version++;
			return res;
		}

		public void Push(T item) {
			list.Add(item);
			version++;
		}


		public T[] ToArray() {
			var res = new T[Count];
			for (int i = 0; i < Count; i++) {
				res[Count - i - 1] = list[i];
			}
			return res;
		}


		private object _syncRoot;

		bool System.Collections.ICollection.IsSynchronized => false;

		object System.Collections.ICollection.SyncRoot {
			get {
				if (_syncRoot == null) System.Threading.Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
				return _syncRoot;
			}
		}

		public void CopyTo(T[] array, int arrayIndex) {
			if (array == null) throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException("Offset must be positive.", nameof(arrayIndex));
			if (array.Length - arrayIndex < Count) throw new ArgumentException("Invalid offset.", nameof(arrayIndex));

			Array.Copy(list.ToArray(), 0, array, arrayIndex, Count);
			Array.Reverse(array, arrayIndex, Count);
		}

		void System.Collections.ICollection.CopyTo(Array array, int arrayIndex) {
			if (array == null) throw new ArgumentNullException(nameof(array));
			if (array.Rank != 1) throw new ArgumentException("Arrays must be of the same rank.", nameof(array));
			if (array.GetLowerBound(0) != 0) throw new ArgumentException("Non-zero lowerbound.", nameof(array));
			if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException("Offset must be positive.", nameof(arrayIndex));
			if (array.Length - arrayIndex < Count) throw new ArgumentException("Invalid offset.", nameof(arrayIndex));

			try {
				Array.Copy(list.ToArray(), 0, array, arrayIndex, Count);
				Array.Reverse(array, arrayIndex, Count);
			} catch (ArrayTypeMismatchException) {
				throw new ArgumentException("Invalid array type.");
			}
		}

		public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator {

			private SerializedStack<T> stack;
			private int index;
			private int version;
			private T current;

			internal Enumerator(SerializedStack<T> stack) {
				this.stack = stack;
				version = this.stack.version;
				index = -2;
				current = default;
			}

			public void Dispose() {
				index = -1;
			}

			public bool MoveNext() {
				bool res;
				if (version != stack.version) throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
				if (index == -2) {
					index = stack.Count - 1;
					res = (index >= 0);
					if (res)
						current = stack.list[index];
					return res;
				} else if (index == -1) {
					return false;
				}

				res = (--index >= 0);
				if (res)
					current = stack.list[index];
				else
					current = default;
				return res;
			}

			object System.Collections.IEnumerator.Current => Current;
			public T Current {
				get {
					if (index == -2) throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
					if (index == -1) throw new InvalidOperationException("The enumeration has already completed.");
					return current;
				}
			}

			void System.Collections.IEnumerator.Reset() {
				if (version != stack.version) throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
				index = -2;
				current = default;
			}
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Collections.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;

	[CustomPropertyDrawer(typeof(SerializedStack<>), true)]
	public class SerializedStackDrawer : PropertyDrawer {

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var list = property.FindPropertyRelative("list");
			return EditorGUI.GetPropertyHeight(list, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			using (PropertyScope(position, label, property, out label)) {
				var list = property.FindPropertyRelative("list");
				EditorGUI.PropertyField(position, list, label);
			}
		}

	}

}
#endif

namespace Unitylity.Collections {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	// !!! Maybe this can be optimized by copying the list only if it is going to change while enumeration is active?

	/// <summary>
	/// Like a normal List but enumeration doesn't throw if the collection is changed, and the enumerated objects don't change.
	/// </summary>
	[Serializable]
	public class SafeList<T> : ICollection<T>, IEnumerable<T>, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, IEnumerable, ICollection, IList {

		public SafeList() => list = new List<T>();
		public SafeList(IEnumerable<T> collection) => list = new List<T>(collection);
		public SafeList(int capacity) => list = new List<T>(capacity);

		[SerializeField]
		private List<T> list;
		private List<T> enumerationTarget;

		public T this[int index] { get => list[index]; set { list[index] = value; enumerationTarget = null; } }

		public int Count => list.Count;

		bool ICollection<T>.IsReadOnly => ((ICollection<T>)list).IsReadOnly;
		bool ICollection.IsSynchronized => ((ICollection)list).IsSynchronized;
		object ICollection.SyncRoot => ((ICollection)list).SyncRoot;
		bool IList.IsFixedSize => ((IList)list).IsFixedSize;
		bool IList.IsReadOnly => ((IList)list).IsReadOnly;
		object IList.this[int index] { get => list[index]; set { list[index] = (T)value; enumerationTarget = null; } }

		private void UpdateIfRequired() {
			if (enumerationTarget == null || enumerationTarget.Count != this.Count) {
				enumerationTarget = list.ToList();
			}
		}

		public void Add(T item) {
			list.Add(item);
			enumerationTarget = null;
		}

		public bool Remove(T item) {
			var res = list.Remove(item);
			if (res) enumerationTarget = null;
			return res;
		}

		public void RemoveAt(int index) {
			list.RemoveAt(index);
			enumerationTarget = null;
		}

		public void Clear() {
			list.Clear();
			enumerationTarget = null;
		}

		public int IndexOf(T item) => list.IndexOf(item);
		public void Insert(int index, T item) => list.Insert(index, item);

		public bool Contains(T item) => list.Contains(item);
		public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<T> GetEnumerator() {
			UpdateIfRequired();
			return enumerationTarget.GetEnumerator();
		}

		void ICollection.CopyTo(Array array, int index) {
			((ICollection)list).CopyTo(array, index);
		}

		int IList.Add(object value) {
			Add((T)value);
			return list.Count - 1;
		}

		bool IList.Contains(object value) {
			if (value is null) return typeof(T).IsByRef && Contains(default);
			if (value is T tv) return Contains(tv);
			return false;
		}

		int IList.IndexOf(object value) {
			return ((IList)list).IndexOf(value);
		}

		void IList.Insert(int index, object value) {
			((IList)list).Insert(index, value);
			enumerationTarget = null;
		}

		void IList.Remove(object value) {
			((IList)list).Remove(value);
			enumerationTarget = null;
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
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomPropertyDrawer(typeof(SafeList<>), true)]
	public class SafeListDrawer : PropertyDrawer {

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var prop = property.FindPropertyRelative("list");
			return EditorGUI.GetPropertyHeight(prop, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			using (PropertyScope(position, label, property, out label)) {
				var prop = property.FindPropertyRelative("list");
				EditorGUI.PropertyField(position, prop, new GUIContent(label));
			}
		}

	}

}
#endif
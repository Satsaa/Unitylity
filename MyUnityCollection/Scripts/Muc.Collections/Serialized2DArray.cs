
namespace Unitylity.Collections {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	[Serializable]
	public class Serialized2DArray<T> : IList<T> {

		public int ConvertIndex(Vector2Int pos) => (pos.x >= size.x || pos.y >= size.y) ? throw new System.IndexOutOfRangeException() : pos.y * size.x + pos.x;
		public Vector2Int ConvertIndex(int index) => (index >= data.Length) ? throw new System.IndexOutOfRangeException() : new(index / size.x, index % size.x);

		[SerializeField]
		private T[] data;

		[field: SerializeField]
		public Vector2Int size { get; private set; }

		private Serialized2DArray() { }

		public Serialized2DArray(in int width, in int height) {
			this.size = new(width, height);
			data = new T[width * height];
		}

		public Serialized2DArray(in int size) {
			this.size = new(size, size);
			data = new T[size * size];
		}

		public Serialized2DArray(in Vector2Int size) {
			this.size = size;
			data = new T[size.x * size.y];
		}

		public Serialized2DArray(in Vector2Int size, in IList<T> source) {
			this.size = size;
			data = new T[size.x * size.y];
			if (size.x * size.y > source.Count) throw new ArgumentException($"{nameof(source)} contains an invalid amount of items.", nameof(source));
			source.CopyTo(data, 0);
		}

		public T this[int x, int y] {
			get => (x >= size.x || y >= size.y) ? throw new System.IndexOutOfRangeException() : data[y * size.x + x];
			set => data[y * size.x + x] = (x >= size.x || y >= size.y) ? throw new System.IndexOutOfRangeException() : value;
		}

		public T this[Vector2Int pos] {
			get => (pos.x >= size.x || pos.y >= size.y) ? throw new System.IndexOutOfRangeException() : data[pos.y * size.x + pos.x];
			set => data[pos.y * size.x + pos.x] = (pos.x >= size.x || pos.y >= size.y) ? throw new System.IndexOutOfRangeException() : value;
		}

		public Vector2Int IndexOf(T item) => ConvertIndex(((IList<T>)data).IndexOf(item));

		#region Interfaces

		T IList<T>.this[int index] { get => data[index]; set => data[index] = value; }

		public bool Contains(T item) => data.Contains(item);
		public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)data).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
		int ICollection<T>.Count => data.Length;
		bool ICollection<T>.IsReadOnly => data.IsReadOnly;
		int IList<T>.IndexOf(T item) => ((IList<T>)data).IndexOf(item);
		void ICollection<T>.CopyTo(T[] array, int arrayIndex) => data.CopyTo(array, arrayIndex);

		void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
		void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
		void ICollection<T>.Add(T item) => throw new NotSupportedException();
		void ICollection<T>.Clear() => throw new NotSupportedException();
		bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

		#endregion

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

	[CustomPropertyDrawer(typeof(Serialized2DArray<>), true)]
	public class Serialized2DArrayDrawer : PropertyDrawer {

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var data = property.FindPropertyRelative("data");
			var dataHeight = 0f;
			if (property.isExpanded) {
				var size = property.FindPropertyRelative(GetBackingFieldName("size"));
				if (size.hasMultipleDifferentValues) {
					dataHeight += lineHeight + spacing;
				} else {
					for (int i = 0; i < data.arraySize; i++) {
						var element = data.GetArrayElementAtIndex(i);
						dataHeight += EditorGUI.GetPropertyHeight(element) + spacing;
					}
				}
			}
			return lineHeight + dataHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			using (PropertyScope(position, label, property, out label)) {
				var size = property.FindPropertyRelative(GetBackingFieldName("size"));
				var sizeVal = size.vector2IntValue;
				var data = property.FindPropertyRelative("data");

				var foldoutPosition = position;
				foldoutPosition.height = lineHeight;

				var sizePosition = foldoutPosition;

				foldoutPosition.width -= Mathf.Min(120 + spacing, position.width - labelWidth);

				sizePosition.xMin = foldoutPosition.xMax + spacing;
				sizePosition.xMax -= 1;

				Foldout(foldoutPosition, property);
				EditorGUI.BeginChangeCheck();
				PropertyField(sizePosition, GUIContent.none, size);
				if (EditorGUI.EndChangeCheck()) {
					if (!size.hasMultipleDifferentValues)
						data.arraySize = sizeVal.x * sizeVal.y;
				}

				if (property.isExpanded) {
					using (LabelWidthScope(v => v - indentSize))
					using (IndentScope()) {
						if (size.hasMultipleDifferentValues) {
							var helpPos = position;
							helpPos.xMin += indentSize;
							helpPos.yMin += lineHeight + spacing;
							helpPos.height = lineHeight;
							HelpBoxField(helpPos, "Cannot edit elements of multiple 2D arrays of different size", MessageType.Warning);
						} else {
							var elementPosition = position;
							elementPosition.xMin += indentSize;
							elementPosition.height = lineHeight;
							for (int i = 0; i < data.arraySize; i++) {
								var element = data.GetArrayElementAtIndex(i);
								elementPosition.y += elementPosition.height + spacing;
								elementPosition.height = EditorGUI.GetPropertyHeight(element);
								PropertyField(elementPosition, new($"{i / sizeVal.x}, {i % sizeVal.x}"), element, true);
							}
							data.arraySize = sizeVal.x * sizeVal.y;
						}
					}
				}
			}
		}

	}

}
#endif
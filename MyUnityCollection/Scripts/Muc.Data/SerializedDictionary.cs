

// License: MIT
// Original Author: Erik Eriksson (2020)
// Original Code: https://github.com/upscalebaby/generic-serializable-dictionary

namespace Muc.Data {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class SerializedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, ISerializationCallbackReceiver {

		public SerializedDictionary() { }
		public SerializedDictionary(IDictionary<TKey, TValue> dictionary) { foreach (var kv in dictionary) { Add(kv.Key, kv.Value); } }
		public SerializedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) { foreach (var kv in collection) { Add(kv.Key, kv.Value); } }

		[SerializeField]
		List<SerializedDictionaryListPair<TKey, TValue>> list = new();

		Dictionary<TKey, int> indexes = new();
		Dictionary<TKey, TValue> dict = new();

		public void OnBeforeSerialize() { }

		public void OnAfterDeserialize() {
			dict.Clear();
			indexes.Clear();

			for (int i = 0; i < list.Count; i++) {
				var kv = list[i];
				if (kv.key == null || ContainsKey(kv.key)) {
					if (!kv.isDuplicate) {
						var item = list[i];
						item.isDuplicate = true;
						list[i] = item;
					}
				} else {
					if (kv.isDuplicate) {
						var item = list[i];
						item.isDuplicate = false;
						list[i] = item;
					}
					dict.Add(kv.key, kv.value);
					indexes.Add(kv.key, i);
				}
			}
		}

		public TValue this[TKey key] {
			get => dict[key];
			set {
				dict[key] = value;
				if (indexes.TryGetValue(key, out var index)) {
					list[index] = new(key, value, false);
				} else {
					list.Add(new(key, value, false));
					indexes.Add(key, list.Count - 1);
				}
			}
		}

		// IDictionary
		public ICollection<TKey> Keys => dict.Keys;
		public ICollection<TValue> Values => dict.Values;

		public void Add(TKey key, TValue value) {
			dict.Add(key, value);
			list.Add(new(key, value, false));
			indexes.Add(key, list.Count - 1);
		}

		public bool ContainsKey(TKey key) => dict.ContainsKey(key);

		public bool Remove(TKey key) {
			if (dict.Remove(key)) {
				var index = indexes[key];
				list.RemoveAt(index);
				indexes.Remove(key);
				for (int i = index; i < list.Count; i++) {
					indexes[list[i].key]--;
				}
				return true;
			} else {
				return false;
			}
		}

		public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);

		// ICollection
		public int Count => dict.Count;
		public bool IsReadOnly { get; set; }

		public void Add(KeyValuePair<TKey, TValue> item) {
			Add(item.Key, item.Value);
		}

		public void Clear() {
			dict.Clear();
			list.Clear();
			indexes.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item) {
			if (dict.TryGetValue(item.Key, out var value)) {
				return EqualityComparer<TValue>.Default.Equals(value, item.Value);
			} else {
				return false;
			}
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
			if (array == null)
				throw new ArgumentException("The array cannot be null.");
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
			if (array.Length - arrayIndex < dict.Count)
				throw new ArgumentException("The destination array has fewer elements than the collection.");

			foreach (var pair in dict) {
				array[arrayIndex] = pair;
				arrayIndex++;
			}
		}

		public bool Remove(KeyValuePair<TKey, TValue> item) {
			if (dict.TryGetValue(item.Key, out var value)) {
				bool valueMatch = EqualityComparer<TValue>.Default.Equals(value, item.Value);
				if (valueMatch) {
					return Remove(item.Key);
				}
			}
			return false;
		}

		// IEnumerable
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => dict.GetEnumerator();

		// IReadOnlyDictionary
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)dict).Keys;
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)dict).Values;

	}
}


#if UNITY_EDITOR
namespace Muc.Data.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Muc.Editor.EditorUtil;
	using static Muc.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CustomPropertyDrawer(typeof(SerializedDictionary<,>), true)]
	public class SerializedDictionaryDrawer : PropertyDrawer {

		public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
			// Draw list.
			var list = property.FindPropertyRelative("list");
			string fieldName = ObjectNames.NicifyVariableName(fieldInfo.Name);
			var currentPos = new Rect(lineHeight, pos.y, pos.width, lineHeight);
			EditorGUI.PropertyField(currentPos, list, new GUIContent(fieldName), true);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			float totHeight = 0f;

			// Height of KeyValue list.
			var listProp = property.FindPropertyRelative("list");
			totHeight += EditorGUI.GetPropertyHeight(listProp, true);

			return totHeight;
		}

	}
}
#endif


namespace Muc.Data {

	using System;
	using UnityEngine;

	[Serializable]
	internal struct SerializedDictionaryListPair<TKey, TValue> {

		public TKey key;
		public TValue value;
		public bool isDuplicate;

		internal SerializedDictionaryListPair(TKey key, TValue value, bool isDuplicate) {
			this.key = key;
			this.value = value;
			this.isDuplicate = isDuplicate;
		}

		public override string ToString() {
			return $"[{key},{value}]";
		}

	}

}

#if UNITY_EDITOR
namespace Muc.Data {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Muc.Editor;
	using UnityEditor;
	using UnityEngine;
	using static Muc.Editor.EditorUtil;
	using static Muc.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomPropertyDrawer(typeof(SerializedDictionaryListPair<,>), true)]
	public class SerializedDictionaryListPairDrawer : PropertyDrawer {

		private static readonly GUIContent keyContent = new("K", "Key");
		private static readonly GUIContent invalidKeyContent = new("K", "Key (Invalid)");
		private static readonly GUIContent valueContent = new("V", "Value");

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var value = property.FindPropertyRelative(nameof(SerializedDictionaryListPair<string, string>.value));
			return Mathf.Max(lineHeight, EditorGUI.GetPropertyHeight(value, label));
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			var propPos = position;
			propPos.xMin = 0;

			using (PropertyScope(propPos, label, property, out label)) {
				var key = property.FindPropertyRelative(nameof(SerializedDictionaryListPair<string, string>.key));
				var value = property.FindPropertyRelative(nameof(SerializedDictionaryListPair<string, string>.value));
				var isDuplicate = property.FindPropertyRelative(nameof(SerializedDictionaryListPair<string, string>.isDuplicate));

				position.xMin -= 5 + spacing;

				var keyRect = position;
				keyRect.width /= 2;
				keyRect.height = lineHeight;

				var valueRect = keyRect;
				valueRect.x += keyRect.width;
				valueRect.height = position.height;

				keyRect.width -= 2;
				valueRect.width += 2;

				if (isDuplicate.boolValue || isDuplicate.hasMultipleDifferentValues) {
					using (EditorUtil.BackgroundColorScope(v => Color.Lerp(v, Color.red, 0.75f))) {
						using (LabelWidthScope(10)) EditorGUI.PropertyField(keyRect, key, invalidKeyContent);
					}
				} else {
					using (LabelWidthScope(10)) EditorGUI.PropertyField(keyRect, key, keyContent);
				}
				using (LabelWidthScope(10)) EditorGUI.PropertyField(valueRect, value, valueContent);
			}
		}

	}
}
#endif
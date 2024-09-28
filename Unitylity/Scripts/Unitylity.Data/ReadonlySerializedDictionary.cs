
// License: MIT
// Original Author: Erik Eriksson (2020)
// Original Code: https://github.com/upscalebaby/generic-serializable-dictionary

namespace Unitylity.Data {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class ReadonlySerializedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, ISerializationCallbackReceiver {

		public ReadonlySerializedDictionary() { }
		public ReadonlySerializedDictionary(IDictionary<TKey, TValue> dictionary) { foreach (var kv in dictionary) { Add(kv.Key, kv.Value); } }
		public ReadonlySerializedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) { foreach (var kv in collection) { Add(kv.Key, kv.Value); } }

		[SerializeField]
		internal List<ReadonlySerializedDictionaryListPair<TKey, TValue>> list = new();

		internal Dictionary<TKey, int> indexes = new();
		internal Dictionary<TKey, TValue> dict = new();

		void ISerializationCallbackReceiver.OnBeforeSerialize() { }

		void ISerializationCallbackReceiver.OnAfterDeserialize() {
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

		public TValue this[TKey key] => dict[key];

		// IDictionary
		public ICollection<TKey> Keys => dict.Keys;
		public ICollection<TValue> Values => dict.Values;

		protected virtual void Add(TKey key, TValue value) {
			dict.Add(key, value);
			list.Add(new(key, value, false));
			indexes.Add(key, list.Count - 1);
		}

		public bool ContainsKey(TKey key) => dict.ContainsKey(key);

		public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);

		// ICollection
		public int Count => dict.Count;
		public virtual bool IsReadOnly => true;

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

		// IEnumerable
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => dict.GetEnumerator();

		// IReadOnlyDictionary
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)dict).Keys;
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)dict).Values;

	}


	[Serializable]
	internal struct ReadonlySerializedDictionaryListPair<TKey, TValue> {

		public TKey key;
		public TValue value;
		public bool isDuplicate;

		internal ReadonlySerializedDictionaryListPair(TKey key, TValue value, bool isDuplicate) {
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
namespace Unitylity.Data.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CustomPropertyDrawer(typeof(ReadonlySerializedDictionary<,>), true)]
	public class ReadonlySerializedDictionaryDrawer : PropertyDrawer {

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


#if UNITY_EDITOR
namespace Unitylity.Data.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Unitylity.Editor;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomPropertyDrawer(typeof(ReadonlySerializedDictionaryListPair<,>), true)]
	public class ReadonlySerializedDictionaryListPairDrawer : PropertyDrawer {

		private static readonly GUIContent keyContent = new("K", "Key");
		private static readonly GUIContent invalidKeyContent = new("K", "Key (Invalid)");
		private static readonly GUIContent valueContent = new("V", "Value");

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var value = property.FindPropertyRelative(nameof(ReadonlySerializedDictionaryListPair<string, string>.value));
			return Mathf.Max(lineHeight, EditorGUI.GetPropertyHeight(value, label));
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			var propPos = position;
			propPos.xMin = 0;

			using (PropertyScope(propPos, label, property, out label)) {
				var key = property.FindPropertyRelative(nameof(ReadonlySerializedDictionaryListPair<string, string>.key));
				var value = property.FindPropertyRelative(nameof(ReadonlySerializedDictionaryListPair<string, string>.value));
				var isDuplicate = property.FindPropertyRelative(nameof(ReadonlySerializedDictionaryListPair<string, string>.isDuplicate));

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
					using (BackgroundColorScope(v => Color.Lerp(v, Color.red, 0.75f))) {
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
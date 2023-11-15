
// License: MIT
// Original Author: Erik Eriksson (2020)
// Original Code: https://github.com/upscalebaby/generic-serializable-dictionary

namespace Unitylity.Data {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class SerializedDictionary<TKey, TValue> : ReadonlySerializedDictionary<TKey, TValue>, IDictionary<TKey, TValue> {

		public SerializedDictionary() : base() { }
		public SerializedDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
		public SerializedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection) { }

		new public TValue this[TKey key] {
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
		new public void Add(TKey key, TValue value) {
			base.Add(key, value);
		}

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

		// ICollection
		public override bool IsReadOnly => false;

		public void Add(KeyValuePair<TKey, TValue> item) {
			Add(item.Key, item.Value);
		}

		public void Clear() {
			dict.Clear();
			list.Clear();
			indexes.Clear();
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

	}

}

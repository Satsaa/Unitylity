
namespace Unitylity.Systems.Lang {

	using System;
	using System.Collections;

	using System.Collections.Generic;
	using System.Linq;
	using Articy.Unity.Utils;

	using UnityEngine;
	using Unitylity.Data;
	using Object = UnityEngine.Object;

	public partial class LangText : TMPro.TextMeshProUGUI, ISerializationCallbackReceiver {

		[Serializable]
		public struct Param {

			enum ParamType {
				Bool,
				Float,
				Int,
				String,
			}

			public Param(bool boolVal) { paramType = ParamType.Bool; this.boolVal = boolVal; stringVal = default; intVal = default; floatVal = default; }
			public Param(float floatVal) { paramType = ParamType.Float; this.floatVal = floatVal; boolVal = default; stringVal = default; intVal = default; }
			public Param(int intVal) { paramType = ParamType.Int; this.intVal = intVal; floatVal = default; boolVal = default; stringVal = default; }
			public Param(string stringVal) { paramType = ParamType.String; this.stringVal = stringVal; intVal = default; floatVal = default; boolVal = default; }

			[SerializeField] ParamType paramType;

			[SerializeField] bool boolVal;
			[SerializeField] float floatVal;
			[SerializeField] int intVal;
			[SerializeField] string stringVal;

			public readonly object value => paramType switch {
				ParamType.Bool => boolVal,
				ParamType.Float => floatVal,
				ParamType.Int => intVal,
				ParamType.String => stringVal,
				_ => null,
			};
		}

		private struct Converter : IReadOnlyDictionary<string, object> {

			private IReadOnlyDictionary<string, Param> dict;

			public Converter(IReadOnlyDictionary<string, Param> dict) => this.dict = dict;

			public readonly object this[string key] => dict[key].value;
			public readonly IEnumerable<string> Keys => dict.Keys;
			public readonly IEnumerable<object> Values => dict.Values.Select(v => v.value);
			public readonly int Count => dict.Count;

			public readonly IEnumerator<KeyValuePair<string, object>> GetEnumerator() => throw new NotImplementedException();
			readonly IEnumerator IEnumerable.GetEnumerator() => dict.GetEnumerator();

			public readonly bool ContainsKey(string key) => dict.ContainsKey(key);
			public readonly bool TryGetValue(string key, out object value) {
				var res = dict.TryGetValue(key, out var _value);
				value = res ? _value.value : default;
				return res;
			}

		}

	}

}

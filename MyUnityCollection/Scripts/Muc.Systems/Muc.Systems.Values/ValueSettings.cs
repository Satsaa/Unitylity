

namespace Muc.Systems.Values {

	using System;
	using System.Linq;
	using System.Collections;
	using System.Collections.Generic;

	using UnityEngine;


	[CreateAssetMenu(fileName = "ValueSettings", menuName = "MUC/ValueSettings", order = 1)]
	public class ValueSettings : ScriptableObject {

		/// <summary>
		/// <para> `Key`: Generic type </para>
		/// <para> `Value`: Modifier types for the generic type </para>
		/// </summary>
		readonly Dictionary<Type, List<Type>> typeDict = new();

		[System.Serializable]
		public class OrderData {
			public string generic;
			public List<string> modifiers = new();

			public OrderData(string generic) => this.generic = generic;
		}

		public List<OrderData> orders = new();

		[field: SerializeField]
		public string orderGuid { get; protected internal set; } = Guid.NewGuid().ToString("N");


		internal static ValueSettings GetDefaultInstance() {
#if UNITY_EDITOR
			return Muc.Editor.MucProjectSettings.instance.defaultValueSettingsInstance;
#else
      		return null;
#endif
		}


		public List<Type> GetModifiers<T>() => GetModifiers(typeof(T));
		public List<Type> GetModifiers(Type type) {
			if (typeDict.TryGetValue(type, out var modifiers))
				return modifiers;
			return typeDict[type] = new List<Type>();
		}



		private void Reset() => Validate();
		private void Awake() => Validate();
		private void OnValidate() => Validate();

		public void Validate() {
			RefreshTypeDict();
			AddMissingNames();
			RemoveUnknownNames();
			DeDuplicateNames();
			SyncDictionary();
		}

		public OrderData GetDataByFullName(string tValue) {
			var res = orders.Find(v => v.generic == tValue);
			if (res == null) {
				orders.Add(new OrderData(tValue));
				return orders.Last();
			}
			return res;
		}

		private void AddMissingNames() {
			foreach (var kv in typeDict) {
				var order = GetDataByFullName(kv.Key.FullName);
				foreach (var modifiers in kv.Value) {
					var name = modifiers.FullName;
					if (!order.modifiers.Contains(name)) {
						order.modifiers.Insert(0, name);
					}
				}
			}
		}

		private void RemoveUnknownNames() {
			for (int i = 0; i < orders.Count; i++) {
				var order = orders[i];
				if (!typeDict.Keys.Any(k => k.FullName == order.generic)) {
					orders.RemoveAt(i--);
				} else {
					for (int j = 0; j < order.modifiers.Count; j++) {
						var modifierName = order.modifiers[j];

						var key = typeDict.Keys.Single(k => k.FullName == order.generic);
						var modifierTypes = typeDict[key];

						if (!modifierTypes.Any(v => v.FullName == modifierName)) {
							order.modifiers.RemoveAt(j--);
						}
					}
				}
			}
		}

		private void DeDuplicateNames() {
			var seenGens = new List<string>();
			var deleteGens = new List<OrderData>();

			foreach (var orderData in orders) {
				if (seenGens.Contains(orderData.generic)) {
					deleteGens.Add(orderData);
					continue;
				}
				seenGens.Add(orderData.generic);

				var seenMods = new List<string>();
				var deleteMods = new List<string>();

				foreach (var modifier in orderData.modifiers) {
					if (seenMods.Contains(modifier)) {
						deleteMods.Add(modifier);
						continue;
					}
					seenMods.Add(modifier);
				}

				foreach (var deleta in deleteMods) {
					orderData.modifiers.RemoveAt(orderData.modifiers.LastIndexOf(deleta));
				}
			}

			foreach (var delet in deleteGens) {
				orders.RemoveAt(orders.LastIndexOf(delet));
			}
		}

		private void SyncDictionary() {
			foreach (var kvp in typeDict) {
				var mods = kvp.Value;
				var valueName = kvp.Key.FullName;
				var modNames = orders.Find(v => v.generic == valueName).modifiers;

				mods.Sort((a, b) => modNames.IndexOf(a.FullName).CompareTo(modNames.IndexOf(b.FullName)));
			}
		}

		private void RefreshTypeDict() {

			foreach (var duo in GetModifierTypes()) {
				var type = duo.Item1;
				var generic = duo.Item2.GenericTypeArguments[0];

				if (this.typeDict.TryGetValue(generic, out var modifiers)) {
					if (!modifiers.Contains(type)) modifiers.Add(type);
				} else {
					this.typeDict[generic] = new List<Type>() { type };
				}
			}
		}

		private static IEnumerable<(Type, Type)> GetModifierTypes() {
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				var name = assembly.GetName().Name;
				var firstDot = name.IndexOf('.');
				var rootName = firstDot == -1 ? name : name.Substring(0, firstDot);
				switch (rootName) {
					case "System": continue;
					case "UnityEngine": continue;
					case "UnityEditor": continue;
					case "mscorlib": continue;
					case "Unity": continue;
					case "Mono": continue;
					default:
						if (
							name.StartsWith("com.unity") ||
							name == "nunit.framework" ||
							name == "ICSharpCode.NRefactory"
						) {
							continue;
						}
						break;
				}
				// Debug.Log($"{name} + ({assembly.GetTypes().Count()})");
				var types = assembly.GetTypes();
				foreach (var type in types) {
					if (TryGetModifierBaseType(type, out var modifierBase)) {
						yield return (type, modifierBase);
					}
				}
			}
		}

		private static bool TryGetModifierBaseType(Type type, out Type modifierBase) {
			modifierBase = null;
			if (!type.IsClass || type.IsAbstract) return false;

			while (type?.IsClass == true) {
				type = type.BaseType;
				if (type == null) return false;
				if (
					type.IsAbstract &&
					type.IsGenericType &&
					type.GenericTypeArguments.Length == 1 && (
						type.GetGenericTypeDefinition() == typeof(Modifier<>)
					)
				) {
					modifierBase = type;
					return true;
				}
			}
			return false;
		}
	}
}


#if UNITY_EDITOR
namespace Muc.Systems.Values.Editor {

	using System.Reflection;
	using System.Collections;
	using System.Collections.Generic;

	using UnityEngine;
	using UnityEditor;
	using UnityEditorInternal;


	[CustomEditor(typeof(ValueSettings))]
	public class ValueSettingsEditor : Editor {

		private readonly Dictionary<ValueSettings.OrderData, CacheData> cache = new();

		private bool showOrders = true;

		private ValueSettings t => target as ValueSettings;

		private class CacheData {

			private readonly ValueSettings target;
			private readonly ReorderableList list;
			private readonly string title;
			private bool show = true;


			public CacheData(IList elements, string title, ValueSettings target) {
				this.title = title;
				this.target = target;
				list = new ReorderableList(elements, typeof(ValueSettings.OrderData)) {
					displayAdd = false,
					displayRemove = false
				};
				// drawer.headerHeight = 1;

				list.onChangedCallback += OnChange;
				list.drawHeaderCallback += OnHeader;
				list.elementHeightCallback += OnElementHeight;
				list.drawElementCallback = null;
				list.drawElementBackgroundCallback = null;
				list.onReorderCallbackWithDetails = OnReorder;
			}

			private float OnElementHeight(int index) => list.elementHeight;

			private void OnHeader(Rect rect) {
				var wasExpanded = show;

				using (new EditorGUI.IndentLevelScope(1))
					show = EditorGUI.Foldout(rect, show, title, true);

				if (show != wasExpanded) {
					if (show) {
						list.elementHeight = EditorGUIUtility.singleLineHeight;
						list.showDefaultBackground = list.draggable = true;
						list.drawElementCallback = null;
						list.drawElementBackgroundCallback = null;
					} else {
						list.elementHeight = 0;
						list.showDefaultBackground = list.draggable = false;
						list.drawElementCallback = (rect1, index1, isActive1, isFocused1) => { };
						list.drawElementBackgroundCallback = (rect2, index2, isActive2, isFocused2) => { };
					}
				}
			}

			private void OnChange(ReorderableList list) {
				target.orderGuid = System.Guid.NewGuid().ToString("N");
			}

			private void OnReorder(ReorderableList list, int oldIndex, int newIndex) {
				// Restore old state for undo
				var moved = list.list[newIndex];
				list.list.RemoveAt(newIndex);
				list.list.Insert(oldIndex, moved);

				EditorUtility.SetDirty(target);
				Undo.RegisterCompleteObjectUndo(target, "Modified list");

				// Restore new state 
				list.list.RemoveAt(oldIndex);
				list.list.Insert(newIndex, moved);

				target.Validate();
			}

			internal void Draw() {
				list.DoLayoutList();
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			if (showOrders = EditorGUILayout.Foldout(showOrders, "Modifier Execution Orders", true)) {

				foreach (var orderData in t.orders) {

					if (!this.cache.TryGetValue(orderData, out var cache)) {
						cache = new CacheData(t.GetDataByFullName(orderData.generic).modifiers, orderData.generic, t);
						this.cache.Add(orderData, cache);
					}

					cache.Draw();

				}

			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif


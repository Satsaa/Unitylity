
namespace Unitylity.Systems.Menus {

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Unitylity.Components.Extended;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using Object = UnityEngine.Object;

#if (Unitylity_HIDE_COMPONENTS || Unitylity_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Menus) + "/" + nameof(Menus))]
#endif
	public class Menus : Singleton<Menus> {

		new public static Transform transform => instance.gameObject.transform;

		[SerializeField, HideInInspector]
		protected List<Menu> _menus;
		public ReadOnlyCollection<Menu> menus => _menus.AsReadOnly();

		[SerializeField, HideInInspector]
		protected List<Menu> _cached;
		public ReadOnlyCollection<Menu> cached => _cached.AsReadOnly();


		/// <summary>
		/// Removes the top-most instance of target menu.
		/// Due to reusage of Menus this may result in unexpected behaviour.
		/// Use the Hide(index) for removal of a specific instance of a Menu.
		/// </summary>
		public static bool Hide(Menu target) => instance._Hide(target);

		/// <summary> Removes the Menu at index. </summary>
		public static void Hide(int index) => instance._Hide(index);

		/// <summary> Removes the last/newest Menu. </summary>
		public static void Pop() => instance._Hide(instance._menus.Count - 1);

		/// <summary> Shows a menu representing the source Menu and optionally runs an initializer on it before it is activated. </summary>
		public static Menu Show(Menu source, Action<Menu> initializer = null) => instance._Show(source, initializer);


		public void TryClose() {
			if (_menus.Count > 0 && _menus.Last().allowCloseKey) {
				Pop();
			}
		}

		private bool _Hide(Menu target) {
			var mi = _menus.FindLastIndex(v => v == target);
			if (mi != -1) {
				Hide(mi);
				return true;
			}
			return false;
		}

		private void _Hide(int index, bool collapse = true) {
			EventSystem.current.SetSelectedGameObject(null);
			var menu = _menus[index];
			var destroy = true;

			// Persist for reuse
			if (destroy && menu.reuse && menu.persist) {
				for (int i = 0; i < _menus.Count; i++) {
					var other = _menus[i];
					if (i != index && Menu.CompareGroup(menu, other)) {
						goto skipPersist;
					}
				}
				destroy = false;
				if (!_cached.Contains(menu)) _cached.Add(menu);
			}
		skipPersist:

			// Check if reused
			if (destroy && menu.reuse) {
				for (int i = 0; i < _menus.Count; i++) {
					var other = _menus[i];
					if (i != index && other.reuse && Menu.CompareGroup(menu, other)) {
						destroy = false;
						goto skipReuse;
					}
				}
			}
		skipReuse:

			menu.destroy = destroy;
			_menus.RemoveAt(index);
			menu.OnHide();

			if (collapse) {
				var offset = _menus.Count - index;
				while (TryCollapse(_menus.Count - offset)) ;
			}

			var last = _menus.LastOrDefault();
			if (last != null && !last.alwaysVisible) {
				last.OnShow();
			}
		}

		private Menu _Show(Menu source, Action<Menu> initializer = null) {
			EventSystem.current.SetSelectedGameObject(null);
			Menu instance = null;

			// reuse
			if (source.reuse) {
				var ci = _cached.FindIndex(v => Menu.CompareGroup(source, v));
				if (ci != -1) {
					instance = _cached[ci];
					_cached.RemoveAt(ci);
					initializer?.Invoke(instance);
				} else {
					var mi = _menus.FindIndex(v => v.reuse && Menu.CompareGroup(source, v));
					if (mi != -1) {
						instance = _menus[mi];
						initializer?.Invoke(instance);
					}
				}
			}

			// default instance
			if (instance == null) {
				instance = Instantiate(source, transform);
				initializer?.Invoke(instance);
			}

			_menus.Add(instance);
			instance.OnShow();

			while (TryCollapse(_menus.Count - 1) || TryReplace(_menus.Count - 1)) { }

			if (_menus.Count >= 2) {
				var before = _menus[^2];
				if (!before.alwaysVisible) {
					before.OnHide(); // No destroy...
				}
			}

			return instance;
		}

		private bool TryCollapse(int index) {
			if (_menus.Count > index && index > 0) {
				var after = _menus[index];
				var before = _menus[index - 1];
				if (Menu.CompareGroup(after, before) && after.collapse && before.collapse) {
					_Hide(index - 1, false);
					return true;
				}
			}
			return false;
		}

		private bool TryReplace(int index) {
			if (_menus.Count > index && index > 0) {
				var menu = _menus[index];
				if (menu.replaceable) {
					_Hide(index, false);
					return true;
				}
			}
			return false;
		}
	}

}


#if UNITY_EDITOR
namespace Unitylity.Systems.Menus.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Menus), true)]
	public class MenusEditor : Editor {

		Menus t => (Menus)target;

		public override void OnInspectorGUI() {
			serializedObject.Update();

			DrawDefaultInspector();

			if (GUILayout.Button("Pop")) {
				Menus.Pop();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}

}
#endif
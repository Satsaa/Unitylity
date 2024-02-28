
namespace Unitylity.Systems.Menus {

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using Unitylity.Components.Extended;
	using Object = UnityEngine.Object;

#if UNITYLITY_SYSTEMS_MENUS_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Menus) + "/" + nameof(Menus))]
#endif
	public class Menus : UISingleton<Menus> {

		[Tooltip("This menu will be shown automatically during Start")]
		public Menu initialMenu;

		[Tooltip("Animate initial menu?")]
		public bool animateInitialMenu = true;

		[field: SerializeField, HideInInspector]
		public bool hidden { get; internal set; }

		[SerializeField, HideInInspector]
		protected internal List<Menu> _menus;
		public ReadOnlyCollection<Menu> menus => _menus.AsReadOnly();

		private Dictionary<Menu, List<Menu>> _cache;
		protected Dictionary<Menu, List<Menu>> cache => _cache ??= new();

		[field: SerializeField, HideInInspector]
		private List<MenuBackground> backgrounds { get; set; }

		[field: SerializeField, HideInInspector]
		public bool needsUpdate { get; set; }

		/// <summary> Shows a menu representing the source Menu. </summary>
		public static Menu Show(Menu source, bool animate = true) => instance._Show(source, animate);

		/// <summary> Removes the last/newest Menu. </summary>
		public static void Pop(bool animate = true) => instance._Pop(animate);

		/// <summary> Pops until the a root Menu with the same group is removed (only if one is found). </summary>
		public static bool RemoveRoot(Menu source, bool animate = true) => instance._RemoveRoot(source.group, animate);
		/// <summary> Pops until any root Menu is removed (only if one is found). </summary>
		public static bool RemoveRoot(bool animate = true) => instance._RemoveRoot(null, animate);

		/// <summary> Pops until any root Menu of the same group is at the top (only if one is found). </summary>
		public static bool ExposeRoot(Menu source, bool animate = true) => instance._ExposeRoot(source.group, animate);
		/// <summary> Pops until any root Menu is at the top (only if one is found). </summary>
		public static bool ExposeRoot(bool animate = true) => instance._ExposeRoot(null, animate);

		public static bool ClearCache(Menu source) => instance.cache.Remove(source); // !!! Add destroying
		public static void ClearCache() => instance.cache.Clear(); // !!! Add destroying

		/// <summary> Shows all the menus </summary>
		public static void ShowMenus(bool animate = true) => instance._ShowMenus(animate);
		/// <summary> Hides all the menus without removing them </summary>
		public static void HideMenus(bool animate = true) => instance._HideMenus(animate);

		public static AnimationCurve easeOut = new(new(0, 0, 2, 2), new(1, 1));
		public static AnimationCurve easeIn = new(new(0, 0), new(1, 1, 2, 2));
		public static AnimationCurve easeInOut = AnimationCurve.EaseInOut(0, 0, 1, 1);

		protected override void Start() {
			base.Start();
			if (initialMenu) {
				_Show(initialMenu, animateInitialMenu);
			}
		}

		protected virtual void LateUpdate() {
			if (needsUpdate) {
				foreach (var bg in backgrounds) {
					bg.animation += Time.deltaTime * 4;
				}
				for (int i = 3; i < 4; i++) { // i = 0; for rgb mixing 
					var current = 0f;
					foreach (var bg in backgrounds) {
						var curve = easeInOut;
						if (bg.toColor.a == 0) {
							curve = easeIn;
						} else if (bg.fromColor.a == 0) {
							curve = easeOut;
						}
						var target = Mathf.Lerp(bg.fromColor[i], bg.toColor[i], curve.Evaluate(bg.animation));
						if (target <= current) {
							bg.color = WithComponent(bg.color, i, 0);
							continue;
						}
						if (current == 0) {
							bg.color = WithComponent(bg.color, i, target);
							current = target;
							continue;
						}
						var value = (target - current) / (1 - current);
						bg.color = WithComponent(bg.color, i, value);
						current = target;
					}
				}
				foreach (var bg in backgrounds) {
					if (bg.destroy) Destroy(bg.gameObject);
				}
				backgrounds.RemoveAll(v => v.destroy);
				needsUpdate = backgrounds.Any(v => v.animation < 1);
			}
			Color WithComponent(Color color, int component, float value) {
				color[component] = value;
				return color;
			}
		}

		internal void AddBackground(MenuBackground background) {
			if (!backgrounds.Contains(background)) {
				backgrounds.Add(background);
			}
			backgrounds.Sort(
				(a, b) =>
					b.transform.GetSiblingIndex()
					- a.transform.GetSiblingIndex()
			);
		}

		private void _ShowMenus(bool animate) {
			if (hidden) {
				hidden = false;
				ShowTopMenus(animate);
			}
		}

		private void _HideMenus(bool animate) {
			if (!hidden) {
				hidden = true;
				HideTopMenus(animate);
			}
		}

		private bool _RemoveRoot(string group, bool animate) {
			// Pop until any root Menu is removed (only if one is found)
			var oldCount = menus.Count;
			for (int i = oldCount - 1; i >= 0; i--) {
				var current = menus[i];
				if (current.isGroupRoot && (String.IsNullOrEmpty(group) || current.group == group)) {
					var removeCount = oldCount - i;
					RemoveAmount(removeCount, animate);
					ShowTopMenus(animate);
					return true;
				}
			}
			return false;
		}

		private bool _ExposeRoot(string group, bool animate) {
			// Pop until any root Menu is at the top (only if one is found)
			var oldCount = menus.Count;
			for (int i = oldCount - 1; i >= 0; i--) {
				var current = menus[i];
				if (current.isGroupRoot && (String.IsNullOrEmpty(group) || current.group == group)) {
					var removeCount = oldCount - i - 1;
					RemoveAmount(removeCount, animate);
					ShowTopMenus(animate);
					return true;
				}
			}
			return false;
		}

		private void _Pop(bool animate) {
			RemoveTop(animate);
			if (Any()) {
				ShowTopMenus(animate);
				EventSystem.current.SetSelectedGameObject(Top().previouslySelected);
			}
		}


		private Menu _Show(Menu s, bool animate) {
			if (s.source) throw new ArgumentException("You may not create instances of shown Menus!", nameof(s));

			if (Any()) {

				_ShowMenus(animate);

				if (s.isGroupRoot) {
					Debug.Assert(s.group != "");

					// Remove until matching root replace group is removed
					var oldCount = menus.Count;
					for (int i = oldCount - 1; i >= 0; i--) {
						var current = menus[i];
						if (current.isGroupRoot && current.group == s.group) {
							var removeCount = oldCount - i;
							RemoveAmount(removeCount, animate);
							ShowTopMenus(animate);
							break;
						}
					}

				} else {

					// Ignore if duplicate Menu
					if (s == Top().source) {
						// Rethink this limitation? Some later customized menus may use the same source Menu?
						Debug.Log("Menu not shown because the same Menu was previous on stack.");
						return Top();
					}

					// Replace previous
					if (s.group != "" && s.group == Top().group && !Top().isGroupRoot) {
						RemoveTop(animate);
					}

				}

			}

			// Store selected GameObject of previous Menu
			var selection = EventSystem.current.currentSelectedGameObject;
			if (Any() && selection) {
				var top = Top();
				top.previouslySelected = null;
				var parent = selection.transform.parent;
				while (parent != null) {
					if (parent == top.transform) {
						top.previouslySelected = selection;
						break;
					}
					parent = parent.parent;
				}
			}

			if (Any() && !s.showPrevious && Top().visible) {
				Top().OnHideInternal(animate, false);
			}

			// Create or use cached instance
			if (!TryGetCached(s, out var instance)) {
				instance = Instantiate(s, rectTransform);
				instance.source = s;
			}
			instance.transform.SetAsLastSibling();
			_menus.Add(instance);
			EventSystem.current.SetSelectedGameObject(instance.select);


			// Show previous menus if needed
			ShowTopMenus(animate);

			return instance;
		}

		private void ShowTopMenus(bool animate) {
			if (Any() && !Top().visible) {
				Top().OnShowInternal(animate);
			}
			for (int i = menus.Count - 2; i >= 0; i--) {
				var above = menus[i + 1];
				var before = menus[i];
				if (!before.visible && above.showPrevious) {
					before.OnShowInternal(animate);
				} else {
					break;
				}
			}
		}


		private void HideTopMenus(bool animate) {
			if (Any() && Top().visible) {
				Top().OnHideInternal(animate, destroy: false);
			}
			for (int i = menus.Count - 2; i >= 0; i--) {
				var above = menus[i + 1];
				var before = menus[i];
				if (before.visible && above.showPrevious) {
					before.OnHideInternal(animate, destroy: false);
				} else {
					break;
				}
			}
		}

		private void RemoveAmount(int amount, bool animate) {
			for (int i = 0; i < amount; i++) {
				RemoveTop(animate);
			}
		}

		private void RemoveTop(bool animate) {
			var top = Top();
			if (top.cache && !IsCached(top)) {
				AddToCache(top);
				top.OnHideInternal(animate, destroy: false);
			} else {
				top.OnHideInternal(animate, destroy: true);
			}
			_menus.RemoveAt(_menus.Count - 1);
		}

		private bool Any() {
			return menus.Any();
		}

		private Menu Top() {
			return menus.Last();
		}

		private bool IsCached(Menu s) {
			return _cache != null && _cache.TryGetValue(s, out var l) && l.Any();
		}

		private void AddToCache(Menu m) {
			if (!cache.TryGetValue(m, out var l)) {
				_cache[m.source] = new();
			}
			_cache[m.source].Add(m);
			m.inCache = true;
		}

		private bool TryGetCached(Menu s, out Menu m) {
			m = default;
			if (_cache == null || !_cache.TryGetValue(s, out var l) || !l.Any()) return false;
			m = l.Last();
			return true;
		}

		private Menu TakeFromCache(Menu s) {
			var l = _cache[s];
			var m = l.Last();
			s.inCache = false;
			l.RemoveAt(l.Count - 1);
			return m;
		}

		private Menu GetCached(Menu s) {
			return _cache[s].Last();
		}

		private int CalcReplaceDepthForRootMenu(Menu s) {
			Debug.Assert(s.isGroupRoot);
			for (int i = _menus.Count - 1; i >= 0; i--) {
				var b = _menus[i];
				if (b.isGroupRoot && b.group == s.group) {
					return i;
				}
			}
			return 0;
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

		SerializedProperty _menus;

		protected virtual void OnEnable() {
			_menus = serializedObject.FindProperty(nameof(Menus._menus));
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			DrawDefaultInspector();

			PropertyField(_menus);

			using (DisabledScope(!Application.isPlaying)) {
				if (GUILayout.Button("Pop")) {
					Menus.Pop();
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}

}
#endif
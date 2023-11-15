
namespace Unitylity.Systems.Menus {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.UI;
	using Unitylity.Data;
	using Object = UnityEngine.Object;

#if UNITYLITY_SYSTEMS_MENUS_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Menus) + "/" + nameof(Menu))]
#endif
	public class Menu : MonoBehaviour {

		[field: SerializeField, Tooltip("Menus with the same group will replace each other.")]
		public string group { get; private set; }

		[field: SerializeField, Tooltip("Root Menus will pop previous Menus until a root Menu with the same replace group is popped (only if there is one).")]
		public bool isGroupRoot { get; internal set; }

		[field: SerializeField, Tooltip("Cache this Menu? One instance is kept for later use.")]
		public bool cache { get; private set; }

		[field: SerializeField, Tooltip("Menu before this will not be hidden.")]
		public bool showPrevious { get; private set; }

		[field: SerializeField, Tooltip("Select this object automatically for inputs")]
		public GameObject select { get; internal set; }

		[field: SerializeField, Tooltip("When enabled, a blocking background is added behind the menu. Note that multiple hues are not reliably supported.")]
		public ToggleValue<Color> useBackground { get; internal set; } = new() { value = new(0, 0, 0, 0.25f) };

		[field: SerializeField, Tooltip("Clicking the background will hide the menu?")]
		public bool bgPressHidesMenu { get; internal set; } = true;


		/// <summary> The original Menu which this was instantiated from. </summary>
		[field: SerializeField, HideInInspector]
		public Menu source { get; internal set; }


		[field: SerializeField, HideInInspector]
		protected internal MenuBackground background { get; set; }

		[field: SerializeField, HideInInspector]
		protected internal GameObject previouslySelected { get; internal set; }

		/// <summary> True if the Menu should be destroyed after the hide animation. </summary>
		[field: SerializeField, HideInInspector]
		protected bool destroy { get; set; }

		/// <summary> Should be visible after animations? </summary>
		[field: SerializeField, HideInInspector]
		public bool visible { get; protected set; }

		/// <summary> Is this Menu currently in cache. </summary>
		[field: SerializeField, HideInInspector]
		public bool inCache { get; internal set; }


		Animator _animator;
		bool _animatorChecked;
		public Animator animator => (_animatorChecked == (_animatorChecked = true)) ? _animator : _animator = GetComponent<Animator>();

		CanvasGroup _canvasGroup;
		bool _canvasGroupChecked;
		public CanvasGroup canvasGroup => (_canvasGroupChecked == (_canvasGroupChecked = true)) ? _canvasGroup : _canvasGroup = GetComponent<CanvasGroup>();


		/// <summary> Returns true if the Menus should be considered the same. </summary>
		public static bool Compare(Menu a, Menu b) {
#pragma warning disable UNT0007
			return (a.source ?? a) == (b.source ?? b);
#pragma warning restore UNT0007
		}

		/// <summary> Returns true if the Menus should be considered the same. </summary>
		public virtual bool Compare(Menu b) {
			return Compare(this, b);
		}

		protected virtual void OnDestroy() {
			if (background)
			{
				background.destroy = true;
				if (Menus.instance) {
					Menus.instance.needsUpdate = true;
				}
			}
		}

		public virtual void Show() {
			var menus = Menus.instance.menus;
			if (isGroupRoot && menus.Any() && menus.First().group == group) {
				if (!Menus.ExposeRoot(this)) {
					Menus.Show(this);
				}
			} else {
				Menus.Show(this);
			}
		}

		public virtual void Hide() {
			if (Menus.instance.menus.LastOrDefault() == this) {
				Menus.Pop();
			} else {
				Debug.Log($"{nameof(Menu)} was not previous in stack");
			}
		}

		protected virtual void OnShow() { }
		internal void OnShowInternal(bool animate) {
			this.destroy = false;
			if (!visible) {
				visible = true;
				Menus.instance.hidden = false;
				if (useBackground.enabled) {
					if (!background) {
						var backgroundObject = new GameObject($"Background ({gameObject.name})");
						backgroundObject.transform.parent = transform.parent;
						var rt = backgroundObject.AddComponent<RectTransform>();
						rt.anchorMin = Vector2.zero;
						rt.anchorMax = Vector2.one;
						rt.anchoredPosition = Vector2.zero;
						rt.sizeDelta = Vector2.zero;
						rt.localScale = Vector3.one;
						background = backgroundObject.AddComponent<MenuBackground>();
						background.menu = this;
						var color = useBackground.value;
						color.a = 0;
						background.color = color;
						background.fromColor = color;
						background.toColor = useBackground.value;
					} else {
						background.raycastTarget = true;
						background.fromColor = background.color;
						background.toColor = useBackground.value;
					}
					background.transform.SetSiblingIndex(transform.GetSiblingIndex());
					background.animation = 0;
					Menus.instance.needsUpdate = true;
					Menus.instance.AddBackground(background);
				}
				gameObject.SetActive(true);
				if (canvasGroup) {
					canvasGroup.blocksRaycasts = true;
				}
				if (animate && animator) {
					animator.SetTrigger("Show");
				}
				OnShow();
			}
		}

		protected virtual void OnHide() { }
		internal void OnHideInternal(bool animate, bool destroy) {
			this.destroy = destroy;
			if (visible) {
				visible = false;
				if (useBackground.enabled && background) {
					background.fromColor = background.color;
					var color = background.fromColor;
					color.a = 0;
					background.toColor = color;
					background.animation = 0;
					Menus.instance.needsUpdate = true;
					background.raycastTarget = false;
				}
				if (canvasGroup) {
					canvasGroup.blocksRaycasts = false;
				}
				if (animate && animator) {
					animator.SetTrigger("Hide");
				} else {
					gameObject.SetActive(false);
				}
				OnHide();
			}
		}

		public void FinalizeHideAnim() {
			Debug.Assert(!visible);
			if (destroy) Destroy(gameObject);
			else gameObject.SetActive(false);
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
	using Menu = Unitylity.Systems.Menus.Menu;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Menu), true)]
	public class MenuEditor : Editor {

		Menu t => (Menu)target;

		SerializedProperty useBackgroundEnabled;

		void OnEnable() {
			useBackgroundEnabled
				= serializedObject.FindProperty($"{GetBackingFieldName(nameof(Menu.useBackground))}.{nameof(Data.ToggleValue<int>.enabled)}");
		}

		public override void OnInspectorGUI() {
			using (SerializedObjectScope(serializedObject)) {
				if (useBackgroundEnabled.boolValue) {
					DrawPropertiesExcluding(serializedObject, script);
				} else {
					DrawPropertiesExcluding(serializedObject, script, GetBackingFieldName(nameof(Menu.bgPressHidesMenu)));
				}
			}
		}
	}

}
#endif
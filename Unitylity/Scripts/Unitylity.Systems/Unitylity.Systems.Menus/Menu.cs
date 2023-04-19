
namespace Unitylity.Systems.Menus {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if UNITYLITY_SYSTEMS_MENUS_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Menus) + "/" + nameof(Menu))]
#endif
	public class Menu : MonoBehaviour {

		[field: SerializeField, Tooltip("Menus with the same group will replace each other.")]
		public string group { get; private set; }

		[field: SerializeField, Tooltip("Root Menus will pop previous Menus until a Menu with the same replace group is popped (only if there is one).")]
		public bool isGroupRoot { get; internal set; }

		[field: SerializeField, Tooltip("Cache this Menu? One instance is kept for later use.")]
		public bool cache { get; private set; }

		[field: SerializeField, Tooltip("Menu before this will not be hidden.")]
		public bool showPrevious { get; private set; }

		[field: SerializeField, Tooltip("Select this object automatically for inputs")]
		public GameObject select { get; internal set; }


		/// <summary> The original Menu which this was instantiated from. </summary>
		[field: SerializeField, HideInInspector]
		public Menu source { get; internal set; }


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
			return (a.source ?? a) == (b.source ?? b);
		}

		/// <summary> Returns true if the Menus should be considered the same. </summary>
		public virtual bool Compare(Menu b) {
			return Compare(this, b);
		}

		public void Show() {
			Menus.Show(this);
		}
		public void Pop() {
			Menus.Pop();
		}

		protected internal virtual void OnShow(bool animate) {
			this.destroy = false;
			if (!visible) {
				visible = true;
				gameObject.SetActive(true);
				if (canvasGroup) {
					canvasGroup.blocksRaycasts = true;
				}
				if (animate && animator) {
					animator.SetTrigger("Show");
				}
			}
		}

		protected internal virtual void OnHide(bool animate, bool destroy) {
			this.destroy = destroy;
			if (visible) {
				visible = false;
				if (canvasGroup) {
					canvasGroup.blocksRaycasts = false;
				}
				if (animate && animator) {
					animator.SetTrigger("Hide");
				} else {
					gameObject.SetActive(false);
				}
			}
		}

		public void FinalizeHide() {
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

		void OnEnable() {

		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			DrawDefaultInspector();

			serializedObject.ApplyModifiedProperties();
		}
	}

}
#endif
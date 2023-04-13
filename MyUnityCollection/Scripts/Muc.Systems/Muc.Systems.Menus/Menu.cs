
namespace Muc.Systems.Menus {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Menus) + "/" + nameof(Menu))]
#endif
	public class Menu : MonoBehaviour {

		[field: SerializeField, Tooltip("Used to compare Menu groups. Many settings define their behaviour by comparing groups of Menus. Some Menus extend the comparison beyond just this.")]
		public string group { get; private set; }

		[field: SerializeField, Tooltip("Will this Menu close the previous chain of Menus that are replaceable?")]
		public bool replace { get; private set; }

		[field: SerializeField, Tooltip("Will this Menu be closed when a Replacing Menu is added after this?")]
		public bool replaceable { get; private set; }

		[field: SerializeField, Tooltip("Disallow multiple consecutive Menus of this group? The newest one is used.")]
		public bool collapse { get; private set; }

		[field: SerializeField, Tooltip("Reuse this Menu for upcoming Menus of the same group? No per-menu data is stored.")]
		public bool reuse { get; private set; }

		[field: SerializeField, Tooltip("Don't destroy the last instance of a Menu of this group, complementing the reuse toggle.")]
		public bool persist { get; private set; }

		[field: SerializeField, Tooltip("Don't hide the Menu when it is not at the top?")]
		public bool alwaysVisible { get; private set; }

		[field: SerializeField, Tooltip("Does the close key defined by Menus close this menu?")]
		public bool allowCloseKey { get; private set; } = true;

		/// <summary> True if the Menu should be destroyed after the hide animation. </summary>
		[field: SerializeField, HideInInspector]
		public bool destroy { get; set; }

		[field: SerializeField, HideInInspector]
		public bool visible { get; protected set; }


		public static bool CompareGroup(Menu a, Menu b) {
			return a.CompareGroup(b) && b.CompareGroup(a);
		}

		protected virtual bool CompareGroup(Menu other) {
			return this.group == other.group;
		}

		public virtual void OnHide() {
			if (visible) {
				visible = false;
				if (TryGetComponent<Animator>(out var animator)) {
					animator.SetTrigger("Hide");
				}
			}
		}

		public virtual void OnShow() {
			if (!visible) {
				visible = true;
				gameObject.SetActive(true);
				if (TryGetComponent<Animator>(out var animator)) {
					animator.SetTrigger("Show");
				}
			}
		}

		public void FinalizeHide() {
			if (destroy) Destroy(gameObject);
			else if (!visible) gameObject.SetActive(false);
		}

	}

}


#if UNITY_EDITOR
namespace Muc.Systems.Menus.Editor {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using Object = UnityEngine.Object;
	using Menu = Muc.Systems.Menus.Menu;
	using static Muc.Editor.PropertyUtil;
	using static Muc.Editor.EditorUtil;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Menu), true)]
	public class MenuEditor : Editor {

		Menu t => (Menu)target;

		SerializedProperty reuse;

		void OnEnable() {
			reuse = serializedObject.FindProperty(GetBackingFieldName(nameof(Menu.reuse)));
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			if (reuse.boolValue) {
				DrawPropertiesExcluding(serializedObject, "m_Script");
			} else {
				DrawPropertiesExcluding(serializedObject, "m_Script", GetBackingFieldName(nameof(Menu.persist)));
			}

			serializedObject.ApplyModifiedProperties();
		}
	}

}
#endif
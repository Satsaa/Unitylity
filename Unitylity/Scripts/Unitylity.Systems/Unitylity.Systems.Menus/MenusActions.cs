
namespace Unitylity.Systems.Menus {

	using System;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if !UNITYLITY_HIDE_SYSTEM_POPUPS
	[CreateAssetMenu(fileName = nameof(MenusActions), menuName = "Unitylity/" + nameof(Unitylity.Systems.Menus) + "/" + nameof(MenusActions))]
#endif
	public class MenusActions : ScriptableObject {

		public void Pop() => Menus.Pop();
		public void RemoveRoot() => Menus.RemoveRoot();
		public void RemoveRoot(Menu menu) => Menus.RemoveRoot(menu);
		public void ExposeRoot() => Menus.ExposeRoot();
		public void ExposeRoot(Menu menu) => Menus.ExposeRoot(menu);
		public void Show(Menu source) => Menus.Show(source);
		public Menu Show_ret(Menu source) => Menus.Show(source);

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
	[CustomEditor(typeof(MenusActions), true)]
	public class MenusActionsEditor : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			HelpBoxField($"{nameof(MenusActions)} is a ScriptableObject that you can use in UnityEvents. For example the Button on click event to show a Menu.", MessageType.Info);
		}
	}

}
#endif
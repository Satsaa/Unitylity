

namespace Unitylity.Systems.Popups {

	using System;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if !UNITYLITY_HIDE_SYSTEM_POPUPS
	[CreateAssetMenu(fileName = nameof(PopupsActions), menuName = "Unitylity/" + nameof(Unitylity.Systems.Popups) + "/" + nameof(PopupsActions))]
#endif
	public class PopupsActions : ScriptableObject {

		public void TryClose() => Popups.TryClose();

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
	[CustomEditor(typeof(PopupsActions), true)]
	public class PopupsActionsEditor : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			HelpBoxField($"{nameof(PopupsActions)} is a ScriptableObject that you can use in UnityEvents.", MessageType.Info);
		}
	}

}
#endif

namespace Unitylity.Systems.Lang {

	using System;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if !UNITYLITY_HIDE_SYSTEM_POPUPS
	[CreateAssetMenu(fileName = nameof(LangActions), menuName = "Unitylity/" + nameof(Unitylity.Systems.Lang) + "/" + nameof(LangActions))]
#endif
	public class LangActions : ScriptableObject {

		public string Format(string strId) => Lang.Format(strId);
		public string GetStr(string strId) => Lang.GetStr(strId);
		public bool HasStr(string strId) => Lang.HasStr(strId);

	}

}

#if UNITY_EDITOR
namespace Unitylity.Systems.Lang.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LangActions), true)]
	public class LangActionsEditor : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			HelpBoxField($"{nameof(LangActions)} is a ScriptableObject that you can use in UnityEvents.", MessageType.Info);
		}
	}

}
#endif
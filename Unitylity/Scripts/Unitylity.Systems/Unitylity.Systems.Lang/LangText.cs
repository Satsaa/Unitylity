
namespace Unitylity.Systems.Lang {

	using System;
	using System.Collections;

	using System.Collections.Generic;
	using System.Linq;
	using Articy.Unity.Utils;

	using UnityEngine;
	using Unitylity.Data;
	using Object = UnityEngine.Object;

#if UNITYLITY_SYSTEMS_LANG_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Lang) + "/" + nameof(LangText))]
#endif
	public partial class LangText : TMPro.TextMeshProUGUI, ISerializationCallbackReceiver {

		[TextArea(5, 10)]
		[SerializeField]
		internal string _stringQuery;

		[SerializeField] SerializedDictionary<string, Param> context;

		public override string text {
			get => _stringQuery;
			set {
				_stringQuery = value;
				base.text = Lang.Format(_stringQuery, new Converter(context));
			}
		}

		protected override void Awake() {
			context["String"] = new("Hi");
			context["NestTest"] = new("{test}");
			context["Int"] = new(9);
			context["Float"] = new(69.420f);
			if (!Application.isPlaying) {
				base.text = Lang.Format(_stringQuery, new Converter(context));
			}
			base.Awake();
		}

		protected override void OnValidate() {
			base.text = Lang.Format(_stringQuery, new Converter(context));
			base.OnValidate();
		}

		public void OnBeforeSerialize() { }

		public void OnAfterDeserialize() { }

	}

}

#if UNITY_EDITOR
namespace Unitylity.Systems.Lang.Editor {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.UI;
	using Object = UnityEngine.Object;
	using static Unitylity.Editor.PropertyUtil;
	using static Unitylity.Editor.EditorUtil;
	using TMPro;


	[CanEditMultipleObjects]
	[CustomEditor(typeof(LangText), true)]
	public class LangTextEditor : TMPro.EditorUtilities.TMP_EditorPanelUI {

		new LangText target => (LangText)base.target;
		new IEnumerable<LangText> targets => base.targets.Cast<LangText>();

		protected override void OnEnable() {
			base.OnEnable();
			m_TextProp = serializedObject.FindProperty(nameof(LangText._stringQuery));
		}

	}
}
#endif
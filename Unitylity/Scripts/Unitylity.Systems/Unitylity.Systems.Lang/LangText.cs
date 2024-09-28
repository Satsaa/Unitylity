
namespace Unitylity.Systems.Lang {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if UNITYLITY_SYSTEMS_LANG_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Lang) + "/" + nameof(LangText))]
#endif
	public class LangText : TMPro.TextMeshProUGUI {

		public string strId {
			get => _strId;
			set { _strId = value; UpdateText(); }
		}

		[SerializeField] string _strId;

		protected override void Awake() {
			if (string.IsNullOrEmpty(strId)) strId = text;
			else UpdateText();
			base.Awake();
		}

		protected virtual void UpdateText() {
			text = Lang.GetStr(strId);
		}
	}

}
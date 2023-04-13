
namespace Muc.Systems.Lang {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Lang) + "/" + nameof(LangText))]
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
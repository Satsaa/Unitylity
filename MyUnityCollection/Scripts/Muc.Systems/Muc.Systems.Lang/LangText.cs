
namespace Muc.Systems.Lang {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using Object = UnityEngine.Object;

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

namespace Unitylity.Systems.Lang {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Lang) + "/" + nameof(LangFormatText))]
#endif
	public class LangFormatText : LangText {

		public void SetValues(params object[] values) {
			this.values = values;
			UpdateText();
		}

		protected override void UpdateText() {
			if (values != null) {
				text = String.Format(Lang.GetStr(strId), values);
			} else {
				base.UpdateText();
			}
		}

		object[] values;
	}

}
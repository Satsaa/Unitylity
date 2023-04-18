
namespace Unitylity.Systems.Lang {

	using System;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if !UNITYLITY_HIDE_SYSTEM_POPUPS
	[CreateAssetMenu(fileName = nameof(LangActions), menuName = "Unitylity/" + nameof(Unitylity.Systems.Lang) + "/" + nameof(LangActions))]
#endif
	public class LangActions : ScriptableObject {

		public void GetStr(string strId) => Lang.GetStr(strId);
		public void HasStr(string strId) => Lang.HasStr(strId);

		public string GetStr_ret(string strId) => Lang.GetStr(strId);
		public bool HasStr_ret(string strId) => Lang.HasStr(strId);

	}

}

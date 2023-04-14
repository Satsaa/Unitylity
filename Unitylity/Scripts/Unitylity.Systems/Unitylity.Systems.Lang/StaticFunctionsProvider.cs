
namespace Unitylity.Systems {

	using System;
	using UnityEngine;
	using Unitylity.Systems.Menus;
	using Object = UnityEngine.Object;

	public partial class StaticFunctionsProvider {

		public void Lang_GetStr(string strId) => Lang.Lang.GetStr(strId);
		public void Lang_HasStr(string strId) => Lang.Lang.HasStr(strId);

		public string Lang_GetStr_ret(string strId) => Lang.Lang.GetStr(strId);
		public bool Lang_HasStr_ret(string strId) => Lang.Lang.HasStr(strId);

	}

}

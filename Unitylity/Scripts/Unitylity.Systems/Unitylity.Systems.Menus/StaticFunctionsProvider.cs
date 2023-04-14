
namespace Unitylity.Systems {

	using System;
	using UnityEngine;
	using Unitylity.Systems.Menus;
	using Object = UnityEngine.Object;

	public partial class StaticFunctionsProvider {

		public void Menus_Hide(Menu target) => Menus.Menus.Hide(target);
		public void Menus_Pop() => Menus.Menus.Pop();
		public void Menus_Show(Menu source) => Menus.Menus.Show(source);

		public bool Menus_Hide_ret(Menu target) => Menus.Menus.Hide(target);
		public Menu Menus_Show_ret(Menu source) => Menus.Menus.Show(source);

	}

}

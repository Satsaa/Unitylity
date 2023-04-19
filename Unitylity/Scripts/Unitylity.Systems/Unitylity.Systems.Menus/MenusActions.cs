
namespace Unitylity.Systems.Menus {

	using System;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if !UNITYLITY_HIDE_SYSTEM_POPUPS
	[CreateAssetMenu(fileName = nameof(MenusActions), menuName = "Unitylity/" + nameof(Unitylity.Systems.Menus) + "/" + nameof(MenusActions))]
#endif
	public class MenusActions : ScriptableObject {

		public void Pop() => Menus.Pop();
		public void RemoveRoot() => Menus.RemoveRoot();
		public void RemoveRoot(Menu menu) => Menus.RemoveRoot(menu);
		public void ExposeRoot() => Menus.ExposeRoot();
		public void ExposeRoot(Menu menu) => Menus.ExposeRoot(menu);
		public void Show(Menu source) => Menus.Show(source);
		public Menu Show_ret(Menu source) => Menus.Show(source);

	}

}

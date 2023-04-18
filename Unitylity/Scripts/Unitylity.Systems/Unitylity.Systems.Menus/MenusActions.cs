
namespace Unitylity.Systems.Menus {

	using System;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if !UNITYLITY_HIDE_SYSTEM_POPUPS
	[CreateAssetMenu(fileName = nameof(MenusActions), menuName = "Unitylity/" + nameof(Unitylity.Systems.Menus) + "/" + nameof(MenusActions))]
#endif
	public class MenusActions : ScriptableObject {

		public void Hide(Menu target) => Menus.Hide(target);
		public void Pop() => Menus.Pop();
		public void Show(Menu source) => Menus.Show(source);

		public bool Hide_ret(Menu target) => Menus.Hide(target);
		public Menu Show_ret(Menu source) => Menus.Show(source);

	}

}

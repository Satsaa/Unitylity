

namespace Unitylity.Systems.Popups {

	using System;
	using UnityEngine;
	using Object = UnityEngine.Object;

#if !UNITYLITY_HIDE_SYSTEM_POPUPS
	[CreateAssetMenu(fileName = nameof(PopupsActions), menuName = "Unitylity/" + nameof(Unitylity.Systems.Popups) + "/" + nameof(PopupsActions))]
#endif
	public class PopupsActions : ScriptableObject {

		public void TryClose() => Popups.TryClose();

	}

}

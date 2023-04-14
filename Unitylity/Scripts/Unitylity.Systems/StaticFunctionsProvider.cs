

#if (UNITYLITY_SYSTEMS_LANG_DISABLED && UNITYLITY_SYSTEMS_MENUS_DISABLED && UNITYLITY_SYSTEMS_POPUPS_DISABLED)
namespace Unitylity.Systems {

	using UnityEngine;

#if (UNITYLITY_SYSTEMS_LANG_HIDDEN && UNITYLITY_SYSTEMS_MENUS_HIDDEN && UNITYLITY_SYSTEMS_POPUPS_HIDDEN)
	[CreateAssetMenu(fileName = nameof(StaticFunctionsProvider), menuName = "Unitylity/" + nameof(Unitylity.Systems) + "/" + nameof(StaticFunctionsProvider))]
#endif
	public partial class StaticFunctionsProvider : ScriptableObject {

		// Static functions for UnityEvents area added by individual features in Unitylity.Systems.*

	}

}
#endif



namespace Unitylity.Systems {

	using UnityEngine;

#if (!UNITYLITY_HIDE_COMPONENTS && !UNITYLITY_HIDE_SYSTEM_COMPONENTS)
	[CreateAssetMenu(fileName = nameof(StaticFunctionsProvider), menuName = "Unitylity/" + nameof(Unitylity.Systems) + "/" + nameof(StaticFunctionsProvider))]
#endif
	public partial class StaticFunctionsProvider : ScriptableObject {

		// Static functions for UnityEvents area added by individual features in Unitylity.Systems.*

	}

}

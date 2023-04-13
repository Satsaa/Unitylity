
namespace Unitylity.Systems.Input {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Unitylity.Extensions;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.InputSystem;
	using UnityEngine.InputSystem.Controls;

#if (Unitylity_HIDE_COMPONENTS || Unitylity_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Input) + "/" + nameof(Vector2Input))]
#endif
	public class Vector2Input : Input<Vector2> {

		protected override void OnInputUpdate(InputAction.CallbackContext context) {
			if (value != (value = context.ReadValue<Vector2>())) {
				onChange.Invoke(value);
			}
		}

		internal override bool IsControlSupported(InputControl control) {
			return control is Vector2Control;
		}

	}

}
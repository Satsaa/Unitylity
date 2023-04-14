
namespace Unitylity.Systems.Input {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.InputSystem;
	using UnityEngine.InputSystem.Controls;
	using UnityEngine.InputSystem.LowLevel;
	using Unitylity.Extensions;

#if UNITYLITY_SYSTEMS_INPUT_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Input) + "/" + nameof(TouchInput))]
#endif
	public class TouchInput : Input<TouchState> {

		protected override void OnInputUpdate(InputAction.CallbackContext context) {
			value = context.ReadValue<TouchState>();
			onChange.Invoke(value);
		}

		internal override bool IsControlSupported(InputControl control) {
			return control is TouchControl;
		}

	}

}
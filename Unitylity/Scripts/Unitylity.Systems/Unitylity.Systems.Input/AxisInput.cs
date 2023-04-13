
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
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Input) + "/" + nameof(AxisInput))]
#endif
	public class AxisInput : Input<float> {

		protected override void OnInputUpdate(InputAction.CallbackContext context) {
			if (value != (value = context.ReadValue<float>())) {
				onChange.Invoke(value);
			}
		}

		internal override bool IsControlSupported(InputControl control) {
			return control is AxisControl;
		}

	}

}
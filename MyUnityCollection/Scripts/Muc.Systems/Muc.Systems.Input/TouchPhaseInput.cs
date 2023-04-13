
namespace Muc.Systems.Input {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Muc.Extensions;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.InputSystem;
	using UnityEngine.InputSystem.Controls;
	using TouchPhase = UnityEngine.InputSystem.TouchPhase;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Muc.Systems.Input) + "/" + nameof(TouchPhaseInput))]
#endif
	public class TouchPhaseInput : Input<TouchPhase> {

		protected override void OnInputUpdate(InputAction.CallbackContext context) {
			if (value != (value = context.ReadValue<TouchPhase>())) {
				onChange.Invoke(value);
			}
		}

		internal override bool IsControlSupported(InputControl control) {
			return control is TouchPhaseControl;
		}

	}

}
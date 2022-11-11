
namespace Muc.Systems.Input {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Muc.Extensions;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.InputSystem;
	using UnityEngine.InputSystem.Controls;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Input) + "/" + nameof(AxisInput))]
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
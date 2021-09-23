

namespace Muc.Systems.Input {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Muc.Extensions;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.InputSystem;
	using UnityEngine.InputSystem.Controls;
	using UnityEngine.InputSystem.LowLevel;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Input) + "/" + nameof(TouchInput))]
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
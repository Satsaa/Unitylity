
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
	[AddComponentMenu("Unitylity/" + nameof(Muc.Systems.Input) + "/" + nameof(Vector3Input))]
#endif
	public class Vector3Input : Input<Vector3> {

		protected override void OnInputUpdate(InputAction.CallbackContext context) {
			if (value != (value = context.ReadValue<Vector3>())) {
				onChange.Invoke(value);
			}
		}

		internal override bool IsControlSupported(InputControl control) {
			return control is Vector3Control;
		}

	}

}
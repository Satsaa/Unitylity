
namespace Unitylity.Systems.Input {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Unitylity.Extensions;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.InputSystem;
	using UnityEngine.InputSystem.Controls;

#if (Unitylity_HIDE_COMPONENTS || Unitylity_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Input) + "/" + nameof(ButtonInput))]
#endif
	public class ButtonInput : AxisInput {

		public bool pressed { get; protected set; }

		public UnityEvent<bool> onStateChange;
		public UnityEvent onPress;
		public UnityEvent onRelease;

		protected override void OnInputUpdate(InputAction.CallbackContext context) {

			if (value != (value = context.ReadValue<float>())) {
				onChange.Invoke(value);
			}

			if (pressed != (pressed = context.ReadValueAsButton())) {
				onStateChange.Invoke(pressed);
				if (pressed) {
					onPress.Invoke();
				} else {
					onRelease.Invoke();
				}
			}
		}

		internal override bool IsControlSupported(InputControl control) {
			return control is InputControl<float>;
		}

	}

}
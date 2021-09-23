

namespace Muc.Systems.Values {

	using System;
	using System.Linq;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;

	using UnityEngine;
	using UnityEngine.Events;

	using Muc.Extensions;


	/// <summary>
	/// A value container which allows adding Modifiers which change the way get, set, add or sub (subtraction) operations are handled.
	/// </summary>
	/// <typeparam name="T">The type of the contained value</typeparam>
	public abstract class ArithmeticValue<T> : Value<T> {


		/// <summary> Standard addition operation </summary>
		protected abstract T AddValues(T a, T b);

		/// <summary> Standard subtraction operation </summary>
		protected abstract T SubtractValues(T a, T b);


		/// <summary> Adds addition, after it is modified, to the value. It is not recommended to use this function inside Modifiers! </summary>
		public virtual T Add(T addition) {
			foreach (var handler in addHandlers) {
				addition = handler(addition);
				if (HadPostHandlerActions()) {
					if (WasSkipped()) break;
				}
			}
			if (HadOnCompleteActions()) {
				DoOnCompletes();
				if (WasIgnored()) return value;
			}
			return value = AddValues(value, addition);
		}

		/// <summary> Subtracts subtraction, after it is modified, from the value. It is not recommended to use this function inside Modifiers! </summary>
		public virtual T Sub(T subtraction) {
			foreach (var handler in subHandlers) {
				subtraction = handler(subtraction);
				if (HadPostHandlerActions()) {
					if (WasSkipped()) break;
				}
			}
			if (HadOnCompleteActions()) {
				DoOnCompletes();
				if (WasIgnored()) return value;
			}
			return value = SubtractValues(value, subtraction);
		}

	}
}
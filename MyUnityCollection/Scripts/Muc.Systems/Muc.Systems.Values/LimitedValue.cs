

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
	/// <para>A value container which allows adding Modifiers which change the way get, set, add or sub (subtraction) operations are handled.</para>
	/// <para>Additionally enforces a limit on the value. The Get function can still return values higher than the maximum value, if, for example, a Get Modifier increases the return value.</para>
	/// </summary>
	/// <typeparam name="T">The type of the contained value</typeparam>
	public abstract class LimitedValue<T> : ArithmeticValue<T> where T : IComparable<T> {

		public abstract T max { get; set; }
		public abstract bool enforceMax { get; set; }

		protected override T value {
			get => _value;
			set {
				if (enforceMax) {
					var comparison = value.CompareTo(max);
					if (comparison > 0) base.value = max;
					else base.value = value;
				} else {
					base.value = value;
				}
			}
		}
	}
}
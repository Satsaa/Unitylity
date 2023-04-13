
namespace Unitylity.Numerics {

	using System;
	using UnityEngine;

	/// <summary> A decimal which loops from the specified threshold value to zero. The value never reaches the threshold. </summary>
	[Serializable]
	public struct CircularDecimal {

		[field: SerializeField] public decimal value { get; private set; }
		[field: SerializeField] public decimal threshold { get; private set; }

		public static CircularDecimal operator +(CircularDecimal a, decimal b) => new(a.value + b, a.threshold);
		public static CircularDecimal operator ++(CircularDecimal a) => new(a.value + 1m, a.threshold);
		public static CircularDecimal operator -(CircularDecimal a, decimal b) => new(a.value - b, a.threshold);
		public static CircularDecimal operator --(CircularDecimal a) => new(a.value - 1m, a.threshold);


		// public static implicit operator sbyte(CircularDecimal a) => a.value;
		// public static implicit operator byte(CircularDecimal a) => a.value;
		// public static implicit operator short(CircularDecimal a) => a.value;
		// public static implicit operator ushort(CircularDecimal a) => a.value;
		// public static implicit operator int(CircularDecimal a) => a.value;
		// public static implicit operator uint(CircularDecimal a) => a.value;
		// public static implicit operator long(CircularDecimal a) => a.value;
		// public static implicit operator ulong(CircularDecimal a) => a.value;

		// public static implicit operator float(CircularDecimal a) => a.value;
		// public static implicit operator double(CircularDecimal a) => a.value;
		public static implicit operator decimal(CircularDecimal a) => a.value;

		public static explicit operator sbyte(CircularDecimal a) => (sbyte)a.value;
		public static explicit operator byte(CircularDecimal a) => (byte)a.value;
		public static explicit operator short(CircularDecimal a) => (short)a.value;
		public static explicit operator ushort(CircularDecimal a) => (ushort)a.value;
		public static explicit operator int(CircularDecimal a) => (int)a.value;
		public static explicit operator uint(CircularDecimal a) => (uint)a.value;
		public static explicit operator long(CircularDecimal a) => (long)a.value;
		public static explicit operator ulong(CircularDecimal a) => (ulong)a.value;

		public static explicit operator float(CircularDecimal a) => (float)a.value;
		public static explicit operator double(CircularDecimal a) => (double)a.value;
		// public static explicit operator decimal(CircularDecimal a) => (decimal)a.value;


		/// <summary> Creates a decimal which loops from threshold to zero. The value never reaches the threshold. </summary>
		public CircularDecimal(decimal value, decimal threshold) {

			if (threshold <= 0m) throw new System.ArgumentOutOfRangeException($"{threshold} must be positive", nameof(threshold));

			if (value >= threshold) value = value == threshold ? 0m : value % threshold;
			else if (value < 0m) value = threshold + value % threshold;

			this.value = value;
			this.threshold = threshold;
		}

		new public string ToString() => value.ToString();

	}

}
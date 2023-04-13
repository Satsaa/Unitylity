
namespace Unitylity.Numerics {

	using System;
	using UnityEngine;

	/// <summary> An integer which loops from the specified threshold value to zero. The value never reaches the threshold. </summary>
	[Serializable]
	public struct CircularInt {

		[field: SerializeField] public int value { get; private set; }
		[field: SerializeField] public int threshold { get; private set; }

		public static CircularInt operator +(CircularInt a, int b) => new(a.value + b, a.threshold);
		public static CircularInt operator ++(CircularInt a) => new(a.value + 1, a.threshold);
		public static CircularInt operator -(CircularInt a, int b) => new(a.value - b, a.threshold);
		public static CircularInt operator --(CircularInt a) => new(a.value - 1, a.threshold);


		// public static implicit operator sbyte(CircularInt a) => a.value;
		// public static implicit operator byte(CircularInt a) => a.value;
		// public static implicit operator short(CircularInt a) => a.value;
		// public static implicit operator ushort(CircularInt a) => a.value;
		public static implicit operator int(CircularInt a) => a.value;
		// public static implicit operator uint(CircularInt a) => a.value;
		public static implicit operator long(CircularInt a) => a.value;
		// public static implicit operator ulong(CircularInt a) => a.value;

		public static implicit operator float(CircularInt a) => a.value;
		public static implicit operator double(CircularInt a) => a.value;
		public static implicit operator decimal(CircularInt a) => a.value;

		public static explicit operator sbyte(CircularInt a) => (sbyte)a.value;
		public static explicit operator byte(CircularInt a) => (byte)a.value;
		public static explicit operator short(CircularInt a) => (short)a.value;
		public static explicit operator ushort(CircularInt a) => (ushort)a.value;
		// public static explicit operator int(CircularInt a) => (int)a.value;
		public static explicit operator uint(CircularInt a) => (uint)a.value;
		// public static explicit operator long(CircularInt a) => (long)a.value;
		public static explicit operator ulong(CircularInt a) => (ulong)a.value;

		// public static explicit operator float(CircularInt a) => (float)a.value;
		// public static explicit operator double(CircularInt a) => (double)a.value;
		// public static explicit operator decimal(CircularInt a) => (decimal)a.value;


		/// <summary> Creates an integer which loops from threshold to zero. The value never reaches the threshold. </summary>
		public CircularInt(int value, int threshold) {

			if (threshold <= 0) throw new ArgumentOutOfRangeException($"{threshold} must be positive", nameof(threshold));

			if (value >= threshold) value = value == threshold ? 0 : value % threshold;
			else if (value < 0) value = threshold + value % threshold;

			this.value = value;
			this.threshold = threshold;
		}

		new public string ToString() => value.ToString();

	}

}
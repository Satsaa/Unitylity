
namespace Unitylity.Numerics {

	using System;
	using UnityEngine;

	/// <summary> A long which loops from the specified threshold value to zero. The value never reaches the threshold. </summary>
	[Serializable]
	public struct CircularLong {

		[field: SerializeField] public long value { get; private set; }
		[field: SerializeField] public long threshold { get; private set; }

		public static CircularLong operator +(CircularLong a, long b) => new(a.value + b, a.threshold);
		public static CircularLong operator ++(CircularLong a) => new(a.value + 1, a.threshold);
		public static CircularLong operator -(CircularLong a, long b) => new(a.value - b, a.threshold);
		public static CircularLong operator --(CircularLong a) => new(a.value - 1, a.threshold);


		// public static implicit operator sbyte(CircularLong a) => a.value;
		// public static implicit operator byte(CircularLong a) => a.value;
		// public static implicit operator short(CircularLong a) => a.value;
		// public static implicit operator ushort(CircularLong a) => a.value;
		// public static implicit operator int(CircularLong a) => a.value;
		// public static implicit operator uint(CircularLong a) => a.value;
		public static implicit operator long(CircularLong a) => a.value;
		// public static implicit operator ulong(CircularLong a) => a.value;

		public static implicit operator float(CircularLong a) => a.value;
		public static implicit operator double(CircularLong a) => a.value;
		public static implicit operator decimal(CircularLong a) => a.value;

		public static explicit operator sbyte(CircularLong a) => (sbyte)a.value;
		public static explicit operator byte(CircularLong a) => (byte)a.value;
		public static explicit operator short(CircularLong a) => (short)a.value;
		public static explicit operator ushort(CircularLong a) => (ushort)a.value;
		public static explicit operator int(CircularLong a) => (int)a.value;
		public static explicit operator uint(CircularLong a) => (uint)a.value;
		// public static explicit operator long(CircularLong a) => (long)a.value;
		public static explicit operator ulong(CircularLong a) => (ulong)a.value;

		// public static explicit operator float(CircularLong a) => (float)a.value;
		// public static explicit operator double(CircularLong a) => (double)a.value;
		// public static explicit operator decimal(CircularLong a) => (decimal)a.value;


		/// <summary> Creates a long which loops from threshold to zero. The value never reaches the threshold. </summary>
		public CircularLong(long value, long threshold) {

			if (threshold <= 0) throw new ArgumentOutOfRangeException($"{threshold} must be positive", nameof(threshold));

			if (value >= threshold) value = value == threshold ? 0 : value % threshold;
			else if (value < 0) value = threshold + value % threshold;

			this.value = value;
			this.threshold = threshold;
		}

		new public string ToString() => value.ToString();

	}

}
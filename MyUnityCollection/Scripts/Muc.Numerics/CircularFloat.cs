
namespace Unitylity.Numerics {

	using System;
	using UnityEngine;

	/// <summary> A float which loops from the specified threshold value to zero. The value never reaches the threshold. </summary>
	[Serializable]
	public struct CircularFloat {

		[field: SerializeField] public float value { get; private set; }
		[field: SerializeField] public float threshold { get; private set; }

		public static CircularFloat operator +(CircularFloat a, float b) => new(a.value + b, a.threshold);
		public static CircularFloat operator ++(CircularFloat a) => new(a.value + 1f, a.threshold);
		public static CircularFloat operator -(CircularFloat a, float b) => new(a.value - b, a.threshold);
		public static CircularFloat operator --(CircularFloat a) => new(a.value - 1f, a.threshold);


		// public static implicit operator sbyte(CircularFloat a) => a.value;
		// public static implicit operator byte(CircularFloat a) => a.value;
		// public static implicit operator short(CircularFloat a) => a.value;
		// public static implicit operator ushort(CircularFloat a) => a.value;
		// public static implicit operator int(CircularFloat a) => a.value;
		// public static implicit operator uint(CircularFloat a) => a.value;
		// public static implicit operator long(CircularFloat a) => a.value;
		// public static implicit operator ulong(CircularFloat a) => a.value;

		public static implicit operator float(CircularFloat a) => a.value;
		public static implicit operator double(CircularFloat a) => a.value;
		// public static implicit operator decimal(CircularFloat a) => a.value;

		public static explicit operator sbyte(CircularFloat a) => (sbyte)a.value;
		public static explicit operator byte(CircularFloat a) => (byte)a.value;
		public static explicit operator short(CircularFloat a) => (short)a.value;
		public static explicit operator ushort(CircularFloat a) => (ushort)a.value;
		public static explicit operator int(CircularFloat a) => (int)a.value;
		public static explicit operator uint(CircularFloat a) => (uint)a.value;
		public static explicit operator long(CircularFloat a) => (long)a.value;
		public static explicit operator ulong(CircularFloat a) => (ulong)a.value;

		// public static explicit operator float(CircularFloat a) => (float)a.value;
		// public static explicit operator double(CircularFloat a) => (double)a.value;
		public static explicit operator decimal(CircularFloat a) => (decimal)a.value;


		/// <summary> Creates a float which loops from threshold to zero. The value never reaches the threshold. </summary>
		public CircularFloat(float value, float threshold) {

			if (threshold <= 0f) throw new ArgumentOutOfRangeException($"{threshold} must be positive", nameof(threshold));

			if (value >= threshold) value = value == threshold ? 0f : value % threshold;
			else if (value < 0f) value = threshold + value % threshold;

			this.value = value;
			this.threshold = threshold;
		}

		new public string ToString() => value.ToString();

	}

}
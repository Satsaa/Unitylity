

namespace Muc.Math {

  /// <summary> A double which loops from the specified threshold value to zero. The value never reaches the threshold. </summary>
  public readonly struct CircularDouble {

    public readonly double value;
    public readonly double ceiling;

    public static CircularDouble operator +(CircularDouble a, double b) => new CircularDouble(a.value + b, a.ceiling);
    public static CircularDouble operator ++(CircularDouble a) => new CircularDouble(a.value + 1d, a.ceiling);
    public static CircularDouble operator -(CircularDouble a, double b) => new CircularDouble(a.value - b, a.ceiling);
    public static CircularDouble operator --(CircularDouble a) => new CircularDouble(a.value - 1d, a.ceiling);


    // public static implicit operator sbyte(CircularDouble a) => a.value;
    // public static implicit operator byte(CircularDouble a) => a.value;
    // public static implicit operator short(CircularDouble a) => a.value;
    // public static implicit operator ushort(CircularDouble a) => a.value;
    // public static implicit operator int(CircularDouble a) => a.value;
    // public static implicit operator uint(CircularDouble a) => a.value;
    // public static implicit operator long(CircularDouble a) => a.value;
    // public static implicit operator ulong(CircularDouble a) => a.value;

    // public static implicit operator float(CircularDouble a) => a.value;
    public static implicit operator double(CircularDouble a) => a.value;
    // public static implicit operator decimal(CircularDouble a) => a.value;

    public static explicit operator sbyte(CircularDouble a) => (sbyte)a.value;
    public static explicit operator byte(CircularDouble a) => (byte)a.value;
    public static explicit operator short(CircularDouble a) => (short)a.value;
    public static explicit operator ushort(CircularDouble a) => (ushort)a.value;
    public static explicit operator int(CircularDouble a) => (int)a.value;
    public static explicit operator uint(CircularDouble a) => (uint)a.value;
    public static explicit operator long(CircularDouble a) => (long)a.value;
    public static explicit operator ulong(CircularDouble a) => (ulong)a.value;

    public static explicit operator float(CircularDouble a) => (float)a.value;
    // public static explicit operator double(CircularDouble a) => (double)a.value;
    public static explicit operator decimal(CircularDouble a) => (decimal)a.value;


    /// <summary> Creates a double which loops from threshold to zero. The value never reaches the threshold. </summary>
    public CircularDouble(double value, double threshold) {

      if (threshold <= 0d) throw new System.ArgumentOutOfRangeException($"{threshold} is less than one", nameof(threshold));

      if (value >= threshold) value = value == threshold ? 0d : value % threshold;
      else if (value < 0d) value = threshold + value % threshold;

      this.value = value;
      this.ceiling = threshold;
    }

    public new string ToString() => value.ToString();
  }

}
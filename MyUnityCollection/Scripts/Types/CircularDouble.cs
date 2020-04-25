

namespace Muc.Types {

  /// <summary> A double which loops from the specified threshold value to zero. The value never reaches the threshold. </summary>
  public readonly struct CircularDouble {

    public readonly double value;
    public readonly double ceil;

    public static CircularDouble operator +(CircularDouble a, double b) => new CircularDouble(a.value + b, a.ceil);
    public static CircularDouble operator ++(CircularDouble a) => new CircularDouble(a.value + 1, a.ceil);
    public static CircularDouble operator -(CircularDouble a, double b) => new CircularDouble(a.value - b, a.ceil);
    public static CircularDouble operator --(CircularDouble a) => new CircularDouble(a.value + 1, a.ceil);

    public static explicit operator int(CircularDouble a) => (int)a.value;
    public static explicit operator float(CircularDouble a) => (float)a.value;
    public static implicit operator double(CircularDouble a) => a.value;
    public static explicit operator decimal(CircularDouble a) => (decimal)a.value;

    /// <summary> Creates an integer value which loops from threshold to zero. The value never reaches the threshold. </summary>
    public CircularDouble(double value, double threshold) {

      if (threshold <= 0) throw new System.ArgumentOutOfRangeException($"{threshold} is less than one", nameof(threshold));

      if (value >= threshold) value = value == threshold ? 0 : value % threshold;
      else if (value < 0) value = threshold + value % threshold;

      this.value = value;
      this.ceil = threshold;
    }

    public new string ToString() => value.ToString();
  }

}
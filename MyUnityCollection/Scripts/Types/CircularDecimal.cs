

namespace MUC.Types {

  /// <summary> A decimal which loops from the specified threshold value to zero. The value never reaches the threshold. </summary>
  public readonly struct CircularDecimal {

    public readonly decimal value;
    public readonly decimal ceil;

    public static CircularDecimal operator +(CircularDecimal a, decimal b) => new CircularDecimal(a.value + b, a.ceil);
    public static CircularDecimal operator ++(CircularDecimal a) => new CircularDecimal(a.value + 1, a.ceil);
    public static CircularDecimal operator -(CircularDecimal a, decimal b) => new CircularDecimal(a.value - b, a.ceil);
    public static CircularDecimal operator --(CircularDecimal a) => new CircularDecimal(a.value + 1, a.ceil);

    public static explicit operator int(CircularDecimal a) => (int)a.value;
    public static explicit operator float(CircularDecimal a) => (float)a.value;
    public static explicit operator double(CircularDecimal a) => (double)a.value;
    public static implicit operator decimal(CircularDecimal a) => a.value;

    /// <summary> Creates an integer value which loops from threshold to zero. The value never reaches the threshold. </summary>
    public CircularDecimal(decimal value, decimal threshold) {

      if (threshold <= 0) throw new System.ArgumentOutOfRangeException($"{threshold} is less than one", nameof(threshold));

      if (value >= threshold) value = value == threshold ? 0 : value % threshold;
      else if (value < 0) value = threshold + value % threshold;

      this.value = value;
      this.ceil = threshold;
    }

    public new string ToString() => value.ToString();
  }

}
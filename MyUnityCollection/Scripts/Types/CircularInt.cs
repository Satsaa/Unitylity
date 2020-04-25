

namespace Muc.Types {

  /// <summary> An integer which loops from the specified threshold value to zero. The value never reaches the threshold. </summary>
  public readonly struct CircularInt {

    public readonly int value;
    public readonly int ceil;

    public static CircularInt operator +(CircularInt a, int b) => new CircularInt(a.value + b, a.ceil);
    public static CircularInt operator ++(CircularInt a) => new CircularInt(a.value + 1, a.ceil);
    public static CircularInt operator -(CircularInt a, int b) => new CircularInt(a.value - b, a.ceil);
    public static CircularInt operator --(CircularInt a) => new CircularInt(a.value - 1, a.ceil);

    public static implicit operator int(CircularInt a) => a.value;
    public static implicit operator float(CircularInt a) => a.value;
    public static implicit operator double(CircularInt a) => a.value;
    public static implicit operator decimal(CircularInt a) => a.value;


    /// <summary> Creates an integer value which loops from threshold to zero. The value never reaches the threshold. </summary>
    public CircularInt(int value, int threshold) {

      if (threshold <= 0) throw new System.ArgumentOutOfRangeException($"{threshold} is less than one", nameof(threshold));

      if (value >= threshold) value = value == threshold ? 0 : value % threshold;
      else if (value < 0) value = threshold + value % threshold;

      this.value = value;
      this.ceil = threshold;
    }

    public new string ToString() => value.ToString();
  }

}
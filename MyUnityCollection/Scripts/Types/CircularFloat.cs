

namespace MUC.Types {

  /// <summary> A float which loops from the specified threshold value to zero. The value never reaches the threshold. </summary>
  public readonly struct CircularFloat {

    public readonly float value;
    public readonly float ceil;

    public static CircularFloat operator +(CircularFloat a, float b) => new CircularFloat(a.value + b, a.ceil);
    public static CircularFloat operator ++(CircularFloat a) => new CircularFloat(a.value + 1, a.ceil);
    public static CircularFloat operator -(CircularFloat a, float b) => new CircularFloat(a.value - b, a.ceil);
    public static CircularFloat operator --(CircularFloat a) => new CircularFloat(a.value + 1, a.ceil);

    public static explicit operator int(CircularFloat a) => (int)a.value;
    public static implicit operator float(CircularFloat a) => a.value;
    public static implicit operator double(CircularFloat a) => a.value;
    public static explicit operator decimal(CircularFloat a) => (decimal)a.value;

    /// <summary> Creates an integer value which loops from threshold to zero. The value never reaches the threshold. </summary>
    public CircularFloat(float value, float threshold) {

      if (threshold <= 0) throw new System.ArgumentOutOfRangeException($"{threshold} is less than one", nameof(threshold));

      if (value >= threshold) value = value == threshold ? 0 : value % threshold;
      else if (value < 0) value = threshold + value % threshold;

      this.value = value;
      this.ceil = threshold;
    }

    public new string ToString() => value.ToString();
  }

}
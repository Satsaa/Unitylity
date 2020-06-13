

namespace Muc {

  using System;

  // IComparable, IComparable<int>, IConvertible, IEquatable<int>, IFormattable

  public static class BinUtil {

    /// <summary> Returns true if the binary values contain a set bit at the same position </summary>
    public static bool BitsOverlap<T>(T a, T b) where T : IConvertible => (Convert.ToInt32(a) & Convert.ToInt32(b)) != 0;

  }

}
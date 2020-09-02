

namespace Muc.Extensions {

  using System.Collections;
  using System.Collections.Generic;

  public static class IEnumerableExtensions {

    public static IEnumerable<T> Enumerate<T>(this IEnumerator<T> enumerator) {
      while (enumerator.MoveNext())
        yield return enumerator.Current;
    }

  }
}
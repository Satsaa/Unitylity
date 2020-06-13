

namespace Muc.Types.Extensions {

  using System.Collections;
  using System.Collections.Generic;

  public static class EnumeratorExtensions {
    public static IEnumerable<T> Enumerate<T>(this IEnumerator<T> enumerator) {
      while (enumerator.MoveNext())
        yield return enumerator.Current;
    }
  }
}
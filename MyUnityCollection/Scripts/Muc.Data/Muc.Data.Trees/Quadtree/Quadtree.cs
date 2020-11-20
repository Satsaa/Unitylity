

namespace Muc.Data.Trees {

  using System;
  using System.Linq;
  using System.Collections.Generic;
  using UnityEngine;
  using System.Collections;

  public static class Quadtree {
    public static Vector2Int IndexToSigns(int i) {
      var pos = new Vector2Int(0, 0);
      if (i >= 2) { i -= 2; pos.y = 1; }
      if (i >= 1) { i -= 1; pos.x = 1; }
      return pos;
    }

    public static int SignsToIndex(Vector2 signs) {
      int i = 0;
      if (signs.x >= 1) i += 1;
      if (signs.y >= 1) i += 2;
      return i;
    }
  }

  public partial class Quadtree<T> : QuadtreeCell<T>, ITree<T> {

    ITreeEnumerator<ICell> ITree.GetEnumerator() => (ITreeEnumerator<ICell>)GetEnumerator();
    ITreeEnumerator<ICell<T>> ITree<T>.GetEnumerator() => (ITreeEnumerator<ICell<T>>)GetEnumerator();

  }
}
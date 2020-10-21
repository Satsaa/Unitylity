

namespace Muc.Data.Trees {

  using System;
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;

  using UnityEngine;

  using Muc.Extensions;

  public class AutoOctree<T> : ITree<T> {

    public readonly Vector3 origin;
    public readonly Vector3 scale;

    readonly Dictionary<Vector3Int, Octree<T>> trees = new Dictionary<Vector3Int, Octree<T>>();

    public AutoOctree(Vector3 origin, Vector3 scale) {
      this.origin = origin;
      this.scale = scale;
    }

    ITreeEnumerator<ICell> ITree.GetEnumerator() => (ITreeEnumerator<ICell>)GetEnumerator();
    public ITreeEnumerator<ICell<T>> GetEnumerator() {
      throw new NotImplementedException();
    }

    public bool Get(Vector3Int pos, out Octree<T> result) {
      return trees.TryGetValue(pos, out result);
    }

  }

}
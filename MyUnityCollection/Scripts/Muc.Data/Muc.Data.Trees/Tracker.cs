

namespace Muc.Data.Trees {

  using System;
  using System.Linq;
  using System.Collections.Generic;
  using UnityEngine;
  using System.Collections;



  internal class Tracker<T> {
    public T child;
    public int i;

    public Tracker(T cell, int i = 0) {
      this.child = cell;
      this.i = i;
    }
  }

}
using System;
using UnityEngine;

namespace Muc.Editor {

  [AttributeUsage(AttributeTargets.Field)]
  public class ReorderableAttribute : PropertyAttribute {
    internal bool readOnly;

    public ReorderableAttribute(bool readOnly = false) {
      this.readOnly = readOnly;
    }
  }

}
using System;
using UnityEngine;

namespace Muc.Editor {

  [AttributeUsage(AttributeTargets.Field)]
  public class ReorderableAttribute : PropertyAttribute {

    public float r = 0, g = 0, b = 0;

    public bool disableAdding;

    public bool disableRemoving;

    public bool disableAddingAndRemoving {
      get => disableAdding && disableRemoving;
      set => disableAdding = disableRemoving = value;
    }

    public bool disableDragging = false;

    public bool elementsAreSubassets = false;

    public string elementHeaderFormat = null;

    public string listHeaderFormat = null;

    public bool hideFooterButtons = false;

    public string[] parallelListNames;

    public enum ParallelListLayout { Rows, Columns };

    public ParallelListLayout parallelListLayout = 0;

    public ReorderableAttribute() { }

    public ReorderableAttribute(params string[] parallelListNames) {
      this.parallelListNames = parallelListNames;
    }

    public const string SingularPluralBlockBegin = "{{";
    public const string SingularPluralBlockSeparator = "|";
    public const string SingularPluralBlockEnd = "}}";

    public string singularListHeaderFormat {
      get {
        if (listHeaderFormat == null)
          return null;
        var value = listHeaderFormat;
        while (value.Contains(SingularPluralBlockBegin)) {
          int beg = value.IndexOf(SingularPluralBlockBegin);
          int end = value.IndexOf(SingularPluralBlockEnd, beg);
          if (end < 0) break;
          end += SingularPluralBlockEnd.Length;
          int blockLen = end - beg;
          var block = value.Substring(beg, blockLen);
          int sep = value.IndexOf(SingularPluralBlockSeparator, beg);
          if (sep < 0) {
            value = value.Replace(block, "");
          } else {
            beg += SingularPluralBlockBegin.Length;
            int singularLen = (sep - beg);
            var singular = value.Substring(beg, singularLen);
            value = value.Replace(block, singular);
          }
        }
        return value;
      }
    }

    public string pluralListHeaderFormat {
      get {
        if (listHeaderFormat == null)
          return null;
        var value = listHeaderFormat;
        while (value.Contains(SingularPluralBlockBegin)) {
          int beg = value.IndexOf(SingularPluralBlockBegin);
          int end = value.IndexOf(SingularPluralBlockEnd, beg);
          if (end < 0) break;
          end += SingularPluralBlockEnd.Length;
          int blockLen = end - beg;
          var block = value.Substring(beg, blockLen);
          int sep = value.IndexOf(SingularPluralBlockSeparator, beg);
          if (sep < 0) {
            beg += SingularPluralBlockBegin.Length;
            end -= SingularPluralBlockEnd.Length;
            int pluralLen = (end - beg);
            var plural = value.Substring(beg, pluralLen);
            value = value.Replace(block, plural);
          } else {
            sep = sep + SingularPluralBlockSeparator.Length;
            end -= SingularPluralBlockEnd.Length;
            int pluralLen = (end - sep);
            var plural = value.Substring(beg, pluralLen);
            value = value.Replace(block, plural);
          }
        }
        return value;
      }
    }

  }

}
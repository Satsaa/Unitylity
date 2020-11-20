#if UNITY_EDITOR
namespace Muc.Editor {

  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Muc.Extensions;
  using Muc.Numerics;
  using UnityEditor;
  using UnityEngine;


  public static partial class EditorUtil {

    private class TypeNamespaceComparer : IComparer<Type> {
      public int Compare(Type x, Type y) {
        if (x == null || y == null) {
          if (x == y) return 0;
          if (x == null) return -1;
          return 1;
        }
        var xNs = x?.Namespace;
        var yNs = y?.Namespace;
        var xDepth = xNs == null ? 0 : xNs.Count(v => v == '.') + 1;
        var yDepth = yNs == null ? 0 : yNs.Count(v => v == '.') + 1;
        return yDepth.CompareTo(xDepth);
      }
    }

    private class TypeMenuNode {

      public string ns;
      public TypeMenuNode source;
      public List<Type> types = new List<Type>();
      public List<TypeMenuNode> branches = new List<TypeMenuNode>();

      public TypeMenuNode(string ns, TypeMenuNode source) {
        this.ns = ns;
        this.source = source;
      }

      public TypeMenuNode GetCreateBranch(string ns, TypeMenuNode source) {
        var res = branches.Find(v => v.ns == ns);
        if (res != null) return res;
        res = new TypeMenuNode(ns, source);
        branches.Add(res);
        return res;
      }
    }


    /// <summary>
    /// Creates a Type selection menu. Types are shown in a hierarchy if there are more than 100 types.
    /// </summary>
    /// <param name="baseType">Types that are either this type or a child of it are displayed.</param>
    /// <param name="onSelect">Callback when something is selected.</param>
    /// <returns>A GenericMenu</returns>
    public static GenericMenu TypeSelectMenu(Type baseType, Action<Type> onSelect) {
      return TypeSelectMenu(baseType, null, onSelect);
    }

    /// <summary>
    /// Creates a Type selection menu. Types are shown in a hierarchy if there are more than 100 types.
    /// </summary>
    /// <param name="baseType">Types that are either this type or a child of it are displayed.</param>
    /// <param name="selected">Marks these types as checked in the menu.</param>
    /// <param name="onSelect">Callback when something is selected.</param>
    /// <returns>A GenericMenu</returns>
    public static GenericMenu TypeSelectMenu(Type baseType, IEnumerable<Type> selected, Action<Type> onSelect) {
      var types = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(v => v.GetTypes())
        .Where(v =>
          (v.IsClass || v.IsInterface || v.IsValueType) &&
          (v == baseType || baseType.IsAssignableFrom(v))
        );
      return TypeSelectMenu(types.ToList(), selected, onSelect);
    }

    /// <summary>
    /// Creates a Type selection menu. Types are shown in a hierarchy if there are more than 100 types.
    /// </summary>
    /// <param name="types">The list of available Types.</param>
    /// <param name="onSelect">Callback when something is selected.</param>
    /// <returns>A GenericMenu</returns>
    public static GenericMenu TypeSelectMenu(List<Type> types, Action<Type> onSelect) {
      return TypeSelectMenu(types, null, onSelect);
    }

    /// <summary>
    /// Creates a Type selection menu. Types are shown in a hierarchy if there are more than 100 types.
    /// </summary>
    /// <param name="types">The list of available Types.</param>
    /// <param name="selected">Marks these types as checked in the menu.</param>
    /// <param name="onSelect">Callback when something is selected.</param>
    /// <returns>A GenericMenu</returns>
    public static GenericMenu TypeSelectMenu(List<Type> types, IEnumerable<Type> selected, Action<Type> onSelect) {

      types.RemoveAll(v => v.FullName.Contains("<PrivateImplementationDetails>"));
      types.Sort(new TypeNamespaceComparer());

      const int maxCount = 999;
      const int splitHierarchyLimit = 999;
      const int hierarchyLimit = 100;

      if (types.Count > splitHierarchyLimit) {
        var position = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);

        var root = new TypeMenuNode("Root", null);
        var current = root;
        var prevNs = "";
        var depth = 0;
        var tokens = new string[] { };
        foreach (var type in types) {
          var ns = type.Namespace;
          if (ns != prevNs) {
            prevNs = ns;
            current = root;
            tokens = ns == null ? new string[] { } : ns.Split('.');
            depth = ns == null ? 0 : tokens.Length;
            for (int i = 0; i < depth; i++) {
              current = current.GetCreateBranch(tokens[i], current);
            }
          }
          current.types.Add(type);
        }
        return NodeMenu(root);

        GenericMenu NodeMenu(TypeMenuNode node) {
          var count = 0;
          var menu = new GenericMenu();
          if (node.source != null) {
            if (count++ >= maxCount) return menu;
            menu.AddItem(new GUIContent("<-"), false, cb);
            void cb() { EditorApplication.delayCall = () => NodeMenu(node.source).DropDown(new Rect(GUIUtility.ScreenToGUIPoint(position), Vector2.one)); }
          }
          foreach (var branch in node.branches) {
            if (count++ >= maxCount) return menu;
            var content = new GUIContent($"{branch.ns} ->");
            menu.AddItem(content, false, cb);
            void cb() => EditorApplication.delayCall = () => NodeMenu(branch).DropDown(new Rect(GUIUtility.ScreenToGUIPoint(position), Vector2.one));
          }
          foreach (var type in node.types) {
            if (count++ >= maxCount) return menu;
            var content = new GUIContent($"{type} ({type.Assembly.GetName().Name})");
            menu.AddItem(content, selected != null && selected.Any(t => t == type), () => onSelect(type));
          }
          return menu;
        }

      } else if (types.Count > hierarchyLimit) {

        var menu = new GenericMenu();
        foreach (var type in types) {
          UnityEngine.GUIContent content = new GUIContent($"{type.ToString().Replace('.', '/')} ({type.Assembly.GetName().Name})");
          menu.AddItem(content, selected != null && selected.Any(t => t == type), () => onSelect(type));
        }
        return menu;

      } else {

        var menu = new GenericMenu();
        foreach (var type in types) {
          var content = new GUIContent($"{type} ({type.Assembly.GetName().Name})");
          menu.AddItem(content, selected != null && selected.Any(t => t == type), () => onSelect(type));
        }
        return menu;
      }

    }

  }
}
#endif
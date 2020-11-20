

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Reflection;
  using UnityEditor;


  internal static class ReorderableDrawerInjector {

    [InitializeOnLoadMethod]
    private static void InitializeOnLoad() {
      UnityEngine.Profiling.Profiler.BeginSample("ReorderableListDrawerInjector");

      // Generates errors on compile
      if (!drawerKeySetDictionary.Contains(typeof(List<>)))
        drawerKeySetDictionary.Add(typeof(List<>), drawerKeySet);

      ApplyToUnityObjectTypes();
      UnityEngine.Profiling.Profiler.EndSample();
    }

    private static void ApplyToUnityObjectTypes() {
      var objType = typeof(UnityEngine.Object);
      var objAssembly = objType.Assembly;
      var objAssemblyFullName = objAssembly.FullName;

      var visited = new HashSet<Type>();
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
        if (AssemblyDependsOn(assembly, objAssemblyFullName)) {
          foreach (var type in assembly.GetTypes()) {
            if (type.IsClass && !type.IsAbstract && objType.IsAssignableFrom(type)) {
              ApplyToArraysAndListsInType(visited, type);
            }
          }
        }
      }
    }

    //======================================================================

    private static bool AssemblyDependsOn(Assembly assembly, string dependencyFullName) {
      if (assembly.FullName == dependencyFullName) return true;
      return assembly.GetReferencedAssemblies().Any(v => v.FullName == dependencyFullName);
    }

    //======================================================================

    private static void ApplyToArraysAndListsInType(HashSet<Type> visited, Type type) {
      if (!visited.Add(type))
        return;

      if (type.IsArray) {
        if (!drawerKeySetDictionary.Contains(type))
          drawerKeySetDictionary.Add(type, drawerKeySet);

        ApplyToArraysAndListsInType(visited, type.GetElementType());
        return;
      }

      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) {
        if (!drawerKeySetDictionary.Contains(type))
          drawerKeySetDictionary.Add(type, drawerKeySet);

        ApplyToArraysAndListsInType(visited, type.GetGenericArguments()[0]);
        return;
      }

      const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      for (; type != null; type = type.BaseType) {
        foreach (var field in type.GetFields(bindingFlags)) {
          ApplyToArraysAndListsInType(visited, field.FieldType);
        }
      }
    }

    //======================================================================

    private static readonly object drawerKeySet = CreateDrawerKeySet();

    private static object CreateDrawerKeySet() {
      var DrawerKeySet = typeof(PropertyDrawer).Assembly.GetType("UnityEditor.ScriptAttributeUtility+DrawerKeySet");

      var DrawerKeySet_drawer = DrawerKeySet.GetField("drawer", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

      var DrawerKeySet_type = DrawerKeySet.GetField("type", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

      var drawerType = typeof(ReorderableDrawer);
      var drawerKeySet = Activator.CreateInstance(DrawerKeySet);
      DrawerKeySet_drawer.SetValue(drawerKeySet, drawerType);
      DrawerKeySet_type.SetValue(drawerKeySet, typeof(IList));

      return drawerKeySet;
    }

    //======================================================================

    private static readonly IDictionary drawerKeySetDictionary = GetDrawerKeySetDictionary();

    private static IDictionary GetDrawerKeySetDictionary() {
      var ScriptAttributeUtility = typeof(PropertyDrawer).Assembly.GetType("UnityEditor.ScriptAttributeUtility");

      // ensure initialization of
      // ScriptAttributeUtility.drawerTypeForType
      ScriptAttributeUtility
        .GetMethod("GetDrawerTypeForType", BindingFlags.NonPublic | BindingFlags.Static)
        .Invoke(null, new object[] { typeof(object) });

      return (IDictionary)ScriptAttributeUtility.GetField("s_DrawerTypeForType", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }

  }

}
#endif
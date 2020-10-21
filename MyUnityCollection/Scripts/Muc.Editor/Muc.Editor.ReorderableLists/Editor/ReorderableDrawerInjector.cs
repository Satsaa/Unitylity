

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
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
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        if (AssemblyDependsOn(assembly, objAssemblyFullName))
          foreach (var type in assembly.GetTypes())
            if (type.IsClass && type.IsAbstract == false && objType.IsAssignableFrom(type))
              ApplyToArraysAndListsInType(visited, type);
    }

    //----------------------------------------------------------------------

    private static bool AssemblyDependsOn(Assembly assembly, string dependencyFullName) {
      if (assembly.FullName == dependencyFullName)
        return true;

      foreach (var reference in assembly.GetReferencedAssemblies())
        if (reference.FullName == dependencyFullName)
          return true;

      return false;
    }

    //----------------------------------------------------------------------

    private static void ApplyToArraysAndListsInType(HashSet<Type> visited, Type type) {
      if (visited.Add(type) == false)
        return;

      if (type.IsArray) {
        if (drawerKeySetDictionary.Contains(type) == false)
          drawerKeySetDictionary.Add(type, drawerKeySet);

        ApplyToArraysAndListsInType(visited, type.GetElementType());
        return;
      }

      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) {
        if (drawerKeySetDictionary.Contains(type) == false)
          drawerKeySetDictionary.Add(type, drawerKeySet);

        ApplyToArraysAndListsInType(visited, type.GetGenericArguments()[0]);
        return;
      }

      const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      for (; type != null; type = type.BaseType)
        foreach (var field in type.GetFields(bindingFlags))
          ApplyToArraysAndListsInType(visited, field.FieldType);
    }

    //----------------------------------------------------------------------

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

    //----------------------------------------------------------------------

    private static readonly IDictionary drawerKeySetDictionary = GetDrawerKeySetDictionary();

    private static IDictionary GetDrawerKeySetDictionary() {
      var ScriptAttributeUtility = typeof(PropertyDrawer).Assembly.GetType("UnityEditor.ScriptAttributeUtility");

      // ensure initialization of
      // ScriptAttributeUtility.draw erTypeForType
      ScriptAttributeUtility
        .GetMethod("GetDrawerTypeForType", BindingFlags.NonPublic | BindingFlags.Static)
        .Invoke(null, new object[] { typeof(object) });

      return (IDictionary)ScriptAttributeUtility.GetField("s_DrawerTypeForType", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }

    //======================================================================

    private struct TimedScope : IDisposable {

      private DateTime timeEnded;

      //------------------------------------------------------------------

      private TimedScope(string description) {
        var stackTrace = new StackTrace(2);
        this.description = description ?? DefaultDescription(stackTrace);
        this.stackTrace = new StackTrace(2);
        timeEnded = default(DateTime);
        timeBegan = DateTime.Now;
      }

      //------------------------------------------------------------------

      private static string DefaultDescription(StackTrace stackTrace) {
        var frame = stackTrace.GetFrame(0);
        var method = frame.GetMethod();
        var className = method.DeclaringType.Name;
        return $"{className}.{method.Name}()";
      }

      //------------------------------------------------------------------

      public string description { get; private set; }

      public bool hasEnded { get => timeEnded != default(DateTime); }

      public StackTrace stackTrace { get; private set; }

      public DateTime timeBegan { get; private set; }

      public TimeSpan timeElapsed {
        get {
          var endTime = hasEnded ? timeEnded : DateTime.Now;
          return endTime - timeBegan;
        }
      }

      //------------------------------------------------------------------

      public static TimedScope Begin() {
        return new TimedScope(null);
      }

      public static TimedScope Begin(string description) {
        return new TimedScope(description);
      }

      public void End() {
        timeEnded = DateTime.Now;
        Log(this);
      }

      //------------------------------------------------------------------

      void IDisposable.Dispose() {
        End();
      }

      //------------------------------------------------------------------

      public static void Log(TimedScope timedScope) {
        UnityEngine.Debug.Log(ToString(timedScope));
      }

      //------------------------------------------------------------------

      public override string ToString() {
        return ToString(this);
      }

      public static string ToString(TimedScope timedScope) {
        return $"{timedScope.description} took {ToString(timedScope.timeElapsed)}\n{timedScope.stackTrace.ToString()}";
      }

      public static string ToString(TimeSpan timeSpan) {
        var period = 0.0;
        var unit = "";
        if ((period = timeSpan.TotalDays) > 1) {
          unit = "days";
        } else if ((period = timeSpan.TotalHours) > 1) {
          unit = "hours";
        } else if ((period = timeSpan.TotalMinutes) > 1) {
          unit = "minutes";
        } else if ((period = timeSpan.TotalSeconds) > 1) {
          unit = "seconds";
        } else {
          period = timeSpan.TotalMilliseconds;
          unit = "milliseconds";
        }

        return $"{(int)period} {unit}";

      }

    }

  }

}
#endif
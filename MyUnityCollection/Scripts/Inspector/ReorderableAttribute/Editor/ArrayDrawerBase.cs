

#if UNITY_EDITOR
namespace Muc.Inspector.Internal {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Reflection;
  using UnityEditor;
  using UnityEngine;


  public abstract class ArrayDrawerBase : DecoratorDrawer {

    internal ArrayDrawerBase() { }

    //----------------------------------------------------------------------

    public sealed override bool CanCacheInspectorGUI() {
      if (!didInjectArrayDrawer)
        InjectArrayDrawer();
      return false;
    }

    public sealed override float GetHeight() {
      if (!didInjectArrayDrawer)
        InjectArrayDrawer();
      return 0;
    }

    public sealed override void OnGUI(Rect position) { }

    //----------------------------------------------------------------------

    private bool didInjectArrayDrawer;

    private void InjectArrayDrawer() {
      didInjectArrayDrawer = true;

      var propertyHandler = GetPropertyHandler();

      var propertyDrawer = GetPropertyDrawer(propertyHandler);

      if (propertyDrawer == null) {
        propertyDrawer = new ArrayDrawerAdapter((ArrayDrawer)this);
        SetPropertyDrawer(propertyHandler, propertyDrawer);
      }
    }

    //======================================================================

    private static readonly PropertyInfo s_PropertyHandlerCache =
      typeof(DecoratorDrawer)
        .Assembly
        .GetType("UnityEditor.ScriptAttributeUtility")
        .GetProperty("propertyHandlerCache", BindingFlags.NonPublic | BindingFlags.Static);

    private static readonly FieldInfo s_PropertyHandlers =
      typeof(DecoratorDrawer)
        .Assembly
        .GetType("UnityEditor.PropertyHandlerCache")
        .GetField("m_PropertyHandlers", BindingFlags.NonPublic | BindingFlags.Instance);

    internal object GetPropertyHandler() {
      var propertyHandlerCache = s_PropertyHandlerCache.GetValue(null, null);

      var propertyHandlerDictionary = (IDictionary)s_PropertyHandlers.GetValue(propertyHandlerCache);

      var propertyHandlers = propertyHandlerDictionary.Values;

      foreach (var propertyHandler in propertyHandlers) {
        var decoratorDrawers = (List<DecoratorDrawer>)propertyHandler_DecoratorDrawers.GetValue(propertyHandler);

        if (decoratorDrawers == null)
          continue;

        var index = decoratorDrawers.IndexOf(this);
        if (index < 0)
          continue;

        return propertyHandler;
      }

      return null;
    }

    //======================================================================

    private static readonly Type propertyHandler = typeof(DecoratorDrawer).Assembly.GetType("UnityEditor.PropertyHandler");

    private static readonly FieldInfo propertyHandler_PropertyDrawer = propertyHandler.GetField("m_PropertyDrawer", BindingFlags.NonPublic | BindingFlags.Instance);

    private static readonly FieldInfo propertyHandler_DecoratorDrawers = propertyHandler.GetField("m_DecoratorDrawers", BindingFlags.NonPublic | BindingFlags.Instance);

    //----------------------------------------------------------------------

    internal static PropertyDrawer GetPropertyDrawer(object propertyHandler) {
      return (PropertyDrawer)propertyHandler_PropertyDrawer.GetValue(propertyHandler);
    }

    internal static void SetPropertyDrawer(object propertyHandler, PropertyDrawer propertyDrawer) {
      propertyHandler_PropertyDrawer.SetValue(propertyHandler, propertyDrawer);
    }

  }

}
#endif
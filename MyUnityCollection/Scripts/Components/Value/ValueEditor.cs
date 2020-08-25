
#if UNITY_EDITOR
namespace Muc.Components.Values {

  using System;
  using System.Linq;
  using System.Reflection;
  using System.Text.RegularExpressions;
  using System.Collections;
  using System.Collections.Generic;

  using UnityEngine;
  using UnityEngine.UIElements;
  using UnityEditor;
  using UnityEditorInternal;

  using Muc.Types.Extensions;

  [CustomEditor(typeof(Value<,>), true)]
  public class ValueDrawer : Editor {

    bool isArithmetic;

    MethodInfo addModifierMethod;

    List<object> modifiersValue => (List<object>)target.GetType().GetField(nameof(modifiers), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
    SerializedProperty modifiers;
    ReorderableList list;

    SerializedProperty valueData;

    Regex matcher = new Regex("(?<=<).+(?=>)", RegexOptions.Compiled);

    void OnEnable() {

      isArithmetic = target.GetType().BaseTypes().Any(t => t.GetGenericTypeDefinition() == typeof(ArithmeticValue<,>));

      // Find the first AddModifier method which takes a generic argument
      addModifierMethod = target.GetType().GetMethods().First(v => v.Name == nameof(Health.AddModifier) && v.ContainsGenericParameters);

      valueData = serializedObject.FindProperty(nameof(valueData));
      modifiers = serializedObject.FindProperty(nameof(modifiers));

      list = new ReorderableList(serializedObject, modifiers);

      list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => DrawElementCallback(list, rect, index, isActive, isFocused);
      list.drawHeaderCallback = (Rect rect) => DrawHeaderCallback(list, rect);
      list.elementHeightCallback = (int index) => ElementHeightCallback(list, index);
      list.onAddDropdownCallback = OnAddDropdownCallback; // Already passes the list as parameter

      list.draggable = false;
    }



    public override void OnInspectorGUI() {
      serializedObject.UpdateIfRequiredOrScript();

      DrawPropertiesExcluding(serializedObject, modifiers.name, "m_Script");

      list.DoLayoutList();

      serializedObject.ApplyModifiedProperties();
    }


    #region Callbacks

    protected void OnAddDropdownCallback(Rect buttonRect, ReorderableList list) {
      var menu = new GenericMenu();

      var vd = (ValueData)valueData.objectReferenceValue;
      if (vd is null) {
        menu.AddItem(new GUIContent($"Define {nameof(valueData)}"), false, () => { });
      } else {
        var modTypes = vd.GetModifiers(target.GetType());
        foreach (var modType in modTypes) {
          menu.AddItem(new GUIContent(modType.Name), false, () => {
            var method = addModifierMethod.MakeGenericMethod(modType);
            Undo.RegisterFullObjectHierarchyUndo(target, "Add Modifier");
            method.Invoke(target, null);
          });
        }
      }

      menu.ShowAsContext();
    }

    protected void DrawHeaderCallback(ReorderableList list, Rect rect) {
      EditorGUI.LabelField(rect, list.serializedProperty.displayName);
    }

    protected float ElementHeightCallback(ReorderableList list, int index) {
      var element = list.serializedProperty.GetArrayElementAtIndex(index);

      var height = EditorGUI.GetPropertyHeight(element, true);
      height += EditorGUIUtility.standardVerticalSpacing / 2;
      return height;
    }

    protected void DrawElementCallback(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused) {
      rect.xMin += 19;
      var element = list.serializedProperty.GetArrayElementAtIndex(index);
      var modifierName = matcher.Match(element.type).Groups[0].Value;

      var rawVal = modifiersValue[index];
      var hasGetHandler = rawVal.GetType().GetProperty(nameof(HealthModifier.onGet)).GetValue(rawVal) != null;
      var hasSetHandler = rawVal.GetType().GetProperty(nameof(HealthModifier.onSet)).GetValue(rawVal) != null;
      var hasAddHandler = rawVal.GetType().GetProperty(nameof(HealthModifier.onAdd)).GetValue(rawVal) != null;
      var hasSubHandler = rawVal.GetType().GetProperty(nameof(HealthModifier.onSub)).GetValue(rawVal) != null;

      var handlerStrings = new List<String>();
      var hasHandler = hasGetHandler || hasSetHandler || hasAddHandler || hasSubHandler;
      if (hasHandler) {
        if (hasGetHandler) handlerStrings.Add("Get");
        if (hasSetHandler) handlerStrings.Add("Set");
        if (hasAddHandler) handlerStrings.Add("Add");
        if (hasSubHandler) handlerStrings.Add("Sub");
        var handlerHint = $" ({String.Join(", ", handlerStrings)})";
      }
      var displayString = hasHandler ? $"{modifierName} ({String.Join(", ", handlerStrings)})" : modifierName;

      EditorGUI.PropertyField(rect, element, new GUIContent(displayString), true);
    }

    #endregion
  }
}
#endif
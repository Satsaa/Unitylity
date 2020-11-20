

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using UnityEditor;
  using UnityEditorInternal;
  using UnityEngine;

  using static EditorUtil;
  using static PropertyUtil;


  internal class ReorderableReferences : ReorderableStructures {


    public ReorderableReferences(SerializedProperty property) : base(property) {
      if (isReferenceList) onAddDropdownCallback = TypeSelectDropdown;
    }

    private void TypeSelectDropdown(Rect buttonRect, ReorderableList list) {
      var menu = TypeSelectMenu(elementBaseType, type => InsertElement(index, type));
      menu.DropDown(buttonRect);
    }


    //======================================================================

    protected override float GetElementHeight(SerializedProperty element, int elementIndex) {
      var types = GetValues<object>(element).Select(v => v.GetType());
      if (types.Any(t => GetDrawerTypeForPropertyAndType(element, t) != null)) {
        var propertyHeight = EditorGUI.GetPropertyHeight(element);
        if (elementIndex > 0) propertyHeight += extraSpacing;
        return propertyHeight;
      }
      return base.GetElementHeight(element, elementIndex);
    }

    //======================================================================

    protected override void DrawElement(Rect position, SerializedProperty element, int elementIndex, bool isActive) {
      var types = GetValues<object>(element).Select(v => v.GetType());
      if (types.Any(t => GetDrawerTypeForPropertyAndType(element, t) != null)) {
        if (elementIndex > 0) {
          DrawHorizontalLine(position);
          position.yMax -= extraSpacing;
        }
        EditorGUI.PropertyField(position, element, GUIContent.none);
      } else {
        base.DrawElement(position, element, elementIndex, isActive);
      }
    }

    //======================================================================


    private static Func<SerializedProperty, Type, Type> getDrawerTypeForPropertyAndType;
    private static Func<SerializedProperty, Type, Type> GetDrawerTypeForPropertyAndType {
      get {
        if (getDrawerTypeForPropertyAndType == null) {
          getDrawerTypeForPropertyAndType =
            (Func<SerializedProperty, Type, Type>)
            Delegate.CreateDelegate(
              typeof(Func<SerializedProperty, Type, Type>),
              null,
              typeof(PropertyDrawer)
                .Assembly
                .GetType("UnityEditor.ScriptAttributeUtility")
                .GetMethod("GetDrawerTypeForPropertyAndType", BindingFlags.NonPublic | BindingFlags.Static)
            );
        }
        return getDrawerTypeForPropertyAndType;
      }
    }

  }
}
#endif
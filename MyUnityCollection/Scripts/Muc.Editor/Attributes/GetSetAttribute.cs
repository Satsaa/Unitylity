

// Original: http://answers.unity.com/answers/1513032/view.html

namespace Muc.Editor {

  using UnityEngine;

  public class GetSetAttribute : PropertyAttribute {
    public readonly string target;
    public bool dirty;

    public GetSetAttribute(string target) {
      this.target = target;
    }
  }
}


#if UNITY_EDITOR
namespace Muc.Editor {

  using System.Reflection;
  using UnityEngine;
  using UnityEditor;

  [CustomPropertyDrawer(typeof(GetSetAttribute))]
  internal class GetSetDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      var attribute = (GetSetAttribute)base.attribute;

      EditorGUI.BeginChangeCheck();
      EditorGUI.PropertyField(position, property, label);

      if (EditorGUI.EndChangeCheck()) {
        attribute.dirty = true;
      } else if (attribute.dirty) {
        var parent = GetParentObject(property.propertyPath, property.serializedObject.targetObject);

        var type = parent.GetType();
        var info = type.GetProperty(attribute.target);

        if (info == null)
          Debug.LogError("Invalid property name \"" + attribute.target + "\"");
        else
          info.SetValue(parent, fieldInfo.GetValue(parent), null);

        attribute.dirty = false;
      }
    }

    public static object GetParentObject(string path, object obj) {
      var fields = path.Split('.');

      if (fields.Length == 1)
        return obj;

      FieldInfo info = obj.GetType().GetField(fields[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      obj = info.GetValue(obj);

      return GetParentObject(string.Join(".", fields, 1, fields.Length - 1), obj);
    }
  }

}
#endif
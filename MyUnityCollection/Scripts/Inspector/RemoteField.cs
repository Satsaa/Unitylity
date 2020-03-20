

namespace MUC.Inspector {

  using UnityEngine;

  [System.Serializable] public class RemoteInt : RemoteField<int> { };
  [System.Serializable] public class RemoteFloat : RemoteField<float> { };
  [System.Serializable] public class RemoteBool : RemoteField<bool> { };
  [System.Serializable] public class RemoteString : RemoteField<string> { };

  [System.Serializable]
  public class RemoteField<T> {

    public MonoBehaviour targetScript;
    public string fieldName;

    private System.Reflection.FieldInfo GetFieldInfo() {
      if (targetScript == null)
        return null;
      var type = targetScript.GetType();
      return type.GetField(fieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    }
    public T Value {
      get {
        var field = GetFieldInfo();
        if (field != null) {
          var val = field.GetValue(targetScript);
          return (T)val;
        }
        return default(T);
      }
      set {
        var field = GetFieldInfo();
        if (field != null) {
          field.SetValue(targetScript, value);
        }
      }
    }
  }

}


#if UNITY_EDITOR
namespace MUC.Inspector.Internal {

  using System.Collections.Generic;
  using UnityEngine;
  using UnityEditor;

  [CustomPropertyDrawer(typeof(RemoteFloat))]
  internal class RemoteFloatDrawer : PropertyDrawer {

    bool isOut;
    List<string> options;
    int selectedIndex;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      isOut = EditorGUILayout.Foldout(isOut, label);
      if (isOut) {
        EditorGUILayout.PropertyField(property.FindPropertyRelative("targetScript"));
        object targetScript = property.FindPropertyRelative("targetScript").objectReferenceValue;
        if (targetScript != null) {
          options = new List<string>();
          System.Reflection.FieldInfo[] fields = targetScript.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
          foreach (System.Reflection.FieldInfo field in fields) {
            if (field.FieldType == typeof(float)) {
              options.Add(field.Name);
            }
          }
          EditorGUILayout.BeginHorizontal();
          EditorGUILayout.LabelField("Float");
          selectedIndex = options.IndexOf(property.FindPropertyRelative("fieldName").stringValue);
          if (selectedIndex == -1) selectedIndex = 0;
          selectedIndex = EditorGUILayout.Popup(selectedIndex, options.ToArray());
          EditorGUILayout.EndHorizontal();
          property.FindPropertyRelative("fieldName").stringValue = options[selectedIndex];
        }
      }
    }
  }
#endif

}



namespace Muc.Systems.Values {

  using UnityEngine;

  [RequireComponent(typeof(Health))]
  public class HealthTestFunctions : MonoBehaviour {

    public Health health;

    [SerializeField]
    private float addVal;

    [SerializeField]
    private float subVal;

    [SerializeField]
    private float setVal;

    [SerializeField]
    private float getVal;

    public void OnValidate() {
      health = GetComponent<Health>();
    }

  }
}

#if UNITY_EDITOR
namespace Muc.Systems.Values {
  using System;
  using System.Linq;
  using System.Reflection;
  using System.Text.RegularExpressions;

  using UnityEngine;
  using UnityEngine.UIElements;
  using UnityEditor;
  using UnityEditorInternal;

  [CustomEditor(typeof(HealthTestFunctions), true)]
  public class HealthTestFunctionsDrawer : Editor {

    SerializedProperty addVal;
    SerializedProperty subVal;
    SerializedProperty setVal;
    SerializedProperty getVal;

    HealthTestFunctions t => (HealthTestFunctions)target;
    Health health => t.health;

    void OnEnable() {
      addVal = serializedObject.FindProperty(nameof(addVal));
      subVal = serializedObject.FindProperty(nameof(subVal));
      setVal = serializedObject.FindProperty(nameof(setVal));
      getVal = serializedObject.FindProperty(nameof(getVal));
    }

    public override void OnInspectorGUI() {
      serializedObject.UpdateIfRequiredOrScript();

      float width = 150;


      using (new EditorGUILayout.HorizontalScope()) {
        if (GUILayout.Button("Add to health", GUILayout.Width(width)))
          health.Add(addVal.floatValue);
        addVal.floatValue = EditorGUILayout.FloatField("", addVal.floatValue);
      }

      using (new EditorGUILayout.HorizontalScope()) {
        if (GUILayout.Button("Subtract from health", GUILayout.Width(width)))
          health.Sub(subVal.floatValue);
        subVal.floatValue = EditorGUILayout.FloatField("", subVal.floatValue);
      }

      using (new EditorGUILayout.HorizontalScope()) {
        if (GUILayout.Button("Set health", GUILayout.Width(width)))
          health.Set(setVal.floatValue);
        setVal.floatValue = EditorGUILayout.FloatField("", setVal.floatValue);
      }

      using (new EditorGUILayout.HorizontalScope()) {
        if (GUILayout.Button("Get health", GUILayout.Width(width)))
          getVal.floatValue = health.Get();
        using (new EditorGUI.DisabledGroupScope(true))
          EditorGUILayout.FloatField("", getVal.floatValue);
      }

      serializedObject.ApplyModifiedProperties();
    }

  }
}
#endif
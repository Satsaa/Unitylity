
namespace MUC.Inspector {

  using System;

  /// <summary>
  /// Attribute to create a button in the inspector for calling the method it is attached to.
  /// The method must have no arguments.
  /// </summary>
  /// <example>
  /// [Button]
  /// public void MyMethod()
  /// {
  ///     Debug.Log("Clicked!");
  /// }
  /// </example>
  [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public sealed class ButtonAttribute : Attribute {
    public object[] parameters;

    public ButtonAttribute(params object[] parameters) {
      this.parameters = parameters;
    }
  }

}


#if UNITY_EDITOR
namespace MUC.Inspector.Internal {

  using System;
  using System.Linq;
  using System.Reflection;
  using System.ComponentModel;
  using UnityEditor;
  using UnityEngine;

  /// <summary>
  /// Custom inspector for Object including derived classes.
  /// </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof(UnityEngine.Object), true)]
  public class ObjectEditor : Editor {
    public override void OnInspectorGUI() {

      // Loop through all methods with required amount of parameters
      var methods = this.target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

      foreach (var method in methods) {
        // Get the ButtonAttribute on the method (if any)
        var attribute = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));


        if (attribute != null) {

          var inputParams = attribute.parameters;
          var niceName = ObjectNames.NicifyVariableName(method.Name);

          // Save previous state
          var wasEnabled = GUI.enabled;

          if (ValidateParams(ref inputParams, method, out var message)) {

            GUI.enabled = true;

            // Draw a button which invokes the method
            if (GUILayout.Button(niceName)) {
              foreach (var t in this.targets) {

                var returnType = method.ReturnType;
                var res = method.Invoke(t, inputParams);

                var paramStrings = String.Join(", ", inputParams.Select(v => v.ToString()));

                Debug.Log($"{method.Name}({paramStrings}) => {(returnType == typeof(void) ? "void" : res.ToString())}");
              }
            }


          } else {
            EditorGUILayout.HelpBox(message, MessageType.Error);

            GUI.enabled = false;
            GUILayout.Button(niceName);
          }

          GUI.enabled = wasEnabled;

        }

      }
      // Draw the rest of the inspector as usual
      DrawDefaultInspector();
    }

    public bool ValidateParams(ref object[] inputValues, MethodInfo method, out string message) {

      var methodParams = method.GetParameters();

      if (methodParams.Length < inputValues.Length) {
        message = "Too many parameters";
        return false;
      }

      for (int i = 0; i < methodParams.Length; i++) {
        var methodParam = methodParams[i];


        // Missing parameter?
        if (i > inputValues.Length - 1) {
          if (methodParam.IsOptional) {
            // Replace with default value
            Array.Resize(ref inputValues, inputValues.Length + 1);
            inputValues[inputValues.Length - 1] = methodParam.DefaultValue;
          } else {
            message = $"Parameter \"{methodParam.Name}\" is required";
            return false;
          }
        }

        var inputValue = inputValues[i];

        var inputValueType = inputValue.GetType();
        var methodParamType = methodParam.ParameterType;

        // Checks to see if the value passed is valid.
        if (!TypeDescriptor.GetConverter(inputValueType).CanConvertTo(methodParamType)) {
          message = $"Value defined for parameter \"{methodParam.Name}\" is not compatible with type {methodParam.ParameterType.Name}";
          return false;
        }

      }

      message = "Valid!";
      return true;
    }
  }

}
#endif
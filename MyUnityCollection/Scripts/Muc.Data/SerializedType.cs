
namespace Muc.Data {

  using System;
  using System.Collections.Generic;
  using UnityEngine;
  using System.Linq;

  [Serializable]
  public class SerializedType {

    public static implicit operator Type(SerializedType t) => t?.type;

    [NonSerialized]
    protected bool updated = false;

    [HideInInspector, SerializeField]
    protected string _name;
    public string name {
      get {
        return _name;
      }
      set {
        _name = value;
        Update();
      }
    }

    protected Type _type;
    public Type type {
      get {
        if (!updated) Update();
        return _type;
      }
      set {
        _type = value;
        _name = _type?.AssemblyQualifiedName;
      }
    }

    protected virtual void Update() {
      if (_name == null) _type = null;
      else _type = Type.GetType(_name);
      updated = true;
    }

    public virtual IEnumerable<Type> GetValidTypes() {
      return AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(v => v.GetTypes())
        .Where(v => v.IsClass || v.IsInterface || v.IsValueType);
    }

  }

  [Serializable]
  public class SerializedType<T> : SerializedType {

    public static implicit operator Type(SerializedType<T> t) => t?.type;

    protected override void Update() {
      var newtype = Type.GetType(_name ?? "");
      if (newtype != null) _type = typeof(T).IsAssignableFrom(newtype) ? newtype : null;
      updated = true;
    }

    public override IEnumerable<Type> GetValidTypes() {
      return AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(v => v.GetTypes())
        .Where(v =>
          (v.IsClass || v.IsInterface || v.IsValueType) &&
          (v == typeof(T) || typeof(T).IsAssignableFrom(v))
        );
    }
  }

}


#if UNITY_EDITOR
namespace Muc.Data {

  using System;
  using System.Linq;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEditor;
  using static Muc.Editor.PropertyUtil;
  using static Muc.Editor.EditorUtil;

  [CustomPropertyDrawer(typeof(SerializedType), true)]
  public class SerializedTypeDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

      var noLabel = label.text is "" && label.image is null;

      if (property.serializedObject.isEditingMultipleObjects) {
        var nameProperty = property.FindPropertyRelative("_name");
        using (DisabledScope(v => true))
        using (PropertyScope(position, label, property, out label)) {
          EditorGUI.TextField(position, label, nameProperty.stringValue);
        }
        return;
      }

      var value = (SerializedType)GetValue(property);

      using (PropertyScope(position, label, property, out label)) {
        var hint = new GUIContent(label);
        hint.text = value.type == null ? "null" : $"{value.type.ToString()} ({value.type.Assembly.GetName().Name})";
        if (!noLabel) {
          EditorGUI.LabelField(position, label);
          position.xMin += EditorGUIUtility.labelWidth + spacing;
        }
        if (EditorGUI.DropdownButton(position, hint, FocusType.Keyboard)) {
          var types = value.GetValidTypes();
          var menu = BuildMenu(property, types.ToList(), value);
          menu.DropDown(position);
        }
      }
    }

    private static GenericMenu BuildMenu(SerializedProperty property, List<Type> types, SerializedType target) {
      var menu = new GenericMenu();

      if (types.Count > 50) {

        // Ensure "No Namespace" comes first if it is used.
        var firstNoNamespace = types.FindIndex(v => v.Namespace == null && !v.FullName.Contains("<PrivateImplementationDetails>"));
        if (firstNoNamespace != -1) {
          var type = types[firstNoNamespace];
          var content = new GUIContent($"No Namespace/{type.ToString().Replace('.', '/')} ({type.Assembly.GetName().Name})");
          menu.AddItem(content, type == (Type)target, () => { OnSelect(property, type); });
        }

        for (int i = 0; i < types.Count; i++) {
          if (i == firstNoNamespace) continue;
          var type = types[i];
          if (type.FullName.Contains("<PrivateImplementationDetails>")) continue;
          UnityEngine.GUIContent content;
          if (type.Namespace == null) {
            content = new GUIContent($"No Namespace/{type.ToString().Replace('.', '/')} ({type.Assembly.GetName().Name})");
          } else {
            content = new GUIContent($"{type.ToString().Replace('.', '/')} ({type.Assembly.GetName().Name})");
          }
          menu.AddItem(content, type == (Type)target, () => { OnSelect(property, type); });
        }

      } else {

        foreach (var type in types) {
          var content = new GUIContent($"{type.ToString()} ({type.Assembly.GetName().Name})");
          menu.AddItem(content, type == (Type)target, () => { OnSelect(property, type); });
        }

      }

      return menu;
    }

    private static void OnSelect(SerializedProperty property, Type type) {
      var value = (SerializedType)GetValue(property);
      value.type = type;
      EditorUtility.SetDirty(property.serializedObject.targetObject);
      property.serializedObject.ApplyModifiedProperties();
    }

  }
}
#endif

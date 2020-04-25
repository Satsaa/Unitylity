
// Original: https://github.com/Deadcows/MyBox


namespace Muc.Inspector {

  using System;

  /// <summary>
  /// Conditionally Show/Hide field in inspector, based on some other field value 
  /// </summary>
  [AttributeUsage(AttributeTargets.Field)]
  public class DrawIfAttribute : Internal.DrawIfBaseAttribute {
    /// <param name="fieldToCheck">String name of the field</param>
    /// <param name="compareValues">On which values field will be shown in inspector</param>
    public DrawIfAttribute(string fieldToCheck, params object[] compareValues) : base(fieldToCheck, false, compareValues) { }
  }

  /// <summary>
  /// Conditionally Show/Hide field in inspector, based on some other field value 
  /// </summary>
  [AttributeUsage(AttributeTargets.Field)]
  public class DrawIfNotAttribute : Internal.DrawIfBaseAttribute {
    /// <param name="fieldToCheck">String name of the field</param>
    /// <param name="compareValues">On which values field will NOT be shown in inspector</param>
    public DrawIfNotAttribute(string fieldToCheck, params object[] compareValues) : base(fieldToCheck, true, compareValues) { }
  }

}


namespace Muc.Inspector.Internal {

  using System;
  using System.Linq;
  using UnityEngine;

  [AttributeUsage(AttributeTargets.Field)]
  public class DrawIfBaseAttribute : PropertyAttribute {
    public readonly string fieldToCheck;
    public readonly string[] compareValues;
    public readonly bool inverse;

    public DrawIfBaseAttribute(string fieldToCheck, bool inverse = false, params object[] compareValues) {
      this.fieldToCheck = fieldToCheck;
      this.inverse = inverse;
      this.compareValues = compareValues.Select(c => c.ToString().ToUpper()).ToArray();
    }
  }
}


#if UNITY_EDITOR
namespace Muc.Inspector.Internal {

  using System;
  using System.Linq;
  using System.Collections.Generic;
  using UnityEngine;
  using System.Reflection;
  using System.Collections.ObjectModel;
  using UnityEditor;


  [CustomPropertyDrawer(typeof(DrawIfAttribute))]
  public class DrawIfAttributeDrawer : PropertyDrawer {
    private DrawIfBaseAttribute instance;

    private bool customDrawersCached;
    private static IEnumerable<Type> typesCache;
    private bool multipleAttributes;
    private bool specialType;
    private PropertyAttribute genericAttribute;
    private PropertyDrawer genericDrawerInstance;
    private Type genericDrawerType;
    private Type genericType;
    private PropertyDrawer genericTypeDrawerInstance;
    private Type genericTypeDrawerType;


    /// <summary>
    /// If conditional is part of type in collection, we need to link properties as in collection
    /// </summary>
    private readonly Dictionary<SerializedProperty, SerializedProperty> conditionalToTarget = new Dictionary<SerializedProperty, SerializedProperty>();
    private bool toShow = true;


    private void Initialize(SerializedProperty property) {
      if (instance == null) instance = base.attribute as DrawIfBaseAttribute;
      if (instance == null) return;

      if (!conditionalToTarget.ContainsKey(property))
        conditionalToTarget.Add(property, ConditionalFieldUtility.FindRelativeProperty(property, instance.fieldToCheck));


      if (customDrawersCached) return;
      if (typesCache == null) {
        typesCache = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => typeof(PropertyDrawer).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
      }

      if (HaveMultipleAttributes()) {
        multipleAttributes = true;
        GetPropertyDrawerType(property);
      } else if (!fieldInfo.FieldType.Module.ScopeName.Equals(typeof(int).Module.ScopeName)) {
        specialType = true;
        GetTypeDrawerType(property);
      }

      customDrawersCached = true;
    }

    private bool HaveMultipleAttributes() {
      if (fieldInfo == null) return false;
      var genericAttributeType = typeof(PropertyAttribute);
      var attributes = fieldInfo.GetCustomAttributes(genericAttributeType, false);
      if (attributes == null || attributes.Length == 0) return false;
      return attributes.Length > 1;
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      Initialize(property);

      toShow = ConditionalFieldUtility.PropertyIsVisible(conditionalToTarget[property], instance.inverse, instance.compareValues);
      if (!toShow) return 0;

      if (genericDrawerInstance != null)
        return genericDrawerInstance.GetPropertyHeight(property, label);
      if (genericTypeDrawerInstance != null)
        return genericTypeDrawerInstance.GetPropertyHeight(property, label);
      return EditorGUI.GetPropertyHeight(property);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      if (!toShow) return;

      if (multipleAttributes && genericDrawerInstance != null) {
        try {
          genericDrawerInstance.OnGUI(position, property, label);
        } catch (Exception e) {
          EditorGUI.PropertyField(position, property, label);
          LogWarning("Unable to instantiate " + genericAttribute.GetType() + " : " + e, property);
        }
      } else if (specialType && genericTypeDrawerInstance != null) {
        try {
          genericTypeDrawerInstance.OnGUI(position, property, label);
        } catch (Exception e) {
          EditorGUI.PropertyField(position, property, label);
          LogWarning("Unable to instantiate " + genericType + " : " + e, property);
        }
      } else {
        EditorGUI.PropertyField(position, property, label, true);
      }
    }

    private void LogWarning(string log, SerializedProperty property) {
      var warning = "Property <color=brown>" + fieldInfo.Name + "</color>";
      if (fieldInfo.DeclaringType != null) warning += " on behaviour <color=brown>" + fieldInfo.DeclaringType.Name + "</color>";
      warning += " caused: " + log;

      Debug.Log(warning, property.serializedObject.targetObject);
    }


    #region Get Custom Property/Type drawers

    private void GetPropertyDrawerType(SerializedProperty property) {
      if (genericDrawerInstance != null) return;

      //Get the second attribute flag
      try {
        genericAttribute = (PropertyAttribute)fieldInfo.GetCustomAttributes(typeof(PropertyAttribute), false)[1];

        if (genericAttribute is ContextMenuItemAttribute) {
          LogWarning("[ConditionalField] does not work with " + genericAttribute.GetType(), property);
          return;
        }

        if (genericAttribute is TooltipAttribute) return;
      } catch (Exception e) {
        LogWarning("Can't find stacked propertyAttribute after ConditionalProperty: " + e, property);
        return;
      }

      //Get the associated attribute drawer
      try {
        genericDrawerType = typesCache.First(x =>
            (Type)CustomAttributeData.GetCustomAttributes(x).First().ConstructorArguments.First().Value == genericAttribute.GetType());
      } catch (Exception e) {
        LogWarning("Can't find property drawer from CustomPropertyAttribute of " + genericAttribute.GetType() + " : " + e, property);
        return;
      }

      //Create instances of each (including the arguments)
      try {
        genericDrawerInstance = (PropertyDrawer)Activator.CreateInstance(genericDrawerType);
        //Get arguments
        IList<CustomAttributeTypedArgument> attributeParams = fieldInfo.GetCustomAttributesData()[1].ConstructorArguments;
        IList<CustomAttributeTypedArgument> unpackedParams = new List<CustomAttributeTypedArgument>();
        //Unpack any params object[] args
        foreach (CustomAttributeTypedArgument singleParam in attributeParams) {
          if (singleParam.Value.GetType() == typeof(ReadOnlyCollection<CustomAttributeTypedArgument>)) {
            foreach (CustomAttributeTypedArgument unpackedSingleParam in (ReadOnlyCollection<CustomAttributeTypedArgument>)singleParam
                .Value) {
              unpackedParams.Add(unpackedSingleParam);
            }
          } else {
            unpackedParams.Add(singleParam);
          }
        }

        object[] attributeParamsObj = unpackedParams.Select(x => x.Value).ToArray();

        if (attributeParamsObj.Any()) {
          genericAttribute = (PropertyAttribute)Activator.CreateInstance(genericAttribute.GetType(), attributeParamsObj);
        } else {
          genericAttribute = (PropertyAttribute)Activator.CreateInstance(genericAttribute.GetType());
        }
      } catch (Exception e) {
        LogWarning("No constructor available in " + genericAttribute.GetType() + " : " + e, property);
        return;
      }

      //Reassign the attribute field in the drawer so it can access the argument values
      try {
        genericDrawerType.GetField("m_Attribute", BindingFlags.Instance | BindingFlags.NonPublic)
            .SetValue(genericDrawerInstance, genericAttribute);
      } catch (Exception e) {
        LogWarning("Unable to assign attribute to " + genericDrawerInstance.GetType() + " : " + e, property);
      }
    }


    private void GetTypeDrawerType(SerializedProperty property) {
      if (genericTypeDrawerInstance != null) return;

      //Get the type
      genericType = fieldInfo.FieldType;

      var _genericObject = fieldInfo;

      //Get the associated attribute drawer
      try {
        genericTypeDrawerType = typesCache.First(x =>
            (Type)CustomAttributeData.GetCustomAttributes(x).First().ConstructorArguments.First().Value == genericType);
      } catch (Exception) {
        // Commented out because of multiple false warnings on Behaviour types
        //LogWarning("[ConditionalField] does not work with "+_genericType+". Unable to find property drawer from the Type", property);
        return;
      }

      //Create instances of each (including the arguments)
      try {
        genericTypeDrawerInstance = (PropertyDrawer)Activator.CreateInstance(genericTypeDrawerType);
      } catch (Exception e) {
        LogWarning("no constructor available in " + genericType + " : " + e, property);
        return;
      }

      //Reassign the attribute field in the drawer so it can access the argument values
      try {
        genericTypeDrawerType.GetField("m_Attribute", BindingFlags.Instance | BindingFlags.NonPublic)
            .SetValue(genericTypeDrawerInstance, _genericObject);
      } catch (Exception) {
        //LogWarning("Unable to assign attribute to " + _genericTypeDrawerInstance.GetType() + " : " + e, property);
      }
    }

    #endregion
  }

  public static class ConditionalFieldUtility {
    #region Property Is Visible 

    public static bool PropertyIsVisible(SerializedProperty property, bool inverse, string[] compareAgainst) {
      if (property == null) return true;

      string asString = SerializedPropertyAsStringValue(property).ToUpper();

      if (compareAgainst != null && compareAgainst.Length > 0) {
        var matchAny = CompareAgainstValues(asString, compareAgainst);
        if (inverse) matchAny = !matchAny;
        return matchAny;
      }

      bool someValueAssigned = asString != "FALSE" && asString != "0" && asString != "NULL";
      if (someValueAssigned) return !inverse;

      return inverse;
    }

    /// <summary>
    /// True if the property value matches any of the values in '_compareValues'
    /// </summary>
    private static bool CompareAgainstValues(string propertyValueAsString, string[] compareAgainst) {
      for (var i = 0; i < compareAgainst.Length; i++) {
        bool valueMatches = compareAgainst[i] == propertyValueAsString;

        // One of the value is equals to the property value.
        if (valueMatches) return true;
      }

      // None of the value is equals to the property value.
      return false;
    }

    #endregion


    #region Find Relative Property

    public static SerializedProperty FindRelativeProperty(SerializedProperty property, string propertyName) {
      if (property.depth == 0) return property.serializedObject.FindProperty(propertyName);

      var path = property.propertyPath.Replace(".Array.data[", "[");
      var elements = path.Split('.');

      var nestedProperty = NestedPropertyOrigin(property, elements);

      // if nested property is null = we hit an array property
      if (nestedProperty == null) {
        var cleanPath = path.Substring(0, path.IndexOf('['));
        var arrayProp = property.serializedObject.FindProperty(cleanPath);
        var target = arrayProp.serializedObject.targetObject;

        var who = "Property <color=brown>" + arrayProp.name + "</color> in object <color=brown>" + target.name + "</color> caused: ";
        var warning = who + "Array fields are not supported by " + nameof(DrawIfAttribute);

        Debug.Log(warning, target);

        return null;
      }

      return nestedProperty.FindPropertyRelative(propertyName);
    }

    // For [Serialized] types with [Conditional] fields
    private static SerializedProperty NestedPropertyOrigin(SerializedProperty property, string[] elements) {
      SerializedProperty parent = null;

      for (int i = 0; i < elements.Length - 1; i++) {
        var element = elements[i];
        int index = -1;
        if (element.Contains("[")) {
          index = Convert.ToInt32(element.Substring(element.IndexOf("[", StringComparison.Ordinal))
              .Replace("[", "").Replace("]", ""));
          element = element.Substring(0, element.IndexOf("[", StringComparison.Ordinal));
        }

        parent = i == 0
            ? property.serializedObject.FindProperty(element)
            : parent != null
                ? parent.FindPropertyRelative(element)
                : null;

        if (index >= 0 && parent != null) parent = parent.GetArrayElementAtIndex(index);
      }

      return parent;
    }

    #endregion


    #region  SerializedPropertyAsStringValue

    private static string SerializedPropertyAsStringValue(SerializedProperty prop) {
      switch (prop.propertyType) {
        case SerializedPropertyType.String:
          return prop.stringValue;

        case SerializedPropertyType.Character:
        case SerializedPropertyType.Integer:
          if (prop.type == "char") return Convert.ToChar(prop.intValue).ToString();
          return prop.intValue.ToString();

        case SerializedPropertyType.ObjectReference:
          return prop.objectReferenceValue != null ? prop.objectReferenceValue.ToString() : "null";

        case SerializedPropertyType.Boolean:
          return prop.boolValue.ToString();

        case SerializedPropertyType.Enum:
          return prop.enumNames[prop.enumValueIndex];

        default:
          return string.Empty;
      }
    }

    #endregion


    #region Behaviour Property Is Visible

    public static bool BehaviourPropertyIsVisible(MonoBehaviour behaviour, string propertyName, DrawIfBaseAttribute appliedAttribute) {
      if (string.IsNullOrEmpty(appliedAttribute.fieldToCheck)) return true;

      var so = new SerializedObject(behaviour);
      var property = so.FindProperty(propertyName);
      var targetProperty = ConditionalFieldUtility.FindRelativeProperty(property, appliedAttribute.fieldToCheck);

      return ConditionalFieldUtility.PropertyIsVisible(targetProperty, appliedAttribute.inverse, appliedAttribute.compareValues);
    }

    #endregion
  }
}
#endif

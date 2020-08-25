

#if UNITY_EDITOR
namespace Muc.Inspector.Internal {

  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using UnityEditor;
  using UnityEditorInternal;
  using UnityEngine;

  using Object = UnityEngine.Object;
  using ParallelListLayout = ReorderableAttribute.ParallelListLayout;
  using BackgroundColorDelegate = ReorderableDrawer.BackgroundColorDelegate;
  using System.Collections;
  using System.Text.RegularExpressions;

  internal class ReorderableValues : ReorderableList {

    public DateTime lastRendered { get; private set; } = DateTime.MaxValue;

    public readonly Type listType;

    public readonly Type elementType;

    public string elementHeaderFormat;

    public bool hasElementHeaderFormat {
      get => elementHeaderFormat != null;
    }

    public string singularListHeaderFormat;

    public string pluralListHeaderFormat;

    public virtual bool showElementHeader {
      get => false;
    }

    public readonly bool showFooterButtons;

    public readonly bool isReferenceList;

    public Color backgroundColor;

    internal BackgroundColorDelegate onBackgroundColor;

    public readonly SerializedProperty[] serializedProperties;

    public readonly ParallelListLayout parallelListLayout;

    protected static readonly new Defaults defaultBehaviours = new Defaults();

    protected readonly GUIContent titleContent = new GUIContent();

    //----------------------------------------------------------------------

    public ReorderableValues(ReorderableAttribute attribute, SerializedProperty primaryProperty, Type listType, Type elementType)
      : base(primaryProperty.serializedObject, primaryProperty.Copy(), !attribute.disableDragging, true, !attribute.disableAdding, !attribute.disableRemoving) {

      this.listType = listType;
      this.elementType = elementType;
      this.elementHeaderFormat = attribute.elementHeaderFormat;
      this.showFooterButtons = (displayAdd || displayRemove) && !attribute.hideFooterButtons;
      this.singularListHeaderFormat = attribute.singularListHeaderFormat ?? "{0} ({1})";
      this.pluralListHeaderFormat = attribute.pluralListHeaderFormat ?? "{0} ({1})";
      this.backgroundColor = new Color(attribute.r, attribute.g, attribute.b);
      this.serializedProperties = AcquireSerializedProperties(this.serializedProperty, attribute.parallelListNames);
      this.parallelListLayout = attribute.parallelListLayout;
      this.isReferenceList = primaryProperty.arrayElementType == "managedReference<>";

      headerHeight -= 2;
      drawHeaderCallback = DrawHeaderCallback;
      drawFooterCallback = DrawFooterCallback;
      elementHeightCallback = ElementHeightCallback;
      drawElementCallback = DrawElementCallback;
      drawElementBackgroundCallback = DrawElementBackgroundCallback;
      drawNoneElementCallback = DrawEmptyElementCallback;

      onAddCallback = OnAddCallback;
      onCanRemoveCallback = OnCanRemoveCallback;
      onRemoveCallback = OnRemoveCallback;

      onSelectCallback = OnSelectCallback;
      onReorderCallback = OnReorderCallback;
    }

    //----------------------------------------------------------------------

    private int dragIndex = 0;

    private void OnSelectCallback(ReorderableList list) {
      dragIndex = list.index;
    }

    private void OnReorderCallback(ReorderableList list) {
      var dragIndex = this.dragIndex;
      if (dragIndex < 0) return;

      var dropIndex = list.index;
      if (dropIndex < 0) return;

      try {
        for (int i = 1; i < serializedProperties.Length; ++i) {
          var array = serializedProperties[i];
          array.MoveArrayElement(dragIndex, dropIndex);
        }
      } catch (Exception ex) {
        Debug.LogException(ex);
      }
      GUI.changed = true;
    }

    //----------------------------------------------------------------------

    private static SerializedProperty[] AcquireSerializedProperties(SerializedProperty primaryProperty, string[] parallelListNames) {
      if (parallelListNames == null || parallelListNames.Length == 0)
        return new[] { primaryProperty };

      var serializedObject = primaryProperty.serializedObject;

      var serializedProperties = new List<SerializedProperty>(1 + parallelListNames.Length);
      serializedProperties.Add(primaryProperty);

      var primaryArraySize = primaryProperty.arraySize;

      var primaryPropertyPath = primaryProperty.propertyPath;
      var lastDotIndex = primaryPropertyPath.LastIndexOf('.');
      var parallelPropertyPrefix = primaryPropertyPath.Substring(0, lastDotIndex + 1);

      foreach (var parallelListName in parallelListNames) {
        var parallelPropertyPath = parallelPropertyPrefix + parallelListName;
        var parallelProperty = serializedObject.FindProperty(parallelPropertyPath);

        if (parallelProperty != null && parallelProperty.isArray) {
          ResizeArray(parallelProperty, primaryArraySize);
          serializedProperties.Add(parallelProperty);
        }
      }
      return serializedProperties.ToArray();
    }

    private static void ResizeArray(SerializedProperty property, int arraySize) {
      while (property.arraySize < arraySize) {
        property.InsertArrayElementAtIndex(property.arraySize);
      }
      while (property.arraySize > arraySize) {
        property.DeleteArrayElementAtIndex(property.arraySize - 1);
      }
    }

    //----------------------------------------------------------------------

    public float GetHeight(GUIContent label) {
      lastRendered = DateTime.Now;
      UpdateLabel(label);
      UpdateElementHeights();
      var height = GetHeight();

      if (!showFooterButtons) {
        height -= 14; // no add/remove buttons in footer
      }

      if (!serializedProperty.isExpanded) {
        var elementCount = elementHeights.Count;
        if (elementCount == 0)
          height -= 21; // no empty element
      }

      return height;
    }

    public virtual void DoGUI(Rect position) {
      if (onNextGUIFrame != null) onNextGUIFrame.Invoke();
      onNextGUIFrame = null;

      if (isReferenceList) displayAdd = this.count > 0;

      if (!displayAdd && !displayRemove && !draggable) {
        index = -1;
      }

      position = EditorGUI.IndentedRect(position);

      using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel)) {
        if (serializedProperty.isExpanded) {
          var listRect = new Rect(position);
          listRect.yMin++;
          DoList(listRect);
        } else {
          index = -1;
          DoCollapsedListBackground(position);
        }
        DrawHeader(position);
      }
    }

    //----------------------------------------------------------------------

    private void DoCollapsedListBackground(Rect position) {
      var headerRect = position;
      headerRect.height = headerHeight;

      var listRect = position;
      listRect.y += headerHeight;
      listRect.height = 9;

      var footerRect = position;
      footerRect.y += headerHeight + listRect.height;
      footerRect.height = footerHeight;

      if (showDefaultBackground && IsRepaint()) {
        defaultBehaviours.DrawHeaderBackground(headerRect);
        defaultBehaviours.boxBackground.Draw(listRect, false, false, false, false);
      }
      DrawFooterCallback(footerRect);
    }

    //----------------------------------------------------------------------

    private void OnAddCallback(ReorderableList list) {
      serializedProperty.isExpanded = true;
      InsertElement(serializedProperty.arraySize);
    }

    private bool OnCanRemoveCallback(ReorderableList list) {
      return serializedProperty.isExpanded;
    }

    private void OnRemoveCallback(ReorderableList list) {
      DeleteElement(index);
    }

    //----------------------------------------------------------------------

    [Serializable]
    private class ClipboardContent {
      public ClipboardElement[] elements;

      public ClipboardContent(int elementCount) {
        elements = new ClipboardElement[elementCount];
      }

      public static ClipboardContent Deserialize(string s) {
        try {
          return JsonUtility.FromJson<ClipboardContent>(s);
        } catch {
          return null;
        }
      }

      public string Serialize() {
        return JsonUtility.ToJson(this);
      }
    }

    [Serializable]
    private struct ClipboardElement {
      public string type;
      public string json;
    }

    private ClipboardContent CopyElementContent(int elementIndex) {
      if (elementIndex < 0) throw new IndexOutOfRangeException("Index must be non-negative.");

      var arrayIndex = 0;
      var arrayCount = serializedProperties.Length;
      var clipboardContent = new ClipboardContent(arrayCount);
      var serializedProperty = this.serializedProperty;
      var serializedObject = serializedProperty.serializedObject;
      foreach (var array in serializedProperties) {
        var arrayObj = (IList)array.GetObject();
        var elementObj = arrayObj[elementIndex];
        var elementType = elementObj.GetType();
        var elementJson = JsonUtility.ToJson(elementObj);
        var clipboardElement = new ClipboardElement();
        clipboardElement.type = elementType.FullName;
        clipboardElement.json = elementJson;
        clipboardContent.elements[arrayIndex] = clipboardElement;
        arrayIndex += 1;
      }
      return clipboardContent;
    }

    private void CopyElementToClipboard(int elementIndex) {
      if (elementIndex < 0) return;
      EditorGUIUtility.systemCopyBuffer = CopyElementContent(elementIndex).Serialize();
    }

    private void CutElement(int elementIndex) {
      if (elementIndex < 0) return;

      CopyElementToClipboard(elementIndex);
      DeleteElement(elementIndex);
    }

    private bool CanPaste(ClipboardContent clipboardContent) {
      if (clipboardContent == null) return false;

      var arrayIndex = 0;
      var arrayCount = serializedProperties.Length;
      var serializedProperty = this.serializedProperty;
      var serializedObject = serializedProperty.serializedObject;
      foreach (var array in serializedProperties) {
        var arrayObj = (IList)array.GetObject();
        var arrayType = arrayObj.GetType();
        var elementType =
          (arrayType.IsArray)
          ? arrayType.GetElementType()
          : arrayType.GetGenericArguments()[0];

        var clipboardElement = clipboardContent.elements[arrayIndex++];
        if (clipboardElement.type != elementType.FullName)
          return false;
      }
      return true;
    }

    private void PasteElement(int elementIndex, ClipboardContent clipboardContent) {
      if (elementIndex < 0) return;

      var clipboardElements = clipboardContent.elements;
      if (clipboardElements.Length == 0) return;

      var arrayIndex = 0;
      var arrayCount = serializedProperties.Length;
      var serializedProperty = this.serializedProperty;
      var serializedObject = serializedProperty.serializedObject;
      var targetObject = serializedObject.targetObject;
      Undo.RecordObject(targetObject, $"Paste {clipboardElements[0].type}");
      foreach (var array in serializedProperties) {
        if (elementIndex >= array.arraySize)
          array.arraySize = elementIndex + 1;

        var clipboardElement = clipboardContent.elements[arrayIndex++];
        var arrayObj = (IList)array.GetObject();
        var elementObj = arrayObj[elementIndex];
        var elementJson = clipboardElement.json;
        JsonUtility.FromJsonOverwrite(elementJson, elementObj);
      }
      serializedObject.Update();
      GUI.changed = true;
    }

    //----------------------------------------------------------------------

    protected static MemberInfo[] GetFirstMemberInHierarchy(Type type, string name, BindingFlags bindingAttr) {
      MemberInfo[] res;
      do {
        res = type.GetMember(name, bindingAttr);
        if (res.Length != 0) return res;
      } while ((type = type.BaseType) != null);
      return res;
    }

    private static MemberInfo[] GetFirstMemberInHierarchy(Type type, string name, MemberTypes memberTypes, BindingFlags bindingAttr) {
      MemberInfo[] res;
      do {
        res = type.GetMember(name, memberTypes, bindingAttr);
        if (res.Length != 0) return res;
      } while ((type = type.BaseType) != null);
      return res;
    }

    object GetMemberValue(object container, string name) {
      if (container == null) return null;
      var type = container.GetType();
      var members = GetFirstMemberInHierarchy(type, name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
      for (int i = 0; i < members.Length; ++i) {
        if (members[i] is FieldInfo field)
          return field.GetValue(container);
        else if (members[i] is PropertyInfo property)
          return property.GetValue(container);
      }
      return null;
    }

    void SetMemberValue(object container, string name, object value) {
      var type = container.GetType();
      var members = GetFirstMemberInHierarchy(type, name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
      for (int i = 0; i < members.Length; ++i) {
        if (members[i] is FieldInfo field) {
          field.SetValue(container, value);
          return;
        } else if (members[i] is PropertyInfo property) {
          property.SetValue(container, value);
          return;
        }
      }
    }

    object GetPathComponentValue(object container, PropertyPathComponent component) {
      if (component.propertyName == null)
        return ((IList)container)[component.elementIndex];
      else
        return GetMemberValue(container, component.propertyName);
    }

    void SetPathComponentValue(object container, PropertyPathComponent component, object value) {
      if (component.propertyName == null)
        ((IList)container)[component.elementIndex] = value;
      else
        SetMemberValue(container, component.propertyName, value);
    }

    Regex arrayElementRegex = new Regex(@"\GArray\.data\[(\d+)\]", RegexOptions.Compiled);

    struct PropertyPathComponent {
      public string propertyName;
      public int elementIndex;
    }

    bool NextPathComponent(string propertyPath, ref int index, out PropertyPathComponent component) {
      component = new PropertyPathComponent();

      if (index >= propertyPath.Length) return false;

      var arrayElementMatch = arrayElementRegex.Match(propertyPath, index);
      if (arrayElementMatch.Success) {
        index += arrayElementMatch.Length + 1; // Skip past next '.'
        component.elementIndex = int.Parse(arrayElementMatch.Groups[1].Value);
        return true;
      }

      int dot = propertyPath.IndexOf('.', index);
      if (dot == -1) {
        component.propertyName = propertyPath.Substring(index);
        index = propertyPath.Length;
      } else {
        component.propertyName = propertyPath.Substring(index, dot - index);
        index = dot + 1; // Skip past next '.'
      }

      return true;
    }

    public void SetValueNoRecord(SerializedProperty property, object value) {
      string propertyPath = property.propertyPath;
      object container = property.serializedObject.targetObject;

      int i = 0;
      NextPathComponent(propertyPath, ref i, out var deferredToken);
      while (NextPathComponent(propertyPath, ref i, out var token)) {
        container = GetPathComponentValue(container, deferredToken);
        deferredToken = token;
      }
      Debug.Assert(!container.GetType().IsValueType, $"Cannot use SerializedObject.SetValue on a struct object, as the result will be set on a temporary. Either change {container.GetType().Name} to a class, or use SetValue with a parent member.");
      SetPathComponentValue(container, deferredToken, value);
    }

    protected static Type GetManagedReferenceType(SerializedProperty property) {
      var typeStrings = property.managedReferenceFullTypename.Split(' ');
      var fullTypeName = typeStrings[1];
      var assemblyName = typeStrings[0];
      var assembly = Assembly.Load(assemblyName);
      var type = assembly.GetType(fullTypeName, true);
      return type;
    }

    protected virtual void InsertElement(int elementIndex) {
      if (elementIndex < 0) return;

      var serializedProperty = this.serializedProperty;
      var serializedObject = serializedProperty.serializedObject;
      foreach (var array in serializedProperties) {
        array.InsertArrayElementAtIndex(elementIndex);

        var type = array.arrayElementType;

        // Create first element with default values and 
        if (array.arraySize == 1) {
          var element = array.GetArrayElementAtIndex(elementIndex);
          var elPropType = element.propertyType;
          if (isReferenceList) {
            if (element.managedReferenceFieldTypename != "mscorlib System.Object") {

            }
          } else {

            switch (elPropType) {

              default:
                break;

              case SerializedPropertyType.Generic:

                var elementType = GetSerializedPropertyType(element);

                if (elementType != null) {
                  object instance = null;
                  try {
                    instance = Activator.CreateInstance(elementType, true);
                  } catch (Exception) {
                    instance = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(elementType);
                  } finally {
                    if (instance != null) {
                      serializedObject.ApplyModifiedProperties();
                      SetValueNoRecord(element, instance);
                    }
                  }
                }
                break;
            }
          }
        } else {
          if (isReferenceList) {
            var element = array.GetArrayElementAtIndex(elementIndex);

            var copyIndex = elementIndex == 0 ? 1 : elementIndex - 1;
            var copyElement = array.GetArrayElementAtIndex(copyIndex);
            var copyElementType = GetManagedReferenceType(copyElement);

            object instance = null;
            try {
              instance = Activator.CreateInstance(copyElementType, true);
            } catch (Exception) {
              instance = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(copyElementType);
            } finally {
              if (instance != null) {
                serializedObject.ApplyModifiedProperties();
                element.managedReferenceValue = instance;
                //SetValueNoRecord(element, instance);
              }
            }
          }
        }

      }
      serializedObject.ApplyModifiedProperties();
      index = elementIndex;
      GUI.changed = true;
    }

    public static Type GetSerializedPropertyType(SerializedProperty property) {
      var parentType = property.serializedObject.targetObject.GetType();
      return GetTypeByPath(parentType, property.propertyPath);
    }

    public static Type GetTypeByPath(Type type, string path) {
      path = path.Replace(".Array.data[", "[");
      var currentType = type;
      FieldInfo field = null;
      foreach (var token in path.Split('.')) {
        if (token.Contains("[")) {
          var elementName = token.Substring(0, token.IndexOf("["));
          var bracketPos = token.IndexOf("[");
          var index = System.Convert.ToInt32(token.Substring(bracketPos + 1, token.Length - (bracketPos + 2)));

          field = GetFirstMemberInHierarchy(currentType, elementName, MemberTypes.Field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault() as FieldInfo;

          if (field is null) return null;
          var listType = field.FieldType;
          if (listType.IsArray) {
            currentType = listType.GetElementType();
          } else {
            currentType = listType.GenericTypeArguments[0];
          }

        } else {
          field = currentType.GetField(token);

          if (field is null) return null;
          currentType = field.FieldType;
        }
      }
      if (field is null) return null;
      return currentType;
    }

    //----------------------------------------------------------------------

    protected virtual void DeleteElement(int elementIndex) {
      if (elementIndex < 0) return;

      var serializedProperty = this.serializedProperty;
      var serializedObject = serializedProperty.serializedObject;
      if (elementIndex < serializedProperty.arraySize) {
        foreach (var array in serializedProperties) {
          var element = array.GetArrayElementAtIndex(elementIndex);
          var oldSubassets = element.FindReferencedSubassets();
          var preDelSize = array.arraySize;
          array.DeleteArrayElementAtIndex(elementIndex);
          if (preDelSize == array.arraySize)
            array.DeleteArrayElementAtIndex(elementIndex);
          if (oldSubassets.Any()) {
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            serializedObject.DestroyUnreferencedSubassets(oldSubassets);
          } else {
            serializedObject.ApplyModifiedProperties();
          }
        }

        var length = serializedProperty.arraySize;
        if (index > length - 1)
          index = length - 1;
      }
      GUI.changed = true;
    }

    //----------------------------------------------------------------------

    protected virtual float GetElementHeight(SerializedProperty element, int elementIndex) {
      return GetPropertyHeight(element, GUIContent.none);
    }

    protected virtual void DrawElement(Rect position, SerializedProperty element, int elementIndex, bool isActive) {
      PropertyField(position, element, GUIContent.none);
    }

    //----------------------------------------------------------------------

    protected static readonly GUIStyle ElementBackgroundStyle = "RL Background";

    private void DrawElementBackground(Rect position, SerializedProperty element, int elementIndex, bool isActive, bool isFocused) {
      if (isActive) {
        var isProSkin = EditorGUIUtility.isProSkin;
        position.xMax += isProSkin ? 1 : 0;
        position.yMin -= isProSkin ? 0 : 1;
        position.yMax += isProSkin ? 2 : 1;
      }
      defaultBehaviours.DrawElementBackground(position, elementIndex, isActive, isFocused, draggable: true);

      if (IsRepaint() && element != null) {
        var fillStyle = ElementBackgroundStyle;
        var fillRect = position;
        fillRect.xMin += 2;
        fillRect.xMax -= 2;
        fillRect.yMin += 1;
        fillRect.yMax -= 1;

        var backgroundColor = GUI.color
          * ((this.backgroundColor == Color.black)
            ? GUI.backgroundColor
            : this.backgroundColor);

        if (onBackgroundColor != null)
          onBackgroundColor.Invoke(serializedProperty, elementIndex, ref this.backgroundColor);

        using (BackgroundColorScope(backgroundColor)) {
          using (ColorAlphaScope(0)) {
            fillStyle.Draw(fillRect, false, false, false, false);
          }
        }
      }
    }

    //----------------------------------------------------------------------

    private Action onNextGUIFrame;

    protected void OnNextGUIFrame(Action action) {
      onNextGUIFrame += action;
    }

    //----------------------------------------------------------------------

    public static readonly GUIContent CutLabel = new GUIContent("Cut");
    public static readonly GUIContent CopyLabel = new GUIContent("Copy");
    public static readonly GUIContent PasteLabel = new GUIContent("Paste");
    public static readonly GUIContent DeleteLabel = new GUIContent("Delete");

    protected virtual void PopulateElementContextMenu(GenericMenu menu, int elementIndex) {
      var serializedProperty = this.serializedProperty;
      var serializedObject = serializedProperty.serializedObject;

      menu.AddItem(CutLabel, false, () => OnNextGUIFrame(() => CutElement(elementIndex)));
      menu.AddItem(CopyLabel, false, () => CopyElementToClipboard(elementIndex));
      var content = ClipboardContent.Deserialize(EditorGUIUtility.systemCopyBuffer);
      var canPaste = CanPaste(content);
      if (canPaste) menu.AddItem(PasteLabel, false, () => OnNextGUIFrame(() => PasteElement(elementIndex, content)));
      else menu.AddDisabledItem(PasteLabel);

      if (displayRemove) {
        menu.AddItem(DeleteLabel, false, () => OnNextGUIFrame(() => DeleteElement(elementIndex)));
      }

      if (displayAdd) {
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Insert Above"), false, () => OnNextGUIFrame(() => InsertElement(elementIndex)));
        menu.AddItem(new GUIContent("Insert Below"), false, () => OnNextGUIFrame(() => InsertElement(elementIndex + 1)));
      }
    }

    //----------------------------------------------------------------------

    protected float GetPropertyHeight(SerializedProperty property) {
      return EditorGUI.GetPropertyHeight(property, includeChildren: true);
    }

    protected float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
    }

    //----------------------------------------------------------------------

    protected void PropertyField(Rect position, SerializedProperty property) {
      EditorGUI.PropertyField(position, property, includeChildren: true);
    }

    protected void PropertyField(Rect position, SerializedProperty property, GUIContent label) {
      EditorGUI.PropertyField(position, property, label, includeChildren: true);
    }

    //----------------------------------------------------------------------

    protected static readonly GUIStyle ContextMenuButtonStyle = "Button";

    protected static bool IsRepaint() {
      var current = Event.current;
      return current != null && current.type == EventType.Repaint;
    }

    //----------------------------------------------------------------------

    private void DrawHeader(Rect position) {
      defaultBehaviours.DrawHeaderBackground(position);
      position.xMin += 16;
      position.y++;
      position.height = EditorGUIUtility.singleLineHeight;

      var foldoutRect = position;
      var property = serializedProperty;
      var wasExpanded = property.isExpanded;
      var isExpanded = EditorGUI.Foldout(foldoutRect, wasExpanded, label, true);
      if (isExpanded != wasExpanded) {
        property.isExpanded = isExpanded;
      }
    }

    //----------------------------------------------------------------------

    private GUIContent label = new GUIContent();

    internal void UpdateLabel(GUIContent label) {
      this.label.image = label.image;

      var tooltip = label.tooltip;
      if (string.IsNullOrEmpty(tooltip)) {
        tooltip = serializedProperty.tooltip;
      }
      this.label.tooltip = tooltip;

      var arraySize = serializedProperty.arraySize;

      var listHeaderFormat =
        (arraySize != 1)
        ? pluralListHeaderFormat
        : singularListHeaderFormat;

      var text = label.text ?? string.Empty;
      text = string.Format(listHeaderFormat, text, arraySize).Trim();
      this.label.text = text;
    }

    //----------------------------------------------------------------------

    private readonly List<float> elementHeights = new List<float>();

    private void UpdateElementHeights() {
      var primaryProperty = serializedProperty;
      var elementCount = primaryProperty.arraySize;
      elementHeights.Clear();
      elementHeights.Capacity = elementCount;
      for (int i = 0; i < elementCount; ++i)
        elementHeights.Add(0);

      if (primaryProperty.isExpanded) {
        var spacing = EditorGUIUtility.standardVerticalSpacing;
        var arrayCount = 0;
        foreach (var array in serializedProperties) {
          for (int i = 0; i < elementCount; ++i) {

            var element = array.GetArrayElementAtIndex(i);
            var elementHeight = GetElementHeight(element, i);
            if (arrayCount > 0)
              elementHeight += spacing;

            switch (parallelListLayout) {
              case ParallelListLayout.Rows:
                elementHeights[i] += elementHeight;
                break;
              case ParallelListLayout.Columns:
                elementHeights[i] = Mathf.Max(elementHeights[i], elementHeight);
                break;
            }
          }
          arrayCount += 1;
        }
        for (int i = 0; i < elementCount; ++i) {
          var elementHeight = elementHeights[i];
          elementHeights[i] = AddElementPadding(elementHeight);
        }
      }
    }

    //----------------------------------------------------------------------

    private void DrawHeaderCallback(Rect position) {
      // DoGUI draws the header content after the list is drawn
    }

    private void DrawFooterCallback(Rect position) {
      if (showFooterButtons)
        defaultBehaviours.DrawFooter(position, this);

      position.xMin += 2;
      position.xMax -= 2;
      position.y -= 6;
    }

    private float ElementHeightCallback(int elementIndex) {
      if (elementIndex >= elementHeights.Count) return EditorGUIUtility.singleLineHeight;
      return elementHeights[elementIndex];
    }

    protected virtual float drawElementIndent { get => 0; }

    private void DrawElementCallback(Rect position, int elementIndex, bool isActive, bool isFocused) {
      var primaryProperty = serializedProperty;
      if (primaryProperty.isExpanded) {
        RemoveElementPadding(ref position);
        position.xMin += drawElementIndent;
        switch (parallelListLayout) {
          case ParallelListLayout.Rows:
            DrawElementRows(position, elementIndex, isActive);
            break;
          case ParallelListLayout.Columns:
            DrawElementColumns(position, elementIndex, isActive);
            break;
        }
      }
    }

    private void DrawElementRows(Rect position, int elementIndex, bool isActive) {
      var spacing = EditorGUIUtility.standardVerticalSpacing;
      var loopCounter = 0;
      foreach (var array in serializedProperties) {
        if (loopCounter++ > 0)
          position.y += spacing;

        var element = array.GetArrayElementAtIndex(elementIndex);
        position.height = GetElementHeight(element, elementIndex);
        DrawElement(position, element, elementIndex, isActive);
        position.y += position.height;
      }
    }

    private void DrawElementColumns(Rect position, int elementIndex, bool isActive) {
      const float columnSpacing = 5;
      var lastColumnXMax = position.xMax;
      var columnCount = serializedProperties.Length;
      var columnSpaceCount = columnCount - 1;
      var columnSpaceWidth = columnSpacing * columnSpaceCount;
      var columnWidth = (position.width - columnSpaceWidth) / columnCount;
      columnWidth = Mathf.Floor(columnWidth);
      position.width = columnWidth;
      var loopCounter = 0;
      foreach (var array in serializedProperties) {
        if (loopCounter++ > 0)
          position.x += columnSpacing + columnWidth;

        if (loopCounter == columnCount)
          position.xMax = lastColumnXMax;

        var element = array.GetArrayElementAtIndex(elementIndex);
        position.height = GetElementHeight(element, elementIndex);
        DrawElement(position, element, elementIndex, isActive);
      }
    }

    private void DrawElementBackgroundCallback(Rect position, int elementIndex, bool isActive, bool isFocused) {
      var array = this.serializedProperty;
      if (array.isExpanded == false)
        return;

      var length = array.arraySize;
      var element = default(SerializedProperty);

      var activeIndex = base.index;
      if (activeIndex == elementIndex && isActive == false) {
        // HACK: ReorderableList invokes this callback with the
        // wrong elementIndex.
        var nonDragTargetIndices = this.nonDragTargetIndices;
        if (nonDragTargetIndices != null) {
          elementIndex = nonDragTargetIndices[elementIndex];
        }
      }

      if (elementIndex >= 0 && elementIndex < length) {
        // HACK: ReorderableList invokes this callback with the
        // wrong height.
        position.height = ElementHeightCallback(elementIndex);
        element = array.GetArrayElementAtIndex(elementIndex);
      }

      DrawElementBackground(position, element, elementIndex, isActive, isFocused);

      if (element != null) {
        HandleElementEvents(position, elementIndex);
      }

      var upperEdge = position;
      upperEdge.xMin += 2;
      upperEdge.xMax -= 2;
      upperEdge.y -= 1;

      var lowerEdge = position;
      lowerEdge.xMin += 2;
      lowerEdge.xMax -= 2;
      lowerEdge.y += lowerEdge.height;
      lowerEdge.y -= 1;
    }

    private void DrawEmptyElementCallback(Rect position) {
      using (new EditorGUI.DisabledScope(true)) {
        EditorGUI.LabelField(position, "List is Empty");
      }
    }

    //----------------------------------------------------------------------

    private void HandleElementEvents(Rect position, int elementIndex) {
      var current = Event.current;
      if (current == null) return;

      var handleRect = position;
      var menuRect = Rect.zero;
      if (showElementHeader) {
        handleRect.width += 1;
        menuRect = position;
        menuRect.xMin = menuRect.xMax - 16;
      } else {
        handleRect.width = 19;
      }

      var isLeftMouseInMenuRect = current.button == 0 && menuRect.Contains(current.mousePosition);

      var isRightMouseInHandleRect = current.button == 1 && handleRect.Contains(current.mousePosition);

      var isMouseInRect = isLeftMouseInMenuRect || isRightMouseInHandleRect;

      var isActiveElementIndex = index == elementIndex;

      switch (current.type) {
        case EventType.MouseDown:
          if (isMouseInRect) {
            EndEditingActiveTextField();
            index = elementIndex;
            return;
          }
          break;

        case EventType.MouseUp:
          if (isMouseInRect && isActiveElementIndex) {
            DoElementContextMenu(handleRect, elementIndex);
            return;
          }
          break;
      }
    }

    //----------------------------------------------------------------------

    private void DoElementContextMenu(Rect position, int elementIndex) {
      position.x += 1;
      position.height = elementHeight - 1;

      var menu = new GenericMenu();

      PopulateElementContextMenu(menu, elementIndex);

      if (menu.GetItemCount() > 0)
        menu.DropDown(position);
    }

    //----------------------------------------------------------------------

    private static readonly FieldInfo m_NonDragTargetIndicesField = typeof(ReorderableList).GetField("m_NonDragTargetIndices", BindingFlags.Instance | BindingFlags.NonPublic);

    private List<int> nonDragTargetIndices {
      get => (List<int>)m_NonDragTargetIndicesField.GetValue(this);
    }

    //----------------------------------------------------------------------

    protected virtual float borderHeight => 0;

    private float AddElementPadding(float elementHeight) {
      var verticalSpacing = EditorGUIUtility.standardVerticalSpacing;
      return borderHeight
        + verticalSpacing
        + elementHeight;
    }

    private void RemoveElementPadding(ref Rect position) {
      var verticalSpacing = EditorGUIUtility.standardVerticalSpacing;
      position.yMin += borderHeight;
      position.yMin += verticalSpacing / 2;
      position.yMax -= verticalSpacing / 2;
    }

    //======================================================================

    private delegate void EndEditingActiveTextFieldDelegate();

    private static readonly EndEditingActiveTextFieldDelegate EndEditingActiveTextField =
      (EndEditingActiveTextFieldDelegate)Delegate.CreateDelegate(
        typeof(EndEditingActiveTextFieldDelegate),
        null,
        typeof(EditorGUI).GetMethod("EndEditingActiveTextField", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
      );

    //======================================================================

    protected struct Deferred : IDisposable {
      private readonly Action onDispose;

      public Deferred(Action onDispose) {
        this.onDispose = onDispose;
      }

      public void Dispose() {
        if (onDispose != null)
          onDispose();
      }
    }

    protected static Deferred BackgroundColorScope(Color newColor) {
      var oldColor = GUI.backgroundColor;
      GUI.backgroundColor = newColor;
      return new Deferred(() => GUI.backgroundColor = oldColor);
    }

    protected static Deferred ColorScope(Color newColor) {
      var oldColor = GUI.color;
      GUI.color = newColor;
      return new Deferred(() => GUI.color = oldColor);
    }

    protected static Deferred ColorAlphaScope(float a) {
      var oldColor = GUI.color;
      GUI.color = new Color(1, 1, 1, a);
      return new Deferred(() => GUI.color = oldColor);
    }

    protected IDisposable LabelWidthScope(float newLabelWidth) {
      var oldLabelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = (int)newLabelWidth;
      return new Deferred(() => EditorGUIUtility.labelWidth = oldLabelWidth);
    }

    //======================================================================

    protected static void TryDestroyImmediate(
        Object obj,
        bool allowDestroyingAssets = false) {
      try {
        if (obj != null)
          Object.DestroyImmediate(obj, allowDestroyingAssets);
      } catch (Exception ex) {
        Debug.LogException(ex);
      }
    }

  }

}
#endif
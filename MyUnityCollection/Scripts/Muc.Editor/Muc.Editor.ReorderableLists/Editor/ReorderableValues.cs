

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Text.RegularExpressions;
  using UnityEditor;
  using UnityEditorInternal;
  using UnityEngine;

  using static PropertyUtil;
  using static EditorUtil;


  internal class ReorderableValues : ReorderableList {

    public SerializedProperty prop => serializedProperty;
    public readonly Type listType;
    public readonly Type elementBaseType;

    public readonly bool isReferenceList;
    public readonly bool isUnityObjectList;

    public new int count { // The base implementation throws with some cases with SerializeReference
      get {
        if (!prop.hasMultipleDifferentValues) return prop.arraySize;
        int smallestSize = prop.arraySize;
        foreach (var targetObject in prop.serializedObject.targetObjects) {
          using (var serializedObject = new SerializedObject(targetObject)) {
            var property = serializedObject.FindProperty(prop.propertyPath);
            if (property != null) smallestSize = Math.Min(property.arraySize, smallestSize);
          }
        }
        return smallestSize;
      }
    }

    protected static readonly new Defaults defaultBehaviours = new Defaults();

    //======================================================================

    public ReorderableValues(SerializedProperty primaryProperty)
      : base(primaryProperty.serializedObject, primaryProperty.Copy(), true, true, true, true) {

      primaryProperty.serializedObject.Update();

      var field = GetFieldInfo(primaryProperty);
      listType = field.FieldType;
      elementBaseType = listType.IsArray ? listType.GetElementType() : listType.GetGenericArguments()[0];
      isUnityObjectList = typeof(UnityEngine.Object).IsAssignableFrom(elementBaseType);

      this.isReferenceList = primaryProperty.arrayElementType == "managedReference<>";
      footerHeight = 0;

      drawHeaderCallback = DrawHeaderCallback;
      drawFooterCallback = DrawFooterCallback;
      elementHeightCallback = ElementHeightCallback;
      drawElementCallback = DrawElementCallback;
      drawElementBackgroundCallback = DrawElementBackgroundCallback;
      drawNoneElementCallback = DrawEmptyElementCallback;

      onCanAddCallback = CanAdd;
      onAddCallback = OnAddCallback;
      onCanRemoveCallback = CanRemove;
      onRemoveCallback = OnRemoveCallback;
    }

    //======================================================================

    public float GetHeight(GUIContent label) {
      UpdateLabel(label);
      if (!prop.isExpanded) return 20;
      UpdateElementHeights();
      var height = GetHeight();
      return height + (count < 1 ? -1 : -0);
    }

    public virtual void DoGUI(Rect position) {
      position = EditorGUI.IndentedRect(position);
      using (IndentScope(v => 0)) {
        try {
          if (prop.isExpanded) {
            if (count < 1) {
              DoEmptyList(position);
            } else {
              DoList(position);
            }
          } else {
            index = -1;
            DoCollapsedListBackground(position);
          }
        } finally {
          DoHeader(position);
          if (displayAdd || displayRemove) {
            var footerRect = position;
            footerRect.yMin += 2;
            DoFooter(footerRect, this);
          }
        }
      }
    }

    //======================================================================

    protected int ClampIndex(int index, int plusMax = -1) {
      var max = Mathf.Max(0, count + plusMax);
      return Mathf.Clamp(index, 0, max);
    }

    //======================================================================

    protected bool HasDifferentSizes() {
      if (!prop.hasMultipleDifferentValues) return false;
      var vals = GetValues<IList>(prop);
      var firstLength = vals.First().Count;
      var sameLengths = vals.Aggregate(true, (b, v) => v == null ? b : b && v.Count == firstLength);
      return !sameLengths;
    }

    //======================================================================

    private void DoCollapsedListBackground(Rect position) {
      var headerRect = position;
      headerRect.height = headerHeight;

      if (showDefaultBackground && Event.current.type == EventType.Repaint) {
        defaultBehaviours.DrawHeaderBackground(headerRect);
      }
    }

    //======================================================================

    private bool CanAdd(ReorderableList list) {
      if (!prop.serializedObject.isEditingMultipleObjects) return true;
      return !prop.hasMultipleDifferentValues && !HasDifferentSizes();
    }

    private void OnAddCallback(ReorderableList list) {
      prop.isExpanded = true;
      InsertElement(index < 0 ? count : index);
    }

    private bool CanRemove(ReorderableList list) {
      return prop.isExpanded && index >= 0 && !prop.hasMultipleDifferentValues;
    }

    private void OnRemoveCallback(ReorderableList list) {
      DeleteElement(index);
    }

    //======================================================================

    [Serializable]
    private class ClipboardElement {
      public string type;
      public string assemblyQualifiedName;
      public string json;
      public int instanceId;

      public static ClipboardElement Deserialize(string s) {
        try {
          return JsonUtility.FromJson<ClipboardElement>(s);
        } catch {
          return null;
        }
      }

      public string Serialize() {
        return JsonUtility.ToJson(this);
      }
    }

    private void CopyElementToClipboard(int elementIndex) {
      if (elementIndex < 0 || elementIndex >= count) return;
      EditorGUIUtility.systemCopyBuffer = CopyElementContent(elementIndex).Serialize();
    }

    private ClipboardElement CopyElementContent(int elementIndex) {
      elementIndex = ClampIndex(elementIndex);

      var arrayObj = (IList)prop.GetObject();
      var elementObj = arrayObj[elementIndex];
      var elementType = elementObj?.GetType() ?? elementBaseType;
      var elementJson =
        elementObj == null ? "null" : (
          elementType.IsPrimitive || elementType == typeof(string) ?
          elementObj.ToString() :
          JsonUtility.ToJson(elementObj)
        );

      var clipboardElement = new ClipboardElement {
        type = elementType.FullName,
        assemblyQualifiedName = elementType.AssemblyQualifiedName,
        json = elementJson
      };
      if (elementObj is UnityEngine.Object unityObject) clipboardElement.instanceId = unityObject.GetInstanceID();
      return clipboardElement;
    }

    private void CutElement(int elementIndex) {
      if (elementIndex < 0 || elementIndex >= count) return;

      CopyElementToClipboard(elementIndex);
      DeleteElement(elementIndex);
    }

    private bool CanCopy(int elementIndex) {
      if (elementIndex < 0 || elementIndex >= count) return false;
      var elementProperty = prop.GetArrayElementAtIndex(elementIndex);
      return !elementProperty.hasMultipleDifferentValues;
    }

    private bool CanPaste(ClipboardElement clipboardElement, int elementIndex) {
      if (clipboardElement == null) return false;
      if (elementIndex < 0 || elementIndex >= count) return false;
      var clipboardType = Type.GetType(clipboardElement.assemblyQualifiedName);
      return elementBaseType.IsAssignableFrom(clipboardType);
    }

    private void PasteElement(int elementIndex, ClipboardElement clipboardElement) {
      if (elementIndex < 0 || elementIndex >= count) return;
      if (clipboardElement == null) return;

      foreach (var targetObject in prop.serializedObject.targetObjects) {
        Undo.RecordObject(targetObject, "Paste value");
      }

      var elementProperty = prop.GetArrayElementAtIndex(elementIndex);
      var elementJson = clipboardElement.json;
      var elementInstanceId = clipboardElement.instanceId;

      if (elementBaseType.IsPrimitive || elementBaseType == typeof(string)) {
        object newValue;
        switch (GetFirstValue<object>(elementProperty)) {
          case String _:
            newValue = elementJson;
            break;
          case Char _:
            newValue = Char.Parse(elementJson);
            break;
          case Boolean _:
            newValue = Boolean.Parse(elementJson);
            break;
          case Single _:
          case Double _:
          case Decimal _:
            newValue = Decimal.Parse(elementJson);
            break;
          case UInt64 _:
            newValue = UInt64.Parse(elementJson);
            break;
          default: // Other number type
            newValue = Int64.Parse(elementJson);
            break;
        }
        var converted = Convert.ChangeType(newValue, elementBaseType);
        SetValueNoRecord(elementProperty, converted);
      } else if (typeof(UnityEngine.Object).IsAssignableFrom(elementBaseType)) {
        var fromId = EditorUtility.InstanceIDToObject(elementInstanceId);
        if (fromId != null) SetValueNoRecord(elementProperty, fromId);
      } else {
        var fromJson = JsonUtility.FromJson(elementJson, elementBaseType);
        if (fromJson != null) SetValueNoRecord(elementProperty, fromJson);
      }

      prop.serializedObject.Update();
      GUI.changed = true;
    }

    //======================================================================

    protected static Type GetManagedReferenceType(SerializedProperty property) {
      var typeStrings = property.managedReferenceFullTypename.Split(' ');
      var fullTypeName = typeStrings[1];
      var assemblyName = typeStrings[0];
      var assembly = Assembly.Load(assemblyName);
      var type = assembly.GetType(fullTypeName, true);
      return type;
    }

    protected virtual void InsertElement(int elementIndex, Type type = null) {
      elementIndex = ClampIndex(elementIndex);

      var serializedObject = prop.serializedObject;
      prop.InsertArrayElementAtIndex(elementIndex);

      // Set correct default values for first element
      if (count == 1) {
        var element = prop.GetArrayElementAtIndex(elementIndex);
        if (isReferenceList) {
          var instance = InstaniateType(type ?? elementBaseType);
          if (instance != null) {
            serializedObject.ApplyModifiedProperties();
            element.managedReferenceValue = instance;
          }
        } else if (element.propertyType == SerializedPropertyType.Generic) {
          var instance = InstaniateType(type ?? elementBaseType);
          if (instance != null) {
            serializedObject.ApplyModifiedProperties();
            SetValueNoRecord(element, instance);
          }
        }
      } else if (isReferenceList) {
        // Copy previous element for new elements
        var element = prop.GetArrayElementAtIndex(elementIndex);

        var copyIndex = elementIndex == 0 ? 1 : elementIndex - 1;
        var copyElement = prop.GetArrayElementAtIndex(copyIndex);
        var copyElementType = GetManagedReferenceType(copyElement);

        object instance = InstaniateType(type ?? copyElementType);
        if (instance != null) {
          serializedObject.ApplyModifiedProperties();
          element.managedReferenceValue = instance;
        }
      }

      serializedObject.ApplyModifiedProperties();
      index = ClampIndex(elementIndex + 1);
      GUI.changed = true;
    }

    private static object InstaniateType(Type type) {
      object instance = null;
      try {
        instance = Activator.CreateInstance(type, true);
      } catch (SystemException) {
        try {
          instance = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
        } catch (SystemException) {
        } catch (Exception) {
          throw;
        }
      }
      return instance;
    }

    //======================================================================

    protected virtual void DeleteElement(int elementIndex) {
      if (elementIndex < 0 || elementIndex >= count) return;

      var prop = this.prop;
      var serializedObject = prop.serializedObject;

      var preDelSize = count;
      prop.DeleteArrayElementAtIndex(elementIndex);
      if (isUnityObjectList && preDelSize == count) { // Unity Objects get set to none first...
        prop.DeleteArrayElementAtIndex(elementIndex);
      }
      if (preDelSize != count) { // Make sure deletion actually happened
        index = Math.Min(elementIndex, count - 1);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
      }
      GUI.changed = true;
    }

    //======================================================================

    protected virtual float GetElementHeight(SerializedProperty element, int elementIndex) {
      return EditorGUI.GetPropertyHeight(element, GUIContent.none);
    }

    protected virtual void DrawElement(Rect position, SerializedProperty element, int elementIndex, bool isActive) {
      EditorGUI.PropertyField(position, element, GUIContent.none);
    }

    //======================================================================

    private void DrawElementBackground(Rect position, SerializedProperty element, int elementIndex, bool isActive, bool isFocused) {
      defaultBehaviours.DrawElementBackground(position, elementIndex, isActive, isFocused, true);
    }

    //======================================================================

    protected static readonly GUIContent cutLabel = new GUIContent("Cut");
    protected static readonly GUIContent copyLabel = new GUIContent("Copy");
    protected static readonly GUIContent pasteLabel = new GUIContent("Paste");
    protected static readonly GUIContent deleteLabel = new GUIContent("Delete");

    protected virtual void PopulateElementContextMenu(GenericMenu menu, int elementIndex) {
      var prop = this.prop;
      var serializedObject = prop.serializedObject;
      var canCopy = CanCopy(elementIndex);
      if (canCopy) {
        menu.AddItem(cutLabel, false, () => CutElement(elementIndex));
        menu.AddItem(copyLabel, false, () => CopyElementToClipboard(elementIndex));
      } else {
        menu.AddDisabledItem(cutLabel);
        menu.AddDisabledItem(copyLabel);
      }
      var content = ClipboardElement.Deserialize(EditorGUIUtility.systemCopyBuffer);
      var canPaste = CanPaste(content, elementIndex);
      if (canPaste) menu.AddItem(pasteLabel, false, () => PasteElement(elementIndex, content));
      else menu.AddDisabledItem(pasteLabel);
    }

    //======================================================================

    protected static readonly GUIStyle ContextMenuButtonStyle = "Button";

    //======================================================================

    private void DoHeader(Rect position) {
      defaultBehaviours.DrawHeaderBackground(position);

      var foldoutRect = position;
      foldoutRect.y++;
      foldoutRect.xMin += indentSize;
      foldoutRect.height = lineHeight;
      foldoutRect.width -= 50;
      prop.isExpanded = EditorGUI.Foldout(foldoutRect, prop.isExpanded, label, true);
    }

    //======================================================================

    private void DoEmptyList(Rect position) {
      // draw the background in repaint
      if (showDefaultBackground && Event.current.type == EventType.Repaint)
        defaultBehaviours.boxBackground.Draw(position, false, false, false, false);

      // draw the background
      DrawElementBackgroundCallback(position, -1, false, false);

      var elementContentRect = position;
      elementContentRect.xMin += Defaults.padding;
      elementContentRect.xMax -= Defaults.padding;
      elementContentRect.y += 10;
      DrawEmptyElementCallback(elementContentRect);
    }

    //======================================================================

    private readonly static GUIContent iconToolbarPlus = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add to list");
    private readonly static GUIContent iconToolbarPlusMore = EditorGUIUtility.TrIconContent("Toolbar Plus More", "Choose to add to list");
    private readonly static GUIContent iconToolbarMinus = EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove selection or last element from list");
    private readonly GUIStyle preButton = "RL FooterButton";

    // draw the default footer
    public void DoFooter(Rect rect, ReorderableList list) {
      float rightEdge = rect.xMax;
      float leftEdge = rightEdge - 8f;
      if (list.displayAdd)
        leftEdge -= 25;
      if (list.displayRemove)
        leftEdge -= 25;
      rect = new Rect(leftEdge, rect.y, rightEdge - leftEdge, rect.height);
      Rect addRect = new Rect(leftEdge + 4, rect.y, 25, 16);
      Rect removeRect = new Rect(rightEdge - 29, rect.y, 25, 16);
      if (list.displayAdd) {
        using (DisabledScope(v => v || (list.onCanAddCallback != null && !list.onCanAddCallback(list)))) {
          if (GUI.Button(addRect, list.onAddDropdownCallback != null ? iconToolbarPlusMore : iconToolbarPlus, preButton)) {
            if (list.onAddDropdownCallback != null)
              list.onAddDropdownCallback(addRect, list);
            else if (list.onAddCallback != null)
              list.onAddCallback(list);
            else
              InsertElement(index < 0 ? count - 1 : index);

            list.onChangedCallback?.Invoke(list);
          }
        }
      }
      if (list.displayRemove) {
        using (DisabledScope(v => v || (
            list.index < 0 || list.index >= list.count ||
            (list.onCanRemoveCallback != null && !list.onCanRemoveCallback(list)))
          )
        ) {
          if (GUI.Button(removeRect, iconToolbarMinus, preButton)) {
            if (list.onRemoveCallback == null) {
              DeleteElement(index);
            } else {
              list.onRemoveCallback(list);
            }

            list.onChangedCallback?.Invoke(list);
          }
        }
      }
    }

    //======================================================================

    private readonly GUIContent label = new GUIContent();

    internal void UpdateLabel(GUIContent source) {
      this.label.image = source.image;
      this.label.tooltip = string.IsNullOrEmpty(source.tooltip) ? prop.tooltip : source.tooltip;
      if (prop.serializedObject.isEditingMultipleObjects) {
        if (HasDifferentSizes()) {
          var vals = GetValues<IList>(prop);
          var lengthString = vals.Aggregate("", (b, v) => v == null ? b : b + $"{v.Count}, ");
          lengthString = lengthString.Substring(0, lengthString.Length - 2);
          this.label.text = $"{source.text ?? string.Empty} ({lengthString})";
          return;
        }
      }
      this.label.text = $"{source.text ?? string.Empty} ({count})";
    }

    //======================================================================

    private readonly List<float> elementHeights = new List<float>();

    private void UpdateElementHeights() {
      var count = this.count;
      elementHeights.Clear();
      elementHeights.Capacity = count;
      for (int i = 0; i < count; i++) elementHeights.Add(0);

      if (prop.isExpanded) {
        for (int i = 0; i < count; i++) {
          var element = prop.GetArrayElementAtIndex(i);
          var elementHeight = GetElementHeight(element, i);
          elementHeights[i] += AddElementPadding(elementHeight);
        }
      }
    }

    //======================================================================

    private void DrawHeaderCallback(Rect position) {
      // DoGUI draws the header content after the list is drawn
    }

    private void DrawFooterCallback(Rect position) {
      // Drawn in header
    }

    private float ElementHeightCallback(int elementIndex) {
      if (elementIndex >= elementHeights.Count) return EditorGUIUtility.singleLineHeight;
      return elementHeights[elementIndex];
    }

    private void DrawElementCallback(Rect position, int elementIndex, bool isActive, bool isFocused) {
      var primaryProperty = prop;
      if (primaryProperty.isExpanded) {
        position = RemoveElementPadding(position);
        DrawElement(position, elementIndex, isActive);
      }
    }

    private void DrawElement(Rect position, int elementIndex, bool isActive) {
      var element = prop.GetArrayElementAtIndex(elementIndex);
      DrawElement(position, element, elementIndex, isActive);
    }

    private void DrawElementBackgroundCallback(Rect position, int elementIndex, bool isActive, bool isFocused) {
      if (!prop.isExpanded)
        return;

      var length = count;
      var element = default(SerializedProperty);

      var activeIndex = base.index;
      if (activeIndex == elementIndex && !isActive) {
        // HACK: ReorderableList invokes this callback with the wrong elementIndex.
        if (nonDragTargetIndices != null) {
          elementIndex = nonDragTargetIndices[elementIndex];
        }
      }

      if (elementIndex >= 0 && elementIndex < length) {
        // HACK: ReorderableList invokes this callback with the wrong height.
        position.height = ElementHeightCallback(elementIndex);
        element = prop.GetArrayElementAtIndex(elementIndex);
      }

      DrawElementBackground(position, element, elementIndex, isActive, isFocused);

      if (element != null) {
        HandleElementEvents(position, elementIndex);
      }
    }

    private void DrawEmptyElementCallback(Rect position) {
      using (DisabledScope()) {
        EditorGUI.LabelField(position, "List is Empty");
      }
    }

    //======================================================================

    private void HandleElementEvents(Rect position, int elementIndex) {
      var current = Event.current;
      if (current == null) return;

      var handleRect = position;
      handleRect.width = 19;

      var isMouseInRect = (current.button == 0 || current.button == 1) && handleRect.Contains(current.mousePosition);

      var isActiveElementIndex = index == elementIndex;

      switch (current.type) {
        case EventType.MouseDown:
          if (isMouseInRect) {
            endEditingActiveTextField();
            index = elementIndex;
            return;
          }
          break;

        case EventType.MouseUp:
          if (isMouseInRect && isActiveElementIndex && current.button == 1) {
            DoElementContextMenu(handleRect, elementIndex);
            return;
          }
          break;
      }
    }

    //======================================================================

    private void DoElementContextMenu(Rect position, int elementIndex) {
      position.x += 1;
      position.height = elementHeight - 1;

      var menu = new GenericMenu();

      PopulateElementContextMenu(menu, elementIndex);

      if (menu.GetItemCount() > 0)
        menu.DropDown(position);
    }

    //======================================================================

    private static readonly FieldInfo m_NonDragTargetIndicesField = typeof(ReorderableList).GetField("m_NonDragTargetIndices", BindingFlags.Instance | BindingFlags.NonPublic);

    private List<int> nonDragTargetIndices => (List<int>)m_NonDragTargetIndicesField.GetValue(this);

    //======================================================================

    protected virtual float extraSpacing => 0;

    private float AddElementPadding(float elementHeight) {
      return extraSpacing
        + spacing
        + elementHeight;
    }

    private Rect RemoveElementPadding(Rect position) {
      var verticalSpacing = EditorGUIUtility.standardVerticalSpacing;
      position.yMin += extraSpacing + verticalSpacing / 2;
      position.yMax -= verticalSpacing / 2;
      return position;
    }

    //======================================================================

    private delegate void EndEditingActiveTextFieldDelegate();

    private static readonly EndEditingActiveTextFieldDelegate endEditingActiveTextField =
      (EndEditingActiveTextFieldDelegate)Delegate.CreateDelegate(
        typeof(EndEditingActiveTextFieldDelegate),
        null,
        typeof(EditorGUI).GetMethod("EndEditingActiveTextField", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
      );

    //======================================================================

    protected static Deferred ColorAlphaScope(float a) {
      var oldColor = GUI.color;
      GUI.color = new Color(1, 1, 1, a);
      return new Deferred(() => GUI.color = oldColor);
    }

  }

}
#endif
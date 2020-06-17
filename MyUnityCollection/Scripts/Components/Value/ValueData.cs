

#if UNITY_EDITOR
namespace Muc.Components.Values.Editor {

  using System.Reflection;
  using System.Collections;
  using System.Collections.Generic;

  using UnityEngine;
  using UnityEditor;
  using UnityEditorInternal;


  [CustomEditor(typeof(ValueData))]
  public class ValuePrefsEditor : Editor {

    private Dictionary<ValueData.OrderData, CacheData> cache = new Dictionary<ValueData.OrderData, CacheData>();

    private bool showOrders = true;

    private class CacheData {
      public bool showPosition = true;
      public ReorderableList drawer;

      public CacheData(IList elements) => drawer = new ReorderableList(elements, typeof(ValueData.OrderData));
    }


    public override void OnInspectorGUI() {
      serializedObject.Update();

      var target = this.target as ValueData;

      showOrders = EditorGUILayout.Foldout(showOrders, "Orders");
      if (showOrders) {
        using (var cHorizontalScope = new GUILayout.HorizontalScope()) {
          GUILayout.Space(EditorGUI.indentLevel * 15 + 4);

          using (var cVerticalScope = new GUILayout.VerticalScope()) {
            EditorGUILayout.LabelField("Value modifiers are executed in these orders");

            foreach (var orderData in target.orders) {

              if (!this.cache.TryGetValue(orderData, out var cache)) {

                cache = new CacheData(target.GetByFullName(orderData.valueName).modifiers);
                this.cache.Add(orderData, cache);

                cache.drawer.displayAdd = false;
                cache.drawer.displayRemove = false;
                cache.drawer.headerHeight = 1;

                cache.drawer.onReorderCallbackWithDetails = (ReorderableList list, int oldIndex, int newIndex) => {

                  // Restore old state
                  var moved = list.list[newIndex];
                  list.list.RemoveAt(newIndex);
                  list.list.Insert(oldIndex, moved);

                  EditorUtility.SetDirty(target);
                  Undo.RegisterCompleteObjectUndo(target, "Modified list");

                  // Restore new state 
                  list.list.RemoveAt(oldIndex);
                  list.list.Insert(newIndex, moved);

                  target.Validate();
                };
              }

              cache.showPosition = EditorGUILayout.BeginFoldoutHeaderGroup(cache.showPosition, orderData.valueName);
              if (cache.showPosition) {
                cache.drawer.DoLayoutList();
              }
              EditorGUILayout.EndFoldoutHeaderGroup();
            }
          }

        }
      }


      EditorGUILayout.EndFoldoutHeaderGroup();
      serializedObject.ApplyModifiedProperties();
    }
  }
}
#endif


namespace Muc.Components.Values {

  using System;
  using System.Linq;
  using System.Collections;
  using System.Collections.Generic;

  using UnityEngine;


  [CreateAssetMenu(menuName = "ValueData")]
  public class ValueData : ScriptableObject {

    /// <summary>
    /// `Key`: Value Type  
    /// `Value`: Modifier Types of that Value Type  
    /// </summary>
    Dictionary<Type, List<Type>> typeDict = new Dictionary<Type, List<Type>>();

    [SerializeField]
    public List<OrderData> orders = new List<OrderData>();

    [System.Serializable]
    public class OrderData {
      public string valueName;
      public List<string> modifiers = new List<string>();
    }

    public OrderData GetByFullName(string tValue) {
      var res = orders.Find(v => v.valueName == tValue);
      if (res == null) {
        orders.Add(new OrderData() { valueName = tValue });
        return orders.Last();
      }
      return res;
    }

    public List<Type> this[Type type] => GetModifiers(type);

    public List<Type> GetModifiers<T>() => GetModifiers(typeof(T));
    public List<Type> GetModifiers(Type type) {
      if (typeDict.TryGetValue(type, out var modifiers))
        return modifiers;
      return typeDict[type] = new List<Type>();
    }

    private void Awake() => Validate();
    private void OnValidate() => Validate();

    public void Validate() {
      RefreshTypeDict();
      AddMissingNames();
      RemoveUnknownNames();
      DeDuplicateNames();
      SyncModifierOrders();
    }

    private void AddMissingNames() {
      foreach (var kv in typeDict) {
        var order = GetByFullName(kv.Key.FullName);
        foreach (var modifiers in kv.Value) {
          var name = modifiers.FullName;
          if (!order.modifiers.Contains(name)) {
            order.modifiers.Insert(0, name);
          }
        }
      }
    }

    private void RemoveUnknownNames() {
      for (int i = 0; i < orders.Count; i++) {
        var order = orders[i];
        if (!typeDict.Keys.Any(k => k.FullName == order.valueName)) {
          orders.RemoveAt(i--);
        } else {
          for (int j = 0; j < order.modifiers.Count; j++) {
            var modifierName = order.modifiers[j];

            var key = typeDict.Keys.Single(k => k.FullName == order.valueName);
            var modifierTypes = typeDict[key];

            if (!modifierTypes.Any(v => v.FullName == modifierName)) {
              order.modifiers.RemoveAt(j--);
            }
          }
        }
      }
    }

    private void DeDuplicateNames() {
      var seenGens = new List<string>();
      var deleteGens = new List<OrderData>();

      foreach (var orderData in orders) {
        if (seenGens.Contains(orderData.valueName)) {
          deleteGens.Add(orderData);
          continue;
        }
        seenGens.Add(orderData.valueName);

        var seenMods = new List<string>();
        var deleteMods = new List<string>();

        foreach (var modifier in orderData.modifiers) {
          if (seenMods.Contains(modifier)) {
            deleteMods.Add(modifier);
            continue;
          }
          seenMods.Add(modifier);
        }

        foreach (var deleta in deleteMods) {
          orderData.modifiers.RemoveAt(orderData.modifiers.LastIndexOf(deleta));
        }
      }

      foreach (var delet in deleteGens) {
        orders.RemoveAt(orders.LastIndexOf(delet));
      }
    }

    private void SyncModifierOrders() {
      foreach (var kvp in typeDict) {
        var mods = kvp.Value;
        var valueName = kvp.Key.FullName;
        var modNames = orders.Find(v => v.valueName == valueName).modifiers;

        mods.Sort((a, b) => modNames.IndexOf(a.FullName).CompareTo(modNames.IndexOf(b.FullName)));
      }
    }

    private void RefreshTypeDict() {

      foreach (var duo in GetModifierTypes()) {
        var type = duo.Item1;
        var tValue = duo.Item2.GenericTypeArguments[1];

        if (this.typeDict.TryGetValue(tValue, out var modifiers)) {
          if (!modifiers.Contains(type)) modifiers.Add(type);
        } else {
          this.typeDict[tValue] = new List<Type>() { type };
        }
      }
    }

    private static IEnumerable<(Type, Type)> GetModifierTypes() {
      var assembly = typeof(Modifier<,>).Assembly;
      var types = assembly.GetTypes();
      foreach (var type in types) {
        if (GetModifierBaseType(type, out var modifierBase)) {
          yield return (type, modifierBase);
        }
      }
    }

    private static bool GetModifierBaseType(Type type, out Type modifierBase) {
      modifierBase = null;
      if (!type.IsClass || type.IsAbstract) return false;

      while (type != null && type.IsClass) {
        type = type.BaseType;
        if (type == null) return false;
        if (
          type.IsAbstract &&
          type.IsGenericType &&
          type.GenericTypeArguments.Length == 2 &&
          type.GetGenericTypeDefinition() == typeof(Modifier<,>)
        ) {
          modifierBase = type;
          return true;
        }
      }
      return false;
    }
  }
}
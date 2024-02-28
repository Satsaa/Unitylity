
// License: MIT
// Original Author: Erik Eriksson (2020)
// Original Code: https://github.com/upscalebaby/generic-serializable-dictionary

namespace Unitylity.Data {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [Serializable]
    public class SerializedHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, ISerializationCallbackReceiver {

        public SerializedHashSet() { }
        public SerializedHashSet(IEnumerable<T> collection) { set = new(collection); }
        public SerializedHashSet(IEqualityComparer<T> comparer) { set = new(comparer); }
        public SerializedHashSet(int capacity) { set = new(capacity); }
        public SerializedHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) { set = new(collection, comparer); }
        public SerializedHashSet(int capacity, IEqualityComparer<T> comparer) { set = new(capacity, comparer); }

        [SerializeField]
        List<T> list = new();

        HashSet<T> set = new();

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            list = new(set);
        }
        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            set = new(list);
        }

        public int Count => set.Count;
        public IEqualityComparer<T> Comparer => set.Comparer;

        public bool Add(T item) => set.Add(item);
        public void Clear() => set.Clear();
        public bool Contains(T item) => set.Contains(item);
        public void CopyTo(T[] array) => set.CopyTo(array);
        public void CopyTo(T[] array, int arrayIndex) => set.CopyTo(array, arrayIndex);
        public void CopyTo(T[] array, int arrayIndex, int count) => set.CopyTo(array, arrayIndex, count);
        public int EnsureCapacity(int capacity) => set.EnsureCapacity(capacity);
        public void ExceptWith(IEnumerable<T> other) => set.ExceptWith(other);
        public void IntersectWith(IEnumerable<T> other) => set.IntersectWith(other);
        public bool IsProperSubsetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<T> other) => set.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<T> other) => set.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<T> other) => set.IsSupersetOf(other);
        public bool Overlaps(IEnumerable<T> other) => set.Overlaps(other);
        public bool Remove(T item) => set.Remove(item);
        public int RemoveWhere(Predicate<T> match) => set.RemoveWhere(match);
        public bool SetEquals(IEnumerable<T> other) => set.SetEquals(other);
        public void SymmetricExceptWith(IEnumerable<T> other) => set.SymmetricExceptWith(other);
        public void TrimExcess() => set.TrimExcess();
        public bool TryGetValue(T equalValue, out T actualValue) => set.TryGetValue(equalValue, out actualValue);
        public void UnionWith(IEnumerable<T> other) => set.UnionWith(other);

        bool ICollection<T>.IsReadOnly => ((ICollection<T>)set).IsReadOnly;
        HashSet<T>.Enumerator GetEnumerator() => set.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)set).GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)set).GetEnumerator();
        void ICollection<T>.Add(T item) => Add(item);
    }

}


#if UNITY_EDITOR
namespace Unitylity.Data.Editor {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using static Unitylity.Editor.EditorUtil;
    using static Unitylity.Editor.PropertyUtil;
    using Object = UnityEngine.Object;

    [CustomPropertyDrawer(typeof(SerializedHashSet<>), true)]
    public class SerializedHashSetDrawer : PropertyDrawer {

        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
            // Draw list.
            var list = property.FindPropertyRelative("list");
            string fieldName = ObjectNames.NicifyVariableName(fieldInfo.Name);
            var currentPos = new Rect(lineHeight, pos.y, pos.width, lineHeight);
            EditorGUI.PropertyField(currentPos, list, new GUIContent(fieldName), true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float totHeight = 0f;

            // Height of KeyValue list.
            var listProp = property.FindPropertyRelative("list");
            totHeight += EditorGUI.GetPropertyHeight(listProp, true);

            return totHeight;
        }

    }

}
#endif


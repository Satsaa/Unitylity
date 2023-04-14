
namespace Unitylity.Components {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

#if UNITYLITY_GENERAL_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/General/" + nameof(Tags))]
#endif
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public class Tags : MonoBehaviour, ICollection<string>, IEnumerable<string>, IEnumerable, IReadOnlyCollection<string>, ISerializationCallbackReceiver {

		#region Global Static Container

		private static Dictionary<string, HashSet<Tags>> tagged = new();

		private static ICollection<string> GetTags() => tagged.Keys;

		/// <summary> Returns an array of GameObjects tagged `tag` </summary>
		public static IEnumerable<GameObject> GetTagged(string tag) {
			if (tagged.TryGetValue(tag, out var val)) {
				foreach (var v in val) {
					yield return v.gameObject;
				}
			}
		}

		/// <summary> Returns an array of GameObjects tagged at least `tags` </summary>
		public static IEnumerable<GameObject> GetTaggedAll(IEnumerable<string> tags) {
			var emumer = tags.GetEnumerator();

			if (!emumer.MoveNext()) yield break; // Empty

			var first = emumer.Current;
			if (tagged.TryGetValue(first, out var val)) {
				foreach (var v in val.Where(c => c.ContainsAll(tags))) {
					yield return v.gameObject;
				}
			}

		}

		/// <summary> Returns an array of GameObjects tagged any of `tags` </summary>
		public static IEnumerable<GameObject> GetTaggedAny(IEnumerable<string> tags) {
			foreach (var tag in tags) {
				if (tagged.TryGetValue(tag, out var val)) {
					foreach (var v in val) {
						yield return v.gameObject;
					}
				}
			}
		}

		/// <summary> Sometimes when prefabs with Tags component is destroyed or created, nulls are created in the index </summary>
		private static void RemoveNulls() {
			foreach (var key in Tags.tagged.Keys) {
				var taggeds = Tags.tagged[key];
				taggeds.RemoveWhere(t => !t);
				if (taggeds.Count == 0) Tags.tagged.Remove(key);
			}
		}

		#endregion



		#region Component
		#region - Functionality

		private HashSet<string> tags = new();
		[SerializeField] string[] serializableTags;

		public int Count => tags.Count;
		public bool IsReadOnly => false;

		void ICollection<string>.Add(string tag) => Add(tag);
		public bool Add(string tag) {
			RegisterTag(tag);
			return tags.Add(tag);
		}

		public bool Remove(string tag) {
			UnregisterTag(tag);
			return tags.Remove(tag);
		}
		public void Clear() {
			UnregisterAll();
			tags.Clear();
		}

		public bool Contains(string tag) => tags.Contains(tag);
		public bool ContainsAll(IEnumerable<string> tags) => tags.All(tag => Contains(tag));
		public bool ContainsAny(IEnumerable<string> tags) => tags.Any(tag => Contains(tag));

		public bool CompareTags(Tags other) => other.CompareTags(tags);
		public bool CompareTags(HashSet<string> tags) {
			if (tags.Count != this.tags.Count) return false;
			return ContainsAll(tags); // Same tags?
		}


		void ICollection<string>.CopyTo(string[] array, int arrayIndex) => tags.CopyTo(array, arrayIndex);
		public IEnumerator<string> GetEnumerator() => tags.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => tags.GetEnumerator();

		#endregion
		#region - Innerworks


		void OnDestroy() {
			UnregisterAll();
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize() {
			serializableTags = tags.ToArray();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize() {
			if (serializableTags == null) return;
			tags = new HashSet<string>(serializableTags);
			RegisterAll();
		}

		private void RegisterAll() {
			foreach (var tag in tags) {
				RegisterTag(tag);
			}
		}
		private void RegisterTag(string tag) {
			if (!tagged.ContainsKey(tag)) tagged[tag] = new HashSet<Tags>();
			tagged[tag].Add(this);
		}


		private void UnregisterAll() {
			foreach (var tag in tags) {
				UnregisterTag(tag);
			}
		}

		private void UnregisterTag(string tag) {
			if (tagged.TryGetValue(tag, out var val)) {
				val.Remove(this);
				if (val.Count == 0)
					tagged.Remove(tag);
			}
		}

		#endregion
		#endregion

	}

}


#if UNITY_EDITOR
namespace Unitylity.Components.Editor {

	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(Tags))]
	public class TagsEditor : Editor {

		private string newTagName = "New Tag";

		private Tags t => target as Tags;

		private List<string> tagDisplay = new();

		public override void OnInspectorGUI() {
			serializedObject.Update();


			// -- Add new tag --
			using (new EditorGUILayout.HorizontalScope()) {
				newTagName = EditorGUILayout.TextField(newTagName);
				newTagName = newTagName.Trim();
				if (GUILayout.Button("Add Tag")) {
					if (!t.Contains(newTagName) && newTagName != "") {
						((ISerializationCallbackReceiver)t).OnBeforeSerialize();
						Undo.RegisterCompleteObjectUndo(t, "Add Tag");
						t.Add(newTagName);
						EditorUtility.SetDirty(target);
					}
				}
			}

			EditorGUILayout.Separator();

			// Display tags, allow removal and editing. ToArray to allow modification during foreach
			var asList = t.ToList();

			// Sync tag and display collection size
			while (tagDisplay.Count > asList.Count) tagDisplay.RemoveAt(tagDisplay.Count - 1);
			while (tagDisplay.Count < asList.Count) tagDisplay.Add(asList[tagDisplay.Count]);

			for (int i = 0; i < asList.Count; i++) {
				var tag = asList[i];

				using (new EditorGUILayout.HorizontalScope()) {

					// -- Tag edit --
					// Reset to actual tag string when not editing
					if (!EditorGUIUtility.editingTextField) tagDisplay[i] = tag;

					EditorGUI.BeginChangeCheck();

					var tagString = EditorGUILayout.TextField(tagDisplay[i]);
					tagString = tagString.Trim();

					if (EditorGUI.EndChangeCheck() && tagString != "" && !t.Contains(tagString)) {
						tagDisplay[i] = tagString;
						t.Remove(tag);
						t.Add(tagString);
						Undo.RegisterCompleteObjectUndo(t, "Rename Tag");
						((ISerializationCallbackReceiver)t).OnBeforeSerialize();
						EditorUtility.SetDirty(target);
					}

					// -- Remove button --
					if (GUILayout.Button("Remove Tag")) {
						if (t.Contains(tag)) {
							Undo.RegisterCompleteObjectUndo(t, "Remove Tag");
							t.Remove(tag);
							((ISerializationCallbackReceiver)t).OnBeforeSerialize();
							EditorUtility.SetDirty(target);
						}
					}

				}
			}

			serializedObject.ApplyModifiedPropertiesWithoutUndo();
		}
	}

}
#endif
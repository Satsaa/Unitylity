
namespace Unitylity.Addressables {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading.Tasks;
	using Unitylity.Data;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.AddressableAssets;
	using UnityEngine.ResourceManagement.AsyncOperations;
	using UnityEngine.ResourceManagement.ResourceProviders;
	using UnityEngine.SceneManagement;
	using UnityEngine.U2D;
	using Object = UnityEngine.Object;

	/// <summary>
	/// A custom (better) implementation of AssetReferenceT<T> which accepts components of assets.
	/// </summary>
	[Serializable]
	public class ComponentReference<T> where T : Component {

		[Serializable]
		protected class ComponentReferenceT : AssetReferenceT<GameObject> {

			public ComponentReferenceT(string guid) : base(guid) { }

			public override bool ValidateAsset(Object obj) {
				return obj is GameObject go && go.TryGetComponent<T>(out var _);
			}

			public override bool ValidateAsset(string mainAssetPath) {
#if UNITY_EDITOR
				if (!typeof(GameObject).IsAssignableFrom(AssetDatabase.GetMainAssetTypeAtPath(mainAssetPath)))
					return false;

				var repr = AssetDatabase.LoadMainAssetAtPath(mainAssetPath);
				return repr != null && repr is GameObject go && go.TryGetComponent<T>(out var _);
#else
				return false;
#endif
			}
		}

		public ComponentReference() {
#if UNITY_EDITOR && UNITY_2019_3_OR_NEWER
			EditorApplication.playModeStateChanged -= OnChange;
			EditorApplication.playModeStateChanged += OnChange;

			void OnChange(PlayModeStateChange state) {
				isCached = false;
				cached = default;
			}
#endif
		}

		public static implicit operator T(ComponentReference<T> v) => v.value;
		public static implicit operator bool(ComponentReference<T> v) => !String.IsNullOrEmpty(v.assetReference.AssetGUID);

		[SerializeField]
		protected ComponentReferenceT assetReference;

		protected AsyncOperationHandle operationHandle => assetReference.OperationHandle;

		public T value => isCached ? cached : LoadSync();
		public T valueOrNull => cached;

		protected T cached;
		protected bool isCached;

		public Transform transform => value.transform;
		public TC GetComponent<TC>() where TC : Component => value.GetComponent<TC>();
		public TC[] GetComponents<TC>() where TC : Component => value.GetComponents<TC>();
		public void GetComponents<TC>(List<TC> results) where TC : Component => value.GetComponents(results);
		public TC GetComponentInChildren<TC>() where TC : Component => value.GetComponentInChildren<TC>();
		public TC GetComponentInChildren<TC>(bool includeInactive) where TC : Component => value.GetComponentInChildren<TC>(includeInactive);
		public TC[] GetComponentsInChildren<TC>() where TC : Component => value.GetComponentsInChildren<TC>();
		public TC[] GetComponentsInChildren<TC>(bool includeInactive) where TC : Component => value.GetComponentsInChildren<TC>(includeInactive);
		public void GetComponentsInChildren<TC>(List<TC> results) where TC : Component => value.GetComponentsInChildren(results);
		public void GetComponentsInChildren<TC>(bool includeInactive, List<TC> results) where TC : Component => value.GetComponentsInChildren(includeInactive, results);
		public TC GetComponentInParent<TC>() where TC : Component => value.GetComponentInParent<TC>();
		public TC GetComponentInParent<TC>(bool includeInactive) where TC : Component => value.GetComponentInParent<TC>(includeInactive);
		public TC[] GetComponentsInParent<TC>() where TC : Component => value.GetComponentsInParent<TC>();
		public TC[] GetComponentsInParent<TC>(bool includeInactive) where TC : Component => value.GetComponentsInParent<TC>(includeInactive);
		public void GetComponentsInParent<TC>(bool includeInactive, List<TC> results) where TC : Component => value.GetComponentsInParent(includeInactive, results);
		public bool TryGetComponent<TC>(out TC component) where TC : Component => value.TryGetComponent(out component);

		/// <summary>
		/// Load the referenced asset asynchronously.
		/// </summary>
		public virtual async Task<T> LoadAsync() {
			if (!this) {
				isCached = true;
				return cached = default;
			}
			if (!operationHandle.IsValid())
				await assetReference.LoadAssetAsync<GameObject>().Task;
			isCached = true;
			return cached = Object.Equals(operationHandle.Result, null) ? default : ((GameObject)operationHandle.Result).GetComponent<T>();
		}

		/// <summary>
		/// Load the referenced asset synchronously.
		/// </summary>
		public virtual T LoadSync() {
			if (!this) {
				isCached = true;
				return cached = default;
			}
			if (!operationHandle.IsValid()) {
				var op = assetReference.LoadAssetAsync<GameObject>();
				op.WaitForCompletion();
			}
			isCached = true;
			return cached = Object.Equals(operationHandle.Result, null) ? default : ((GameObject)operationHandle.Result).GetComponent<T>();
		}

		/// <summary>
		/// Releases the asset from memory.
		/// </summary>
		public virtual void Release() {
			isCached = false;
			cached = default;
			assetReference.ReleaseAsset();
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Addressables.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomPropertyDrawer(typeof(ComponentReference<>), true)]
	public class ComponentReferenceDrawer : PropertyDrawer {

		static GUIStyle text = "ControlLabel";
		static GUIStyle bg = "TabWindowBackground";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var assetReference = property.FindPropertyRelative("assetReference");
			using (PropertyScope(position, label, property, out label)) {
				var actualLabel = new GUIContent(label);
				using (ForceIndentScope(position, out var indented)) {
					EditorGUI.BeginChangeCheck();
					PropertyField(indented, label, assetReference);
					if (EditorGUI.EndChangeCheck()) {
						Debug.Log("Changed");
					}
				}
				var style = new GUIStyle {
					alignment = TextAnchor.MiddleLeft
				};

				var labelRect = LabelRect(position);
				actualLabel.text = ObjectNames.NicifyVariableName(actualLabel.text);
				GUI.Box(labelRect, "", bg);
				GUI.Box(labelRect, actualLabel, text);

			}
		}

	}

}
#endif
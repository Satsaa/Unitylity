
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
	/// A custom (better) implementation of AssetReferenceT<T>
	/// </summary>
	[Serializable]
	public class AssetReference<T> where T : Object {

		public AssetReference() {
#if UNITY_EDITOR && UNITY_2019_3_OR_NEWER
			EditorApplication.playModeStateChanged -= OnChange;
			EditorApplication.playModeStateChanged += OnChange;

			void OnChange(PlayModeStateChange state) {
				isCached = false;
				cached = default;
			}
#endif
		}

		public static implicit operator T(AssetReference<T> v) => v.value;
		public static implicit operator bool(AssetReference<T> v) => !String.IsNullOrEmpty(v.assetReference.AssetGUID);

		[SerializeField]
		protected AssetReferenceT<T> assetReference;

		protected AsyncOperationHandle operationHandle => assetReference.OperationHandle;

		public T value => isCached ? cached : LoadSync();
		public T valueOrNull => cached;

		protected T cached;
		protected bool isCached;

		/// <summary>
		/// Load the referenced asset asynchronously.
		/// </summary>
		public virtual async Task<T> LoadAsync() {
			if (!this) {
				isCached = true;
				return cached = default;
			}
			if (!operationHandle.IsValid())
				await assetReference.LoadAssetAsync<T>().Task;
			isCached = true;
			return cached = operationHandle.Result as T;
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
				var op = assetReference.LoadAssetAsync<T>();
				op.WaitForCompletion();
			}
			isCached = true;
			return cached = operationHandle.Result as T;
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
	[CustomPropertyDrawer(typeof(AssetReference<>), true)]
	public class AssetReferenceDrawer : PropertyDrawer {

		static GUIStyle text = "ControlLabel";
		static GUIStyle bg = "TabWindowBackground";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var assetReference = property.FindPropertyRelative("assetReference");
			using (PropertyScope(position, label, property, out label)) {
				var actualLabel = new GUIContent(label);
				using (ForceIndentScope(position, out var indented)) {
					PropertyField(indented, label, assetReference);
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
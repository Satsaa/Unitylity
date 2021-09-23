

namespace Muc.Components.Extended {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using Object = UnityEngine.Object;

	public abstract class UISingleton<T> : ExtendedUIBehaviour where T : ExtendedUIBehaviour {

		public static T instance => _instance;
		private static T _instance;

		new public static RectTransform rectTransform => instance.rectTransform;

		new protected void OnValidate() {
#if UNITY_EDITOR // Prevent activation in prefabs
			if (UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) == null && !UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject)) {
#endif
				if (_instance != null && _instance != this) {
					Debug.LogWarning($"Multiple {typeof(T).Name} GameObjects!", this);
					Debug.LogWarning($"Main instance of {typeof(T).Name}: {_instance}", _instance);
				} else {
					_instance = this as T;
				}
#if UNITY_EDITOR
			}
			base.OnValidate();
#endif
		}

		new protected void Awake() {
			if (_instance != null && _instance != this) {
				Debug.LogWarning($"Multiple {typeof(T).Name} GameObjects!", this);
				Debug.LogWarning($"Main instance of {typeof(T).Name}: {_instance}", _instance);
			} else {
				_instance = this as T;
			}
			base.Awake();
		}

		new protected void OnDestroy() {
			if (_instance == this) {
				_instance = null;
			}
			base.OnDestroy();
		}

	}

}
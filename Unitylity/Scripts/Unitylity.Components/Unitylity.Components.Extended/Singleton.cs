
namespace Unitylity.Components.Extended {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Object = UnityEngine.Object;

	[DefaultExecutionOrder(-1)]
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

		public static T instance => _instance;
		private static T _instance;

		protected virtual void OnValidate() {
#if UNITY_EDITOR // Prevent activation in prefabs
			if (UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) == null && !UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
#endif
				if (_instance != null && _instance != this) {
					Debug.LogWarning($"Multiple {typeof(T).Name} GameObjects!", this);
					Debug.LogWarning($"Main instance of {typeof(T).Name}: {_instance}", _instance);
				} else {
					_instance = this as T;
				}
		}

		protected virtual void Awake() {
			if (_instance != null && _instance != this) {
				Debug.LogWarning($"Multiple {typeof(T).Name} GameObjects!", this);
				Debug.LogWarning($"Main instance of {typeof(T).Name}: {_instance}", _instance);
			} else {
				_instance = this as T;
			}
		}

		protected virtual void OnDestroy() {
			if (_instance == this) {
				_instance = null;
			}
		}

	}

}
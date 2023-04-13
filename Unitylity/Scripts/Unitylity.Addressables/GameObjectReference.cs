
namespace Unitylity.Addressables {

	using System;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// A custom (better) implementation of AssetReferenceT<T>
	/// </summary>
	[Serializable]
	public class GameObjectReference : AssetReference<GameObject> {

		public static implicit operator GameObject(GameObjectReference v) => v.value;
		public static implicit operator bool(GameObjectReference v) => !string.IsNullOrEmpty(v.assetReference.AssetGUID);

		public Transform transform => value.transform;
		public T GetComponent<T>() where T : Component => value.GetComponent<T>();
		public T[] GetComponents<T>() where T : Component => value.GetComponents<T>();
		public void GetComponents<T>(List<T> results) where T : Component => value.GetComponents(results);
		public T GetComponentInChildren<T>() where T : Component => value.GetComponentInChildren<T>();
		public T GetComponentInChildren<T>(bool includeInactive) where T : Component => value.GetComponentInChildren<T>(includeInactive);
		public T[] GetComponentsInChildren<T>() where T : Component => value.GetComponentsInChildren<T>();
		public T[] GetComponentsInChildren<T>(bool includeInactive) where T : Component => value.GetComponentsInChildren<T>(includeInactive);
		public void GetComponentsInChildren<T>(List<T> results) where T : Component => value.GetComponentsInChildren(results);
		public void GetComponentsInChildren<T>(bool includeInactive, List<T> results) where T : Component => value.GetComponentsInChildren(includeInactive, results);
		public T GetComponentInParent<T>() where T : Component => value.GetComponentInParent<T>();
		public T GetComponentInParent<T>(bool includeInactive) where T : Component => value.GetComponentInParent<T>(includeInactive);
		public T[] GetComponentsInParent<T>() where T : Component => value.GetComponentsInParent<T>();
		public T[] GetComponentsInParent<T>(bool includeInactive) where T : Component => value.GetComponentsInParent<T>(includeInactive);
		public void GetComponentsInParent<T>(bool includeInactive, List<T> results) where T : Component => value.GetComponentsInParent(includeInactive, results);
		public bool TryGetComponent<T>(out T component) where T : Component => value.TryGetComponent(out component);

	}

}
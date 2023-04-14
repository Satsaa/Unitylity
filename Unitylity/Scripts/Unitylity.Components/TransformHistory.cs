
namespace Unitylity.Components {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Unitylity.Collections;

#if UNITYLITY_GENERAL_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/General/" + nameof(TransformHistory))]
#endif
	[DefaultExecutionOrder(1000)]
	public class TransformHistory : MonoBehaviour, IEnumerable<TransformHistory.TransformData>, IReadOnlyList<TransformHistory.TransformData> {

		public TransformData this[int index] => history[index];

		public int Count { get => history.Length; private set => history.Length = value; }

		IEnumerator IEnumerable.GetEnumerator() => history.GetEnumerator();
		public IEnumerator<TransformData> GetEnumerator() => history.GetEnumerator();

		private readonly CircularArray<TransformData> history = new(2);

		void LateUpdate() {
			history.Add(new TransformData(transform));
		}

		/// <summary>
		/// Sets the size of the history to length if it would increase it.
		/// </summary>
		/// <returns> Resulting length of history</returns>
		public int SetMinSize(int count) {
			if (count <= Count) return Count;
			history.Resize(count);
			return history.Length;
		}

		public struct TransformData {
			readonly public Vector3 position;
			readonly public Vector3 localPosition;
			readonly public Quaternion rotation;
			readonly public Quaternion localRotation;
			readonly public Vector3 localScale;

			public static implicit operator Vector3(TransformData a) => a.position;

			public TransformData(UnityEngine.Transform transform) {
				position = transform.position;
				localPosition = transform.localPosition;
				rotation = transform.rotation;
				localRotation = transform.localRotation;
				localScale = transform.localScale;
			}
		}

	}

}
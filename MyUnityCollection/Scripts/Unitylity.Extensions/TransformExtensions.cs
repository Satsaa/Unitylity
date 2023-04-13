
namespace Unitylity.Extensions {

	using UnityEngine;

	public static class TransformExtensions {

		/// <summary>
		/// Returns a Rect that represents a RectTransform in screen space.
		/// </summary>
		/// <remarks>
		/// Only supports overlay canvas mode, or more specifically any canvas that is in screenspace coordinates.
		/// </remarks>
		public static Rect ScreenRect(this RectTransform rectTransform) {
			Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);
			return new Rect((Vector2)rectTransform.position - (size * 0.5f), size);
		}

		/// <summary>
		/// Moves the pivot without moving the object visually.
		/// </summary>
		/// <param name="rectTransform"></param>
		/// <param name="pivot"></param>
		public static void ShiftPivot(this RectTransform rectTransform, Vector2 pivot) {
			Vector2 size = rectTransform.rect.size;
			Vector2 deltaPivot = rectTransform.pivot - pivot;
			Vector3 deltaPosition = rectTransform.rotation * new Vector3((deltaPivot.x * size.x) * rectTransform.localScale.x, (deltaPivot.y * size.y) * rectTransform.localScale.y);
			rectTransform.pivot = pivot;
			rectTransform.localPosition -= deltaPosition;
		}

	}

}
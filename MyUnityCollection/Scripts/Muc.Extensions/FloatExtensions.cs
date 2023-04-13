
namespace Unitylity.Extensions {

	using UnityEngine;

	public static class FloatExtensions {

		public static float RoundToNearest(this float integer, float nearest) => Mathf.Round(integer / nearest) * nearest;

		public static float Remap(this float value, float from1, float to1, float from2, float to2) => (value - from1) / (to1 - from1) * (to2 - from2) + from2;

	}

}
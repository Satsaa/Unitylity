

namespace Muc.Systems.RenderImages {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Muc.Components.Extended;
	using Muc.Extensions;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.RenderImages) + "/" + nameof(RenderObjects))]
#endif
	public class RenderObjects : Singleton<RenderObjects>, ISerializationCallbackReceiver {

		[SerializeField] float distance = 200;

		[SerializeField, HideInInspector] Vector2Int pos;
		[SerializeField, HideInInspector] int steps = 1; // Steps for direction
		[SerializeField, HideInInspector] int dir; // 0 - 3
		[SerializeField, HideInInspector] int dirI; // Steps taken in a direction

		Dictionary<RenderObject, RenderObject> shareds = new();

		public RenderObject GetObject(RenderObject prefab, bool shared) {
			var res = default(RenderObject);
			if (!shared || ((res = GetSharedObject(prefab)) == null)) {
				res = Instantiate(prefab, transform);
				if (shared) shareds.Add(prefab, res);
			}
			var scaledPos = pos.Mul(distance);
			res.transform.localPosition = scaledPos.x0y();
			AdvancePos();
			return res;
		}

		private RenderObject GetSharedObject(RenderObject prefab) {
			shareds.TryGetValue(prefab, out var res);
			if (res == null) shareds.Remove(prefab);
			return res;
		}

		void AdvancePos() {

			pos += dir switch {
				3 => new Vector2Int(0, -1),
				2 => new Vector2Int(-1, 0),
				1 => new Vector2Int(0, 1),
				_ => new Vector2Int(1, 0),
			};

			dirI++;
			if (dirI >= steps) {
				dir++;
				dirI = 0;
				if (dir >= 4) dir = 0;
				if (dir % 2 == 0) {
					steps++;
				}
			}
		}

		[SerializeField, HideInInspector] List<RenderObject> shareds_keys;
		[SerializeField, HideInInspector] List<RenderObject> shareds_values;
		void ISerializationCallbackReceiver.OnBeforeSerialize() {
			shareds_keys = shareds.Keys.ToList();
			shareds_values = shareds.Values.ToList();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize() {
			shareds = shareds_keys.Zip(shareds_values, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v); ;
		}
	}

}


namespace Unitylity.Systems.RenderImages {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using Unitylity.Components.Extended;
	using Unitylity.Data;
	using Unitylity.Extensions;

#if UNITYLITY_SYSTEMS_RENDERIMAGES_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.RenderImages) + "/" + nameof(RenderObjects))]
#endif
	[DefaultExecutionOrder(-1)]
	public class RenderObjects : Singleton<RenderObjects> {

		[SerializeField] float distance = 200;

		[SerializeField, HideInInspector] Vector2Int pos;
		[SerializeField, HideInInspector] int steps = 1; // Steps for direction
		[SerializeField, HideInInspector] int dir; // 0 - 3
		[SerializeField, HideInInspector] int dirI; // Steps taken in a direction

		[SerializeField, HideInInspector] SerializedDictionary<RenderObject, RenderObject> shareds;
		[SerializeField, HideInInspector] SerializedDictionary<RenderObject, List<RenderObject>> pool;

		public RenderObject GetObject(RenderObject prefab) {
			var res = default(RenderObject);
			if (prefab.shared) {
				res = GetShared(prefab);
				if (prefab.poolSize != 0) res = GetPooled(prefab);
				if (res != null) return res;
				if (res == null) res = Instantiate(prefab, transform);
				shareds.Add(prefab, res);
			} else {
				if (prefab.poolSize != 0) res = GetPooled(prefab);
				if (res != null) return res;
				if (res == null) res = Instantiate(prefab, transform);
			}
			var scaledPos = pos.Mul(distance);
			res.transform.localPosition = scaledPos.x0y();
			AdvancePos();
			res.prefab = prefab;
			return res;
		}

		private RenderObject GetShared(RenderObject prefab) {
			shareds.TryGetValue(prefab, out var res);
			if (res == null) shareds.Remove(prefab);
			return res;
		}

		private RenderObject GetPooled(RenderObject prefab) {
			if (pool.TryGetValue(prefab, out var list)) {
				var res = list.Last();
				list.RemoveAt(list.Count - 1);
				if (!list.Any()) pool.Remove(prefab);
				res.gameObject.SetActive(true);
				res.OnPickedFromPool();
				if (res.prefab != prefab) Debug.LogWarning("Prefab mismatch!", res);
				return res;
			}
			return null;
		}

		internal bool Pool(RenderObject renderObject) {
			var prefab = renderObject.prefab;
			if (!pool.TryGetValue(prefab, out var list)) {
				list = new();
				pool[prefab] = list;
			}
			if (prefab.poolSize > 0 && prefab.poolSize <= list.Count) return false;
			list.Add(renderObject);
			return true;
		}

		internal bool Unpool(RenderObject renderObject) {
			var prefab = renderObject.prefab;
			if (pool.TryGetValue(prefab, out var list)) {
				var res = list.Remove(renderObject);
				if (!list.Any()) pool.Remove(prefab);
				return res;
			}
			return false;
		}

		internal void Cleanup(RenderObject renderObject) {
			var prefab = renderObject.prefab;
			shareds.Remove(prefab);
			if (prefab.poolSize != 0) shareds.Remove(prefab);
		}

		private void AdvancePos() {

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

	}

}

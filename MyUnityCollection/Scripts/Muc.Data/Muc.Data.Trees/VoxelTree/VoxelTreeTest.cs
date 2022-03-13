﻿
namespace Muc.Data.Trees {

	using UnityEngine;

	internal class VoxelTreeTest : MonoBehaviour {

		public bool update;
		public bool alwaysUpdate;

		public int treeDebth;

		public bool drawing;
		public Vector3Int drawPos;
		public Color drawColor = Color.green;

		public int maxRendered = 5000;
		public float minLineWidth = 2;
		public float maxLineWidth = 3;

		[HideInInspector]
		public int _treeDebth;
		internal VoxelTree<Data> tree;

		public void OnValidate() {
			if (update || alwaysUpdate) {
				update = false;
				tree = new VoxelTree<Data>(treeDebth);
				_treeDebth = treeDebth;
			}
			if (drawing) {
				tree[drawPos.x, drawPos.y, drawPos.z] = new Data() { color = drawColor };
			}
		}

		public class Data {
			public Color color;
		}
	}
}


#if UNITY_EDITOR
namespace Muc.Data.Trees {

	using System;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(VoxelTreeTest))]
	internal class VoxelTreeTestEditor : Editor {

		private VoxelTreeTest t => (VoxelTreeTest)target;

		protected virtual void OnSceneGUI() {
			if (t.tree != null && Event.current.GetTypeForControl(0) == EventType.Repaint) {
				Draw();
			}
		}


		void Draw() {

			var e = t.tree.GetDetailedEnumerator();
			int i = 0;
			while (e.MoveNext() && i++ < t.maxRendered) {

				var color = Color.white;
				color.a /= (e.depth * 2 + 1);
				var width = Mathf.Lerp(t.maxLineWidth, t.minLineWidth, e.depth / (float)(t._treeDebth + 1));

				var origin = t.transform.position + new Vector3(e.currentOrigin.x * t.transform.lossyScale.x, e.currentOrigin.y * t.transform.lossyScale.y, e.currentOrigin.z * t.transform.lossyScale.z);
				var size = e.currentSize * t.transform.lossyScale;

				if (e.Current.isLeaf) {
					DrawLeaf(origin, size, color, width);
				} else {
					if (i == 1) DrawLeaf(origin, size, color, width);
					DrawParent(origin, size, color, width);
				}

				if (e.depth < t._treeDebth) continue;

				var voxelColor = e.Current.data == null ? (Color.red * 0.25f) : e.Current.data.color;
				DrawVoxel(origin, size, voxelColor);
			}
		}

		private void DrawVoxel(Vector3 origin, Vector3 size, Color color) {
			var prevColor = Handles.color;
			Handles.color = color;
			Handles.CubeHandleCap(0, origin + size / 2, Quaternion.identity, size.x, EventType.Repaint);
			Handles.color = prevColor;
		}


		void DrawLeaf(Vector3 origin, Vector3 size, Color color, float width) {
			var prevColor = Handles.color;
			Handles.color = color;

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 0),
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 0),
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 0),
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 0),
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 0),
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 1)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 1),
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 0)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 0)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 0)
			);

			Handles.color = prevColor;
		}

		void DrawParent(Vector3 origin, Vector3 size, Color color, float width) {
			var prevColor = Handles.color;
			Handles.color = color;

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0.5f, size.y * 0, size.z * 0),
				origin + new Vector3(size.x * 0.5f, size.y * 1, size.z * 0),
				origin + new Vector3(size.x * 0.5f, size.y * 1, size.z * 1),
				origin + new Vector3(size.x * 0.5f, size.y * 0, size.z * 1),
				origin + new Vector3(size.x * 0.5f, size.y * 0, size.z * 0)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0, size.y * 0.5f, size.z * 0),
				origin + new Vector3(size.x * 1, size.y * 0.5f, size.z * 0),
				origin + new Vector3(size.x * 1, size.y * 0.5f, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 0.5f, size.z * 1),
				origin + new Vector3(size.x * 0, size.y * 0.5f, size.z * 0)
			);

			Handles.DrawAAPolyLine(width,
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 0.5f),
				origin + new Vector3(size.x * 1, size.y * 0, size.z * 0.5f),
				origin + new Vector3(size.x * 1, size.y * 1, size.z * 0.5f),
				origin + new Vector3(size.x * 0, size.y * 1, size.z * 0.5f),
				origin + new Vector3(size.x * 0, size.y * 0, size.z * 0.5f)
			);

			Handles.color = prevColor;
		}


	}

}
#endif
